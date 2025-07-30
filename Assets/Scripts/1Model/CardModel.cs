using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel 
{
    Queue<Card> CardLibrary = new Queue<Card>();
    public void InitCardLibrary()
    {
        for(int color = 1 ; color < 5 ; color++)
        {
            for (int weight = 0; weight < 13; weight++) 
            {
                ColorType c = (ColorType)color;
                Weight w = (Weight)weight;
                string name = c.ToString() + w.ToString();
                Card card = new Card(name, c, w, CharacterType.Library);
                CardLibrary.Enqueue(card);
            }
        }
        Card sJoker = new Card("SJoker", ColorType.None, Weight.SJoker, CharacterType.Library);
        Card bJoker = new Card("BJoker", ColorType.None, Weight.BJoker, CharacterType.Library);
        CardLibrary.Enqueue(sJoker);
        CardLibrary.Enqueue(bJoker);
    }
    /// <summary>
    /// 洗牌算法
    /// </summary>>
    public void Shuffle()
    {
        // 1. 转换为 List
        List<Card> list = new List<Card>(CardLibrary);

        // 2. 使用 Fisher-Yates 洗牌算法
        System.Random rng = new System.Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            Card temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        // 3. 清空原队列并重新入队
        CardLibrary.Clear();
        foreach (var card in list)
        {
            CardLibrary.Enqueue(card);
        }
    }
    /// <summary>
    /// 最开始发牌
    /// </summary>>
    ///<param name="sendTo">发给谁</param>
    public Card DealCard(CharacterType sendTo)
    {
        if (CardLibrary.Count == 0)
        {
            Debug.LogError("没有牌了");
            return null;
        }
        Card card = CardLibrary.Dequeue();
        card.BelongTo = sendTo; //设置归属
        return card;
    }
}
