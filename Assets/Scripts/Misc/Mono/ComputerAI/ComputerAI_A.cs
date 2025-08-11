using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//初级ai，只会根据最低权重来寻找出牌方案
public class ComputerAI_A : MonoBehaviour
{
    /// <summary>
    /// 要出的牌
    /// </summary>
    public List<Card> selecCards = new List<Card>();

    public CardType currType = CardType.None;

    public void SmartSelectCards(List<Card> cards, CardType cardType, int weight, int length, bool isBiggest)
    {
        cardType = isBiggest ? CardType.None : cardType;
        currType = cardType;
        selecCards.Clear();

        switch (cardType)
        {
            case CardType.None:
                //随机出牌
                selecCards = FindSmallestCards(cards);
                break;
            case CardType.Single:
                selecCards = FindSingle(cards, weight);
                break;
            case CardType.Double:
                selecCards = FindDouble(cards, weight);
                break;
            case CardType.Straight:
                selecCards = FindStraight(cards, weight, length);
                if (selecCards.Count == 0)
                {
                    selecCards = FindBomb(cards, -1);
                    currType = CardType.Bomb;
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindJokerBoom(cards);
                        currType = CardType.JokerBomb;
                    }
                }
                break;
            case CardType.DoubleStraight:
                selecCards = FindDoubleStraight(cards, weight, length);
                if (selecCards.Count == 0)
                {
                    selecCards = FindBomb(cards, -1);
                    currType = CardType.Bomb;
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindJokerBoom(cards);
                        currType = CardType.JokerBomb;
                    }
                }
                break;
            case CardType.TripleStraight:
                selecCards = FindTripleStraight(cards, weight, length);
                if (selecCards.Count == 0)
                {
                    selecCards = FindBomb(cards, -1);
                    currType = CardType.Bomb;
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindJokerBoom(cards);
                        currType = CardType.JokerBomb;
                    }
                }
                break;
            case CardType.ThreeWithoutPair:
                selecCards = FindTripleOnly(cards, weight);
                break;
            case CardType.TripleWithSingle:
                selecCards = FindThreeWithSingle(cards, weight);
                break;
            case CardType.ThreeWithAPair:
                selecCards = FindThreeWithPair(cards, weight);
                break;
            case CardType.PlaneWithSingleWings:
                selecCards = FindPlaneWithSingleWings(cards, weight, length);
                if (selecCards.Count == 0)
                {
                    selecCards = FindBomb(cards, -1);
                    currType = CardType.Bomb;
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindJokerBoom(cards);
                        currType = CardType.JokerBomb;
                    }
                }
                break;
            case CardType.PlaneWithPairWings:
                selecCards = FindTripleStraight(cards, weight, length);
                if (selecCards.Count == 0)
                {
                    selecCards = FindBomb(cards, -1);
                    currType = CardType.Bomb;
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindJokerBoom(cards);
                        currType = CardType.JokerBomb;
                    }
                }
                break;
            case CardType.Bomb:
                selecCards = FindBomb(cards, weight);
                if (selecCards.Count == 0)
                {
                    selecCards = FindJokerBoom(cards);
                    currType = CardType.JokerBomb;
                }
                break;
            case CardType.JokerBomb:
                break;
            default:
                break;
        }
    }
    public List<Card> FindSmallestCards(List<Card> cards)
    {
        List<Card> select = new List<Card>();

        if (cards == null || cards.Count == 0) return select;

        // 按权重从小到大排序
        cards = cards.OrderBy(c => (int)c.Cardweight).ToList();

        // 1. 顺子：最长优先，从12开始递减到5
        for (int len = Math.Min(12, cards.Count); len >= 5; len--)
        {
            select = FindStraight(cards, -1, len);
            if (select.Count > 0) currType =CardType.Straight; return select;
        }

        // 2. 飞机带单翅膀：可能是 12、8 张
        foreach (int len in new int[] { 12, 8 })
        {
            if (cards.Count >= len)
            {
                select = FindPlaneWithSingleWings(cards, -1, len);
                if (select.Count > 0) currType = CardType.PlaneWithSingleWings; return select;
            }
        }

        // 3. 飞机带双翅膀：可能是 15、10 张
        foreach (int len in new int[] { 15, 10 })
        {
            if (cards.Count >= len)
            {
                select = FindPlaneWithPairWings(cards, -1, len);
                if (select.Count > 0) currType = CardType.PlaneWithPairWings; return select;
            }
        }

        // 4. 三带二（固定 5 张）
        if (cards.Count >= 5)
        {
            select = FindThreeWithPair(cards, -1);
            if (select.Count > 0) currType = CardType.ThreeWithAPair; return select;
        }

        // 5. 三带一（固定 4 张，允许拆对子）
        if (cards.Count >= 4)
        {
            select = FindThreeWithSingle(cards, -1);
            if (select.Count > 0) currType = CardType.TripleWithSingle; return select;
        }

        // 6. 对子（固定 2 张）
        if (cards.Count >= 2)
        {
            select = FindDouble(cards, -1);
            if (select.Count > 0) currType = CardType.Double; return select;
        }

        // 7. 单牌（固定 1 张）
        select = FindSingle(cards, -1);
        if (select.Count > 0) currType = CardType.Single; return select;

        // 8. 炸弹（固定 4 张）
        select = FindBomb(cards, -1);
        if (select.Count > 0) currType = CardType.Bomb; return select;

        // 9. 王炸（固定 2 张）
        select = FindJokerBoom(cards);
        if (select.Count > 0) currType = CardType.JokerBomb; return select;

        return select;
    }


    /// </summary>
    /// <param name="cards">排序好的手牌</param>
    /// <param name="weight">上家出牌大小</param>
    /// <returns></returns>
    public List<Card> FindSingle(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();

        for (int i = 0; i < cards.Count; i++)
        {
            if ((int)cards[i].Cardweight > weight)
            {
                select.Add(cards[i]);
                break;
            }
        }
        return select;
    }
    /// <summary>
    /// 对儿
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public List<Card> FindDouble(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if ((int)cards[i].Cardweight == (int)cards[i + 1].Cardweight)
            {
                if ((int)cards[i].Cardweight > weight)
                {
                    select.Add(cards[i]);
                    select.Add(cards[i + 1]);
                    break;
                }
            }

        }
        return select;

    }
    /// <summary>
    /// 找顺子
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="minWeight"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public List<Card> FindStraight(List<Card> cards, int minWeight, int length)
    {

        List<Card> select = new List<Card>();
        int counter = 1;
        //CARD的索引
        List<int> indexList = new List<int>();

        for (int i = 0; i < cards.Count - 4; i++)
        {
            int weight = (int)cards[i].Cardweight;

            if (weight > minWeight)
            {
                counter = 1;
                indexList.Clear();
                for (int j = i + 1; j < cards.Count; j++)
                {
                    if (cards[j].Cardweight > Weight.One)
                        break;

                    if ((int)cards[j].Cardweight - weight == counter)
                    {
                        counter++;
                        indexList.Add(j);
                    }

                    if (counter == length)
                        break;

                }
            }

            if (counter == length)
            {
                indexList.Insert(0, i);
                break;

            }
        }
        //加元素
        if (counter == length)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                select.Add(cards[indexList[i]]);
            }
        }

        return select;

    }
    /// <summary>
    /// 找双顺 556677
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="minWeight"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public List<Card> FindDoubleStraight(List<Card> cards, int minWeight, int length)
    {

        List<Card> select = new List<Card>();
        int counter = 0;
        //CARD的索引
        List<int> indexList = new List<int>();

        for (int i = 0; i < cards.Count - 4; i++)
        {
            int weight = (int)cards[i].Cardweight;

            if (weight > minWeight)
            {
                counter = 0;
                indexList.Clear();

                //游标 控制counter ++
                int temp = 0;
                for (int j = i + 1; j < cards.Count; j++)
                {
                    if (cards[j].Cardweight > Weight.One)
                        break;

                    if ((int)cards[j].Cardweight - weight == counter)
                    {
                        temp++;

                        if (temp % 2 == 1)
                        {
                            counter++;
                        }
                        indexList.Add(j);
                    }

                    if (counter == length / 2)
                        break;

                }
            }

            if (counter == length / 2)
            {
                indexList.Insert(0, i);
                break;

            }
        }
        //加元素
        if (counter == length / 2)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                select.Add(cards[indexList[i]]);
            }
        }

        return select;

    }
    public List<Card> FindTripleStraight(List<Card> cards, int minWeight, int length) 
    {
        List<Card> select = new List<Card>();
        if (length <= 0 || cards == null || cards.Count == 0) return select;

        // 1) 建立权重 -> 索引 列表的映射（便于快速知道某权重出现了几张，以及取哪些牌）
        Dictionary<int, List<int>> idxMap = new Dictionary<int, List<int>>();
        for (int i = 0; i < cards.Count; i++)
        {
            int w = (int)cards[i].Cardweight;
            if (!idxMap.ContainsKey(w))
                idxMap[w] = new List<int>();
            idxMap[w].Add(i);
        }

        // 2) 得到所有存在的权重，并升序排序（与原 FindStraight 逻辑一致）
        var weights = idxMap.Keys.ToList();
        weights.Sort(); // 从小到大搜索起点

        // 3) 遍历可能的起点权重
        for (int wi = 0; wi < weights.Count; wi++)
        {
            int start = weights[wi];
            if (start <= minWeight) continue;            // 起点权重大于 minWeight

            // 起点必须至少有 3 张牌才能构成一个三张
            if (idxMap[start].Count < 3) continue;

            bool ok = true;
            // 检查之后 length-1 个连续权重是否都各自至少有 3 张
            for (int k = 1; k < length; k++)
            {
                int wt = start + k;

                // 如果你项目里有“不能用 2 或王”的限制（比如 Weight.One），在这里加判断：
                if ((Weight)wt > Weight.One) { ok = false; break; }

                if (!idxMap.ContainsKey(wt) || idxMap[wt].Count < 3)
                {
                    ok = false;
                    break;
                }
            }

            // 如果成功找到 length 个连续的三张
            if (ok)
            {
                for (int k = 0; k < length; k++)
                {
                    int wt = start + k;
                    // 这里取每个权重的前三张（一般牌组中三顺的三张就是任意三张同点数）
                    // 若你希望优先取"更高"或"特定花色"，可以在 idxMap[wt] 中调整索引选择策略
                    for (int t = 0; t < 3; t++)
                    {
                        select.Add(cards[idxMap[wt][t]]);
                    }
                }
                break; // 找到第一组符合要求的三顺后退出（和你的 FindStraight 一致）
            }
        }

        return select; // 没找到则为空列表
    }
    public List<Card> FindTripleOnly(List<Card> cards, int minWeight)
    {
        List<Card> select = new List<Card>();
        if (cards == null || cards.Count < 3) return select;

        // 建立权重 -> 索引 列表的映射
        Dictionary<int, List<int>> idxMap = new Dictionary<int, List<int>>();
        for (int i = 0; i < cards.Count; i++)
        {
            int w = (int)cards[i].Cardweight;
            if (!idxMap.ContainsKey(w)) idxMap[w] = new List<int>();
            idxMap[w].Add(i);
        }

        // 遍历存在的权重，按照升序（可改为降序以优先更大的三张）
        var weights = idxMap.Keys.ToList();
        weights.Sort();

        foreach (var w in weights)
        {
            if (w <= minWeight) continue;          // 必须大于 minWeight
                                                   // 如果你项目中需要排除 2 或 王，按项目的 Weight 枚举在此处加额外判断：
            if ((Weight)w > Weight.Two) continue;

            if (idxMap[w].Count >= 3)
            {
                // 取出现的前三张作为三不带（你也可以按花色/位置偏好选择）
                select.Add(cards[idxMap[w][0]]);
                select.Add(cards[idxMap[w][1]]);
                select.Add(cards[idxMap[w][2]]);
                break; // 找到第一个符合条件的三不带就返回
            }
        }

        return select;
    }
    // 专用版：三带一（length == 4）的简洁实现，允许拆对子提供单牌，但不拆三张或炸弹
    public List<Card> FindThreeWithSingle(List<Card> cards, int minWeight)
    {
        var select = new List<Card>();
        if (cards == null || cards.Count < 4) return select;

        // 建映射
        Dictionary<int, List<int>> idxMap = new Dictionary<int, List<int>>();
        for (int i = 0; i < cards.Count; i++)
        {
            int w = (int)cards[i].Cardweight;
            if (!idxMap.ContainsKey(w)) idxMap[w] = new List<int>();
            idxMap[w].Add(i);
        }

        var weights = idxMap.Keys.ToList();
        weights.Sort();

        foreach (var w in weights)
        {
            if (w <= minWeight) continue;
            if (idxMap[w].Count < 3) continue; // 需要至少 3 张组成三张

            // 三张索引
            int t0 = idxMap[w][0], t1 = idxMap[w][1], t2 = idxMap[w][2];

            // 先找单张（count == 1），按权重最小
            int singleIdx = -1;
            foreach (var kv in idxMap.OrderBy(kv => kv.Key))
            {
                if (kv.Key == w) continue;
                if (kv.Value.Count == 1)
                {
                    singleIdx = kv.Value[0];
                    break;
                }
            }

            // 若没有现成单张，则允许拆对子（count == 2）取一张作为单牌（但不拆 count >= 3）
            if (singleIdx == -1)
            {
                foreach (var kv in idxMap.OrderBy(kv => kv.Key))
                {
                    if (kv.Key == w) continue;
                    if (kv.Value.Count == 2)
                    {
                        singleIdx = kv.Value[0]; // 从对子取一张
                        break;
                    }
                }
            }

            if (singleIdx != -1)
            {
                select.Add(cards[t0]);
                select.Add(cards[t1]);
                select.Add(cards[t2]);
                select.Add(cards[singleIdx]);
                return select;
            }
            // else 继续尝试下一个三张点数
        }

        return select;
    }


    public List<Card> FindThreeWithPair(List<Card> cards, int minWeight)
    {
        List<Card> select = new List<Card>();
        if (cards == null || cards.Count < 5) return select;

        // 建立权重 -> 索引 列表 的映射
        Dictionary<int, List<int>> idxMap = new Dictionary<int, List<int>>();
        for (int i = 0; i < cards.Count; i++)
        {
            int w = (int)cards[i].Cardweight;
            if (!idxMap.ContainsKey(w)) idxMap[w] = new List<int>();
            idxMap[w].Add(i);
        }

        // 升序权重列表（若需优先更大三张，改为降序）
        var weights = idxMap.Keys.ToList();
        weights.Sort();

        // 遍历可能作为三张的权重（优先更小的三张）
        foreach (var w in weights)
        {
            if (w <= minWeight) continue;      // 起点必须大于 minWeight
            if (idxMap[w].Count < 3) continue; // 需要至少 3 张来做三张

            // 三张索引（取前三张）
            int t0 = idxMap[w][0];
            int t1 = idxMap[w][1];
            int t2 = idxMap[w][2];

            // 在其它权重中寻找现成的对子（严格 count == 2），并选择权重最小的那个
            int pairWeight = int.MaxValue;
            foreach (var kv in idxMap)
            {
                int wt = kv.Key;
                if (wt == w) continue;           // 不能与三张相同点数
                if (kv.Value.Count == 2)         // 只接受恰好为2的对子（不拆三张/四张）
                {
                    if (wt < pairWeight) pairWeight = wt;
                }
            }

            // 找到合适对子则组成三代二返回；否则继续尝试下一个三张点数
            if (pairWeight != int.MaxValue)
            {
                select.Add(cards[t0]);
                select.Add(cards[t1]);
                select.Add(cards[t2]);

                // 取该 pairWeight 的两张（顺序按 idxMap 中的索引）
                select.Add(cards[idxMap[pairWeight][0]]);
                select.Add(cards[idxMap[pairWeight][1]]);

                return select;
            }
        }

        // 没找到（不允许拆三/拆四时找不到合适对子）
        return select;
    }
    // 飞机带单翅膀，允许拆对子提供单牌，但不拆三张或炸弹
    public List<Card> FindPlaneWithSingleWings(List<Card> cards, int minWeight, int length)
    {
        var select = new List<Card>();
        if (cards == null || cards.Count < length) return select;
        if (length % 4 != 0) return select; // 每组三带一必须是4的倍数

        int trioCount = length / 4; // 三顺的长度（连续多少组三张）

        // 按权重分组：权重 -> 索引列表
        Dictionary<int, List<int>> idxMap = new Dictionary<int, List<int>>();
        for (int i = 0; i < cards.Count; i++)
        {
            int w = (int)cards[i].Cardweight;
            if (!idxMap.ContainsKey(w))
                idxMap[w] = new List<int>();
            idxMap[w].Add(i);
        }

        var weights = idxMap.Keys.Where(w => w <= (int)Weight.One).ToList(); // 排除 2 和王
        weights.Sort();

        // 查找连续三张组合
        for (int start = 0; start <= weights.Count - trioCount; start++)
        {
            int firstW = weights[start];
            if (firstW <= minWeight) continue;

            bool valid = true;
            List<int> trioIdxList = new List<int>();

            for (int offset = 0; offset < trioCount; offset++)
            {
                int curW = firstW + offset;
                if (!idxMap.ContainsKey(curW) || idxMap[curW].Count < 3)
                {
                    valid = false;
                    break;
                }
                trioIdxList.AddRange(idxMap[curW].Take(3));
            }

            if (!valid) continue;

            // 找翅膀
            List<int> singleWingIdx = new List<int>();

            // 1️⃣ 先找现成单牌（count == 1）
            foreach (var kv in idxMap.OrderBy(k => k.Key))
            {
                if (trioIdxList.Contains(kv.Value[0])) continue; // 不能用三张里的牌
                if (kv.Value.Count == 1)
                {
                    singleWingIdx.Add(kv.Value[0]);
                    if (singleWingIdx.Count == trioCount) break;
                }
            }

            // 2️⃣ 不够的话，从对子里拆一个（count == 2）
            if (singleWingIdx.Count < trioCount)
            {
                foreach (var kv in idxMap.OrderBy(k => k.Key))
                {
                    if (trioIdxList.Contains(kv.Value[0])) continue;
                    if (kv.Value.Count == 2)
                    {
                        singleWingIdx.Add(kv.Value[0]);
                        if (singleWingIdx.Count == trioCount) break;
                    }
                }
            }

            if (singleWingIdx.Count == trioCount)
            {
                // 组合最终牌
                select.AddRange(trioIdxList.Select(i => cards[i]));
                select.AddRange(singleWingIdx.Select(i => cards[i]));
                return select;
            }
        }

        return select;
    }
    // 飞机带双翅膀，允许拆对子提供翅膀，但不拆三张或炸弹
    public List<Card> FindPlaneWithPairWings(List<Card> cards, int minWeight, int length)
    {
        var select = new List<Card>();
        if (cards == null || cards.Count < length) return select;
        if (length % 5 != 0) return select; // 三带二必须是5的倍数

        int trioCount = length / 5; // 连续多少组三张

        // 按权重分组
        Dictionary<int, List<int>> idxMap = new Dictionary<int, List<int>>();
        for (int i = 0; i < cards.Count; i++)
        {
            int w = (int)cards[i].Cardweight;
            if (!idxMap.ContainsKey(w))
                idxMap[w] = new List<int>();
            idxMap[w].Add(i);
        }

        // 权重排序，排除 2 和王
        var weights = idxMap.Keys.Where(w => w <= (int)Weight.One).ToList();
        weights.Sort();

        // 找连续三张组合
        for (int start = 0; start <= weights.Count - trioCount; start++)
        {
            int firstW = weights[start];
            if (firstW <= minWeight) continue;

            bool valid = true;
            List<int> trioIdxList = new List<int>();

            for (int offset = 0; offset < trioCount; offset++)
            {
                int curW = firstW + offset;
                if (!idxMap.ContainsKey(curW) || idxMap[curW].Count < 3)
                {
                    valid = false;
                    break;
                }
                trioIdxList.AddRange(idxMap[curW].Take(3));
            }

            if (!valid) continue;

            // 找翅膀（对子）
            List<int> pairWingIdx = new List<int>();

            // 1️⃣ 先找现成对子（count == 2）
            foreach (var kv in idxMap.OrderBy(k => k.Key))
            {
                if (trioIdxList.Contains(kv.Value[0])) continue; // 不能用三张里的牌
                if (kv.Value.Count == 2)
                {
                    pairWingIdx.AddRange(kv.Value.Take(2));
                    if (pairWingIdx.Count == trioCount * 2) break;
                }
            }

            // 2️⃣ 不够的话，从三张里拆（不允许），从炸弹拆（不允许） → 直接跳过
            if (pairWingIdx.Count == trioCount * 2)
            {
                // 成功找到
                select.AddRange(trioIdxList.Select(i => cards[i]));
                select.AddRange(pairWingIdx.Select(i => cards[i]));
                return select;
            }
        }

        return select;
    }
    // 找炸弹（四张相同的牌），不考虑王炸
    public List<Card> FindBomb(List<Card> cards, int minWeight)
    {
        var select = new List<Card>();
        if (cards == null || cards.Count < 4) return select;

        // 按权重分组
        Dictionary<int, List<int>> idxMap = new Dictionary<int, List<int>>();
        for (int i = 0; i < cards.Count; i++)
        {
            int w = (int)cards[i].Cardweight;
            if (!idxMap.ContainsKey(w))
                idxMap[w] = new List<int>();
            idxMap[w].Add(i);
        }

        // 找最小的炸弹
        foreach (var kv in idxMap.OrderBy(k => k.Key))
        {
            if (kv.Key <= minWeight) continue;
            if (kv.Value.Count == 4) // 四张刚好
            {
                foreach (var idx in kv.Value)
                    select.Add(cards[idx]);
                return select;
            }
        }

        return select;
    }

    /// <summary>
    /// 王炸
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public List<Card> FindJokerBoom(List<Card> cards)
    {
        List<Card> select = new List<Card>();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].Cardweight == Weight.SJoker
                && cards[i + 1].Cardweight == Weight.BJoker)
            {
                select.Add(cards[i]);
                select.Add(cards[i + 1]);
                break;
            }
        }
        return select;

    }


}
