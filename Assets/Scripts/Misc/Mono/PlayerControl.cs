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
        if (selectedCards == null || selectedCardUIs == null)
            return;
        else
        {

            for (int i = 0; i < selectedCards.Count; i++)
            {
                selectedCardUIs[i].Destroy();
                CardList.Remove(selectedCards[i]);
            }

            SortCardUI(CardList);
            characterUI.SetShouPai(CardCount);


        }
    }
}
