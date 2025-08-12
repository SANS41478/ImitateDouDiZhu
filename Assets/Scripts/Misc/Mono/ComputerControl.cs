using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControl :CharacterBase
{
    public CharacterUI characterUI;

    public CanvasGroup group;

    public ComputerAI_A ComputerAI;
    /// <summary>
    /// 当前要出的牌
    /// </summary>
    public List<Card> SelectCards
    {
        get { return ComputerAI.selectCards; }
    }
    /// <summary>
    /// 当前出牌类型
    /// </summary>
    public CardType CurrType
    {
        get { return ComputerAI.currType; }
    }

    Identity identity;

    /// <summary>
    /// 角色身份
    /// </summary>
    public Identity Identity
    {
        get
        {
            return identity;
        }

        set
        {
            identity = value;
            characterUI.SetIdentity(value);
        }
    }

    public override void AddCard(Card card, bool selected)
    {
        base.AddCard(card, selected);
        characterUI.SetShouPai(CardCount);

    }

    public override Card DealCard()
    {
        Card card = base.DealCard();
        characterUI.SetShouPai(CardCount);
        return card;
    }
    /// <summary>
    /// 电脑出牌
    /// </summary>
    /// <param name="cards">手牌</param>
    /// <param name="cardType">出牌当前类型</param>
    /// <param name="weight">出牌大小</param>
    /// <param name="length">长度</param>
    /// <param name="isBiggest">是不是最大的人</param>
    public bool SmartSelectCards(CardType cardType, int weight, int length, bool isBiggest)
    {
        ComputerAI.SmartSelectCards(CardList, cardType, weight, length, isBiggest);
        if (SelectCards.Count != 0)
        {
            //删除手牌
            DestroyCards();
            return true;
        }
        else
        {
            ComputerPass();
            return false;
        }
    }

    private void DestroyCards()
    {
        //删数据
        //删UI
        CardUI[] cardsUI = CreatePoint.GetComponentsInChildren<CardUI>();
        for (int i = 0; i < cardsUI.Length; i++)
        {
            for (int j = 0; j < SelectCards.Count; j++)
            {
                //是否出牌和当前UI一样
                if (SelectCards[j] == cardsUI[i].Card)
                {
                    cardsUI[i].Destroy();
                    CardList.Remove(SelectCards[j]);
                }

            }
        }
        //UI排序
        SortCardUI(CardList);
        characterUI.SetShouPai(CardCount);

    }

    /// <summary>
    /// 显示passUI
    /// </summary>
    public void ComputerPass()
    {
        group.alpha = 1;
        StartCoroutine(Pass());
    }

    IEnumerator Pass()
    {
        yield return new WaitForSeconds(1.5f);
        group.alpha = 0;

    }
}
