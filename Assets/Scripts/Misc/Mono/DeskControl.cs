using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskControl : CharacterBase
{
    public DeskUI deskUI;

    /// <summary>
    /// 手牌列表
    /// </summary>
    List<Card> playercardList = new List<Card>();
    public List<Card> PlayercardList { get { return playercardList; } }

    List<Card> leftcardList = new List<Card>();
    public List<Card> LeftcardList { get { return leftcardList; } }

    List<Card> rightcardList = new List<Card>();
    public List<Card> RightcardList { get { return rightcardList; } }

    Transform playerPoint;
    public Transform PlayerPoint
    {
        get
        {
            if (playerPoint == null)
                playerPoint = transform.Find("PlayerPai");
            return playerPoint;
        }
    }

    Transform leftPoint;
    public Transform LeftPoint
    {
        get
        {
            if (leftPoint == null)
                leftPoint = transform.Find("LeftPai");
            return leftPoint;
        }
    }

    Transform rightPoint;
    public Transform RightPoint
    {
        get
        {
            if (rightPoint == null)
                rightPoint = transform.Find("RightPai");
            return rightPoint;
        }
    }
    public void SetShowCard(Card card, int index)
    {
        deskUI.SetShowCard(card, index);
    }
    /// <summary>
    /// 创建卡片UI
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    /// <param name="isSeleted"></param>
    /// <param name="pos"></param>
    public  void CreateCardUI(Card card, int index, bool isSeleted, ShowPoint pos)
    {
        //对象池生成
        GameObject go = LeanPool.Spawn(prefab);
        go.name = characterType.ToString() + index.ToString();
        //设置位置和是否选中
        CardUI cardUI = go.GetComponent<CardUI>();
        cardUI.Card = card;
        cardUI.IsSelected = isSeleted;
        switch (pos)
        {
            case ShowPoint.Desk:
                cardUI.SetPosition(CreatePoint, index);
                break;
            case ShowPoint.Player:
                cardUI.SetPosition(PlayerPoint, index);
                break;
            case ShowPoint.Right:
                cardUI.SetPosition(RightPoint, index);
                break;
            case ShowPoint.Left:
                cardUI.SetPosition(LeftPoint, index);
                break;
            default:
                break;
        }
        //cardUI.SetPosition(CreatePoint, index);
    }
    public void AddCard(Card card, bool selected, ShowPoint pos)
    {
        if (PlayercardList.Contains(card) || CardList.Contains(card)
         || LeftcardList.Contains(card) || RightcardList.Contains(card))
        {
            Debug.LogWarning($"[DeskControl] 试图重复添加同一张牌: {card}");
            return;
        }

        switch (pos)
        {
            case ShowPoint.Desk:
                CardList.Add(card);
                card.BelongTo = characterType;
                CreateCardUI(card, CardList.Count - 1, selected, pos);
                break;
            case ShowPoint.Player:
                PlayercardList.Add(card);
                card.BelongTo = characterType;
                CreateCardUI(card, PlayercardList.Count - 1, selected, pos);

                break;
            case ShowPoint.Right:
                RightcardList.Add(card);
                card.BelongTo = characterType;
                CreateCardUI(card, RightcardList.Count - 1, selected, pos);

                break;
            case ShowPoint.Left:
                LeftcardList.Add(card);
                card.BelongTo = characterType;
                CreateCardUI(card, LeftcardList.Count - 1, selected, pos);
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// 桌面清空
    /// </summary>
    /// <param name="pos"></param>
    public void Clear(ShowPoint pos)
    {
        switch (pos)
        {
            case ShowPoint.Desk:
                CardList.Clear();
                CardUI[] cardUIs = CreatePoint.GetComponentsInChildren<CardUI>();
                for (int i = 0; i < cardUIs.Length; i++)
                    cardUIs[i].Destroy();
                break;
            case ShowPoint.Player:
                PlayercardList.Clear();
                CardUI[] cardUIPlayer = PlayerPoint.GetComponentsInChildren<CardUI>();
                for (int i = 0; i < cardUIPlayer.Length; i++)
                    cardUIPlayer[i].Destroy();
                break;
            case ShowPoint.Right:
                RightcardList.Clear();
                CardUI[] cardUIRight = RightPoint.GetComponentsInChildren<CardUI>();
                for (int i = 0; i < cardUIRight.Length; i++)
                    cardUIRight[i].Destroy();
                break;
            case ShowPoint.Left:
                LeftcardList.Clear();
                CardUI[] cardUILeft = LeftPoint.GetComponentsInChildren<CardUI>();
                for (int i = 0; i < cardUILeft.Length; i++)
                    cardUILeft[i].Destroy();
                break;
            default:
                break;
        }
    }
}
