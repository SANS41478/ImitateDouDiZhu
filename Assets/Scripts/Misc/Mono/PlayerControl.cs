using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : CharacterBase
{
    public CharacterUI characterUI;
    Identity identity;

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
        //card.BelongTo = CharacterType.Player;
        base.AddCard(card, selected);
        // Update the UI to reflect the new card count
        characterUI.SetShouPai(CardCount);
    }
    public override Card DealCard()
    {
        Card card = base.DealCard();
        characterUI.SetShouPai(CardCount);
        return card;
    }
    /// <summary>
    /// 找到被选中的手牌
    /// </summary>
    /// <returns></returns>
    List<Card> selectedCards = new List<Card>();
    List<CardUI> selectedCardUIs = new List<CardUI>();
    public List<Card> FindSelectCard()
    {
        selectedCards.Clear();
        selectedCardUIs.Clear();
        CardUI[] cardUIs = CreatePoint.GetComponentsInChildren<CardUI>();

        foreach (var cardUI in cardUIs)
        {
            if (cardUI.IsSelected)
            {
                selectedCards.Add(cardUI.Card);
                selectedCardUIs.Add(cardUI);
            }
        }
        Tool.Sort(selectedCards, true);
        return selectedCards;
    }
    /// <summary>
    /// 删除手牌/成功出牌
    /// </summary>
    public void DestroySelectCard()
    {
        // 保证先刷新一次
        FindSelectCard();

        for (int i = 0; i < selectedCards.Count; i++)
        {
            selectedCardUIs[i].Destroy();
            CardList.Remove(selectedCards[i]);
        }

        SortCardUI(CardList);
        characterUI.SetShouPai(CardCount);

        // 出完牌后清空缓存
        selectedCards.Clear();
        selectedCardUIs.Clear();
    }

}
