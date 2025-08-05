using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class CharacterBase : MonoBehaviour
{

    public CharacterType characterType;

    List<Card> cardList = new List<Card>();

    Transform createPoint;

    public GameObject prefab;
    /// <summary>
    /// 手牌
    /// </summary>
    public List<Card> CardList
    {
        get
        {
            return cardList;
        }

    }

    /// <summary>
    /// 是否有牌
    /// </summary>
    public bool HasCard
    {
        get { { return cardList.Count != 0; } }

    }

    public int CardCount { get { return cardList.Count; } }

    public Transform CreatePoint
    {
        get
        {
            if (createPoint == null)
                createPoint = transform.Find("Pai");
            return createPoint;
        }

    }

    /// <summary>
    /// 添加牌
    /// </summary>
    /// <param name="card">添加的牌</param>
    /// <param name="selected">要增高么</param>
    public virtual void AddCard(Card card, bool selected)
    {
        cardList.Add(card);
        //****//
        card.BelongTo = characterType;
        CreateCardUI(card, cardList.Count - 1, selected);
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="asc">升序？</param>
    public void Sort(bool asc)
    {
        Tool.Sort(cardList, asc);
        //UI
        SortCardUI(cardList);
    }
    /// <summary>
    /// 排序CardUI
    /// </summary>
    /// <param name="cards">有序list</param>
    public void SortCardUI(List<Card> cards)
    {
        CardUI[] cardUIs = CreatePoint.GetComponentsInChildren<CardUI>();
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cardUIs.Length; j++)
            {
                if (cardUIs[j].Card == cards[i])
                {
                    cardUIs[j].SetPosition(CreatePoint, i);
                }
            }
        }
    }

    /// <summary>
    /// 根据数据创建CardUI
    /// </summary>
    /// <param name="card">数据</param>
    /// <param name="index">索引</param>
    /// <param name="isSeleted">上升？</param>
    public void CreateCardUI(Card card, int index, bool isSeleted)
    {
        //对象池生成
        GameObject go = LeanPool.Spawn(prefab);
        go.name = characterType.ToString() + index.ToString();
        //设置位置和是否选中
        CardUI cardUI = go.GetComponent<CardUI>();
        cardUI.Card = card;
        cardUI.IsSelected = isSeleted;
        cardUI.SetPosition(CreatePoint, index);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <returns></returns>
    public virtual Card DealCard()
    {
        Card card = cardList[CardCount - 1];
        cardList.Remove(card);
        return card;
    }

}