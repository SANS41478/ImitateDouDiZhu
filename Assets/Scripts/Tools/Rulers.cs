using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rulers : MonoBehaviour
{
    /// <summary>
    /// 可否出牌
    /// </summary>>
    /// <param name="cards">传入的牌</param>
    /// <param name="type">出牌类型</param>
    /// <returns>是否可以出牌</returns>
    //public static bool CanPop()
    //{

    //}
    //单牌
    public static bool IsSingle(List<Card> cards)
    {
        return cards.Count == 1;
    }
    //对子
    public static bool IsDouble(List<Card> cards)
    {
        if (cards.Count == 2)
        {
            if (cards[0].Cardweight == cards[1].Cardweight)
                return true;
        }
        return false;
    }
    //顺子
    public static bool IsStraight(List<Card> cards)
    {
        if (cards.Count < 5 || cards.Count > 12) return false;
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].Cardweight + 1 != cards[i + 1].Cardweight)
                return false;
            //不能超过ACE
            if (cards[i + 1].Cardweight >= Weight.Two)
                return false;
        }
        return true;
    }
    //双顺
    public static bool IsDoubleStraight(List<Card> cards)
    {
        if (cards.Count < 6 || cards.Count % 2 != 0)
            return false;

        for (int i = 0; i < cards.Count; i += 2)
        {
            if (cards[i].Cardweight != cards[i + 1].Cardweight)
                return false;

            if (cards[i].Cardweight >= Weight.Two)
                return false;
            //判断两对之间必须是连续的

            if (i > 0 && cards[i].Cardweight != cards[i - 2].Cardweight + 1)
                return false;
        }

        return true;
    }
    //三顺
    public static bool IsTripleStraight(List<Card> cards)
    {
        if (cards.Count < 6 || cards.Count % 3 != 0)
            return false;

        // 每三个一组，确保每组三张牌相等
        for (int i = 0; i < cards.Count; i += 3)
        {
            if (cards[i].Cardweight != cards[i + 1].Cardweight ||
                cards[i].Cardweight != cards[i + 2].Cardweight)
            {
                return false;
            }

            // 检查不能包含 2 和王（Weight.Two 及以上为非法）
            if (cards[i].Cardweight >= Weight.Two)
            {
                return false;
            }

            // 判断是否连续（跳过第一组）
            if (i > 0)
            {
                if (cards[i].Cardweight != cards[i - 3].Cardweight + 1)
                {
                    return false;
                }
            }
        }

        return true;
    }
    //三不带
    public static bool IsThreeWithoutPair(List<Card> cards)
    {
        return cards.Count == 3 &&
               cards[0].Cardweight == cards[1].Cardweight &&
               cards[1].Cardweight == cards[2].Cardweight;
    }
    //三带一
    public static bool IsTripleWithSingle(List<Card> cards)
    {
        if (cards.Count != 4)
            return false;

        // 情况一：三张在前 + 单张在后 例如 5558
        if (cards[0].Cardweight == cards[1].Cardweight &&
            cards[1].Cardweight == cards[2].Cardweight)
            return true;

        // 情况二：单张在前 + 三张在后 例如 8555
        if (cards[1].Cardweight == cards[2].Cardweight &&
            cards[2].Cardweight == cards[3].Cardweight)
            return true;

        return false;
    }
    //三带一对
    public static bool IsThreeWithAPair(List<Card> cards)
    {
        if (cards.Count != 5) return false;

        // 情况1: AAA BB
        bool case1 = cards[0].Cardweight == cards[2].Cardweight &&
                     cards[3].Cardweight == cards[4].Cardweight;

        // 情况2: BB AAA
        bool case2 = cards[0].Cardweight == cards[1].Cardweight &&
                     cards[2].Cardweight == cards[4].Cardweight;

        return case1 || case2;
    }

    /// <summary>
    /// 判断飞机带单翅膀：两组三张+两单牌（共 8 张）或三组三张+三单牌（共 12 张）
    /// </summary>
    public static bool IsPlaneWithSingleWings(List<Card> cards)
    {
        int total = cards.Count;
        int n;
        if (total == 8) n = 2;
        else if (total == 12) n = 3;
        else return false; // 只可能是 8 或 12 张

        // 按点数统计出现次数
        var countMap = cards.GroupBy(c => c.Cardweight)
                            .ToDictionary(g => g.Key, g => g.Count());

        // 只把恰好出现 3 次的点数当作三张组
        var triples = countMap.Where(kv => kv.Value == 3)
                              .Select(kv => kv.Key)
                              .OrderBy(w => w)
                              .ToList();

        // 必须正好 n 个三张组
        if (triples.Count != n)
            return false;

        // 检查这 n 个点数连续且都 < Two
        for (int i = 1; i < n; i++)
        {
            if (triples[i] != triples[i - 1] + 1 || triples[i] >= Weight.Two)
                return false;
        }

        // 减去三顺部分的 3*n 张
        foreach (var w in triples)
            countMap[w] -= 3;

        // 剩余牌必须正好 n 张，且每个点数只出现一次，且不能与 triples 重复
        int remainCount = countMap.Values.Sum();
        if (remainCount != n)
            return false;
        foreach (var kv in countMap)
        {
            if (kv.Value == 1)
            {
                // 单张翅膀点数不得是三顺点数
                if (triples.Contains(kv.Key))
                    return false;
            }
            else if (kv.Value != 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 判断飞机带对子翅膀：两组三张+两对子（共 10 张）或三组三张+三对子（共 15 张）
    /// 严格要求：三张组点数恰好出现 3 次（排除 4 张炸弹），翅膀对子不能与三顺重复
    /// </summary>
    public static bool IsPlaneWithPairWings(List<Card> cards)
    {
        int total = cards.Count;
        int n;
        if (total == 10) n = 2;
        else if (total == 15) n = 3;
        else return false; // 只可能是 10 或 15 张

        var countMap = cards.GroupBy(c => c.Cardweight)
                            .ToDictionary(g => g.Key, g => g.Count());

        var triples = countMap.Where(kv => kv.Value == 3)
                              .Select(kv => kv.Key)
                              .OrderBy(w => w)
                              .ToList();

        if (triples.Count != n)
            return false;
        for (int i = 1; i < n; i++)
        {
            if (triples[i] != triples[i - 1] + 1 || triples[i] >= Weight.Two)
                return false;
        }

        // 减去三顺部分的 3*n 张
        foreach (var w in triples)
            countMap[w] -= 3;

        // 剩余牌必须是 2*n 张，且每个点数只出现 2 次（对子），且点数不在 triples 里
        int remainCount = countMap.Values.Sum();
        if (remainCount != 2 * n)
            return false;
        foreach (var kv in countMap)
        {
            if (kv.Value == 2)
            {
                // 对翅膀点数不得是三顺点数
                if (triples.Contains(kv.Key))
                    return false;
            }
            else if (kv.Value != 0)
            {
                return false;
            }
        }

        return true;
    }
    //炸弹
    public static bool IsBomb(List<Card> cards)
    {
        // 必须恰好 4 张牌
        if (cards == null || cards.Count != 4)
            return false;

        // 取第一张的点数
        var w = cards[0].Cardweight;

        // 检查剩余三张是否和第一张点数相同
        for (int i = 1; i < 4; i++)
        {
            if (cards[i].Cardweight != w)
                return false;
        }

        return true;
    }
    //王炸
    public static bool IsJokerBomb(List<Card> cards)
    {
        // 必须恰好 2 张牌
        if (cards == null || cards.Count != 2)
            return false;
        // 检查是否是小王和大王
        return cards[0].Cardweight == Weight.SJoker && cards[1].Cardweight == Weight.BJoker;
    }
}