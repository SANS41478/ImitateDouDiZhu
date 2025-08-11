using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Tool 
{
    static Transform uiParent;
    public static Transform UiParent
    {
        get
        {
            if (uiParent == null)
            {
                uiParent = GameObject.Find("Canvas").transform;
            }


            return uiParent;
        }
    }
    /// <summary>
    /// Creates a panel of the specified type and returns the GameObject instance.
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    public static GameObject CreatedPanel(PanelType panelType) 
    {
        GameObject go = Resources.Load<GameObject>(panelType.ToString());
        if (go == null)
        {
            Debug.LogError($"Panel {panelType} not found in Resources.");
            return null;
        }

        Transform parent = UiParent; // 确保属性触发
        if (parent == null)
        {
            Debug.LogError("UiParent is null. Panel will not be attached to Canvas.");
        }
        GameObject gameObject = GameObject.Instantiate(go, parent, false);
        gameObject.name = panelType.ToString();
        return gameObject;
    }
    /// <summary>
    /// 对牌进行排序
    /// </summary>
    /// <param name="cards">要排序的牌</param>"
    /// <param name="asc">是否升序排序</param>"
    public static void Sort(List<Card> cards, bool asc)
    {
        cards.Sort((Card a, Card b) =>
        {
            if (asc)
                return a.Cardweight.CompareTo(b.Cardweight);
            else
                return -a.Cardweight.CompareTo(b.Cardweight);
        });
    }
    /// <summary>
    /// 获取牌的大小
    /// </summary>
    /// <param name="card">牌的大小</param>"
    /// <param name="cardTyoe">出牌类型</param>
    public static int GetWeight(List<Card> cards, CardType cardType)
    {
        // 假设传入的 cards 与 cardType 已经是合法对应的牌型（不再做合法性验证）
        if (cards == null || cards.Count == 0) return -1;

        // 辅助：权重 -> 出现次数
        Dictionary<int, int> count = new Dictionary<int, int>();
        foreach (var c in cards)
        {
            int w = (int)c.Cardweight;
            if (!count.ContainsKey(w)) count[w] = 0;
            count[w]++;
        }

        // 得到升序的权重列表（distinct）
        var weights = count.Keys.ToList();
        weights.Sort();

        switch (cardType)
        {
            case CardType.Single:
                // 假设合法，直接返回那张牌的权重
                return (int)cards[0].Cardweight;

            case CardType.Double:
                // 返回对子权重（任意一张即可）
                return (int)cards[0].Cardweight;

            case CardType.Straight:
                // 顺子：所有权重各为1，返回最小权重
                return weights.Min();

            case CardType.DoubleStraight:
                // 双顺：每个权重至少2，返回最小权重
                // 找出所有 count>=2 的权重，取最小
                return count.Where(kv => kv.Value >= 2).Select(kv => kv.Key).Min();

            case CardType.TripleStraight:
                // 三顺：每个权重至少3，返回最小权重
                return count.Where(kv => kv.Value >= 3).Select(kv => kv.Key).Min();

            case CardType.ThreeWithoutPair:
            case CardType.TripleWithSingle:
            case CardType.ThreeWithAPair:
                // 这类以三张组的权重为准：找到 count>=3 的权重，取最小
                return count.Where(kv => kv.Value >= 3).Select(kv => kv.Key).Min();

            case CardType.PlaneWithSingleWings:
            case CardType.PlaneWithPairWings:
                // 飞机类：以最小三张组权重为准（找到所有 count>=3 的权重，取最小）
                return count.Where(kv => kv.Value >= 3).Select(kv => kv.Key).Min();


            default:
                return -1;
        }
    }


}
