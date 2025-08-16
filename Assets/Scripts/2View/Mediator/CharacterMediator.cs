using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMediator : EventMediator
{
    [Inject]
    public CharacterView view { get; set; }
    override public void OnRegister()
    {
        view.Init();
        dispatcher.AddListener(ViewEvent.FaPai, OnFaPai);
        dispatcher.AddListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        dispatcher.AddListener(ViewEvent.FaDiZhu,OnFaDiZhu);
        dispatcher.AddListener(ViewEvent.RequestDeal, OnRequestDeal);
        dispatcher.AddListener(ViewEvent.SuccessDeal, OnSuccessDeal);

    }


    override public void OnRemove()
    {
        dispatcher.RemoveListener(ViewEvent.FaPai, OnFaPai);
        dispatcher.RemoveListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        dispatcher.RemoveListener(ViewEvent.FaDiZhu, OnFaDiZhu);
        dispatcher.RemoveListener(ViewEvent.RequestDeal, OnRequestDeal);
        dispatcher.RemoveListener(ViewEvent.SuccessDeal, OnSuccessDeal);
    }

    private void OnSuccessDeal(IEvent payload)
    {
        List<Card> cardList = view.PlayerControl.FindSelectCard();
        //清空桌面
        view.DeskControl.Clear(ShowPoint.Player);
        //添加到桌面
        foreach (var card in cardList)
            view.DeskControl.AddCard(card, false, ShowPoint.Player);

        view.PlayerControl.DestroySelectCard();

        if (!view.PlayerControl.HasCard)
        {
            //游戏结束
            Identity r = view.ComputerRightControl.Identity;
            Identity l = view.ComputerLeftControl.Identity;
            Identity p = view.PlayerControl.Identity;
            GameOverArg eee = new GameOverArg()
            {
                ComputerRightWin = r == p ? true : false,
                ComputerLeftWin = l == p ? true : false,
                PlayerWin = true,
                isLandlord = p == Identity.Landlord,
            };
            //游戏结束
            dispatcher.Dispatch(CommandEvent.GameOver, eee);
        }

    }

    /// <summary>
    /// 玩家出牌调用
    private void OnRequestDeal()
    {
        //不能直接出牌
        //需要判断 
        List<Card> cardList = view.PlayerControl.FindSelectCard();
        CardType cardType;
        if (Rulers.CanPop(cardList, out cardType))
        {
            PlayCardArgs e = new PlayCardArgs()
            {
                CardType = cardType,
                CharacterType = CharacterType.Player,
                Length = cardList.Count,
                Weight = Tool.GetWeight(cardList, cardType)
            };

            dispatcher.Dispatch(CommandEvent.ChuPai, e);
            //牌不对的话，if没有判断成功
        }
        else
        {
            Debug.Log("牌不对！");
        }
    }

    private void OnFaDiZhu(IEvent evt)
    {
        GrabAndDisGrabArg e = (GrabAndDisGrabArg)evt.data;
        view.FaDiZhuPai(e.cType);
    }
    private void OnCompleteFaPai()
    {
        view.ComputerLeftControl.Sort(true);
        view.ComputerRightControl.Sort(true);
        view.DeskControl.Sort(true);
    }
    /// <summary>
    /// 处理发牌
    /// </summary>
    /// <param name="evt"></param>
    private void OnFaPai(IEvent evt)
    {
        FaPaiArg e = (FaPaiArg)evt.data;
        view.AddCard(e.cType, e.card, e.isSlect, ShowPoint.Desk);
    }
}
