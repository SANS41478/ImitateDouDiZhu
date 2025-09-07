using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMediator : EventMediator
{
    // 音乐只触发一次
    private bool hasPlayedLiangzhangpaiBGM = false;

    // 每个角色的音效只触发一次
    private bool hasPlayedLiangzhangpaiSFX_Player = false;
    private bool hasPlayedLiangzhangpaiSFX_Left = false;
    private bool hasPlayedLiangzhangpaiSFX_Right = false;
    private bool hasPlayedYizhangpaiSFX_Player = false;
    private bool hasPlayedYizhangpaiSFX_Left = false;
    private bool hasPlayedYizhangpaiSFX_Right = false;

    [Inject]
    public CharacterView view { get; set; }
    [Inject]
    public RoundModel RoundModel { get; set; }
    override public void OnRegister()
    {
        view.Init();
        dispatcher.AddListener(ViewEvent.FaPai, OnFaPai);
        dispatcher.AddListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        dispatcher.AddListener(ViewEvent.FaDiZhu,OnFaDiZhu);
        dispatcher.AddListener(ViewEvent.RequestDeal, OnRequestDeal);
        dispatcher.AddListener(ViewEvent.SuccessDeal, OnSuccessDeal);
        dispatcher.AddListener(ViewEvent.UpdateIntegration, OnUpdateIntegration);
        dispatcher.AddListener(ViewEvent.RestartGame, OnRestartGame);
        dispatcher.AddListener(ViewEvent.AISuccessDeal, OnAISuccessDeal);
        dispatcher.Dispatch(CommandEvent.RequestUpdate);

        RoundModel.ComputerHandler += RoundModel_ComputerHandler;
        RoundModel.PlayerHandler += RoundModel_PlayerHandler;
    }

    override public void OnRemove()
    {
        dispatcher.RemoveListener(ViewEvent.FaPai, OnFaPai);
        dispatcher.RemoveListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        dispatcher.RemoveListener(ViewEvent.FaDiZhu, OnFaDiZhu);
        dispatcher.RemoveListener(ViewEvent.RequestDeal, OnRequestDeal);
        dispatcher.RemoveListener(ViewEvent.SuccessDeal, OnSuccessDeal);
        dispatcher.RemoveListener(ViewEvent.UpdateIntegration, OnUpdateIntegration);
        dispatcher.RemoveListener(ViewEvent.RestartGame, OnRestartGame);
        dispatcher.RemoveListener(ViewEvent.AISuccessDeal, OnAISuccessDeal);


        RoundModel.ComputerHandler -= RoundModel_ComputerHandler;
        RoundModel.PlayerHandler -= RoundModel_PlayerHandler;
    }

    private void OnRestartGame()
    {
        //对象池的回收
        Lean.Pool.LeanPool.DespawnAll();

        //数据移除
        view.PlayerControl.CardList.Clear();
        view.ComputerLeftControl.CardList.Clear();
        view.ComputerRightControl.CardList.Clear();
        view.DeskControl.CardList.Clear();

        //初始化UI
        view.Init();
        view.PlayerControl.characterUI.SetShouPai(0);
        view.ComputerLeftControl.characterUI.SetShouPai(0);
        view.ComputerRightControl.characterUI.SetShouPai(0);
        view.DeskControl.deskUI.SetAlpha(0);

    }

    private void OnUpdateIntegration(IEvent payload)
    {
        GameData gameData = (GameData)payload.data;
        view.PlayerControl.characterUI.SetScore(gameData.playerIntegration);
        view.ComputerLeftControl.characterUI.SetScore(gameData.computerLeftIntegration);
        view.ComputerRightControl.characterUI.SetScore(gameData.computerRightIntegration);
    }

    private void RoundModel_PlayerHandler(bool obj)
    {
        view.DeskControl.Clear(ShowPoint.Player);

    }

    private void RoundModel_ComputerHandler(ComputerSmartArgs e)
    {
        StartCoroutine(Delay(e));
    }
    private IEnumerator Delay(ComputerSmartArgs e)
    {
        if (view == null) yield break;

        bool can = false;
        yield return new WaitForSeconds(1.4f);
        switch (e.CharacterType)
        {
            case CharacterType.Right:

                //清空桌面
                view.DeskControl.Clear(ShowPoint.Right);


                bool isLeader = (RoundModel.BiggestCharacter == CharacterType.Right);
                can = view.ComputerRightControl.SmartSelectCards(e.CardType, e.Weight, e.Length, isLeader);

                if (can)
                {
                    List<Card> cardList = view.ComputerRightControl.SelectCards;
                    CardType CurrType = view.ComputerRightControl.CurrType;

                    //添加牌桌面
                    foreach (var card in cardList)
                        view.DeskControl.AddCard(card, false, ShowPoint.Right);
                    PlayCardArgs ee = new PlayCardArgs()
                    {
                        CardType = CurrType,
                        Length = cardList.Count,
                        CharacterType = CharacterType.Right,
                        Weight = Tool.GetWeight(cardList, CurrType)
                    };

                    //判断胜负
                    if (!view.ComputerRightControl.HasCard)
                    {
                        Identity r = view.ComputerRightControl.Identity;
                        Identity l = view.ComputerLeftControl.Identity;
                        Identity p = view.PlayerControl.Identity;
                        GameOverArg eee = new GameOverArg()
                        {
                            ComputerRightWin = true,
                            ComputerLeftWin = l == r ? true : false,
                            PlayerWin = p == r ? true : false,
                            isLandlord = p == Identity.Landlord,

                        };
                        //游戏结束
                        dispatcher.Dispatch(CommandEvent.GameOver, eee);
                    }
                    else
                    {

                        dispatcher.Dispatch(CommandEvent.ChuPai, ee);

                    }
                }
                else
                {
                    dispatcher.Dispatch(CommandEvent.BuChu);
                }

                break;
            case CharacterType.Left:
                //清空桌面
                view.DeskControl.Clear(ShowPoint.Left);

                can = view.ComputerLeftControl.SmartSelectCards(e.CardType, e.Weight, e.Length,
                    e.IsBiggest == CharacterType.Left);
                if (can)
                {
                    List<Card> cardList = view.ComputerLeftControl.SelectCards;
                    CardType CurrType = view.ComputerLeftControl.CurrType;
                    //添加牌桌面
                    foreach (var card in cardList)
                        view.DeskControl.AddCard(card, false, ShowPoint.Left);
                    PlayCardArgs ee = new PlayCardArgs()
                    {
                        CardType = CurrType,
                        Length = cardList.Count,
                        CharacterType = CharacterType.Left,
                        Weight = Tool.GetWeight(cardList, CurrType)
                    };

                    //判断胜负
                    if (!view.ComputerLeftControl.HasCard)
                    {
                        //游戏结束
                        Identity r = view.ComputerRightControl.Identity;
                        Identity l = view.ComputerLeftControl.Identity;
                        Identity p = view.PlayerControl.Identity;
                        GameOverArg eee = new GameOverArg()
                        {
                            ComputerLeftWin = true,
                            ComputerRightWin = r == l ? true : false,
                            PlayerWin = p == l ? true : false,
                            isLandlord = p == Identity.Landlord,

                        };
                        //游戏结束
                        dispatcher.Dispatch(CommandEvent.GameOver, eee);
                    }
                    else
                    {

                        dispatcher.Dispatch(CommandEvent.ChuPai, ee);


                    }
                }
                else
                {
                    dispatcher.Dispatch(CommandEvent.BuChu);
                }
                break;

            default:
                break;
        }
    }


    private void OnSuccessDeal()
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
                PlayerWin = p == r || p == l ? false : true,

                isLandlord = p == Identity.Landlord,
            };
            //游戏结束
            dispatcher.Dispatch(CommandEvent.GameOver, eee);
        }
        CheckAndPlayAudio();
    }

    private void OnAISuccessDeal()
    {
        CheckAndPlayAudio();
    }

    public void CheckAndPlayAudio()
    {
        // 音乐只触发一次
        if (!hasPlayedLiangzhangpaiBGM &&
            (view.PlayerControl.CardCount <= 2 ||
             view.ComputerLeftControl.CardCount <= 2 ||
             view.ComputerRightControl.CardCount <= 2))
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic("liangzhangpaibgm");
            hasPlayedLiangzhangpaiBGM = true;
        }

        // 两张牌音效每角色只触发一次
        if (view.PlayerControl.CardCount == 2 && !hasPlayedLiangzhangpaiSFX_Player)
        {
            AudioManager.Instance.PlaySFX("liangzhangpai");
            hasPlayedLiangzhangpaiSFX_Player = true;
        }
        if (view.ComputerLeftControl.CardCount == 2 && !hasPlayedLiangzhangpaiSFX_Left)
        {
            AudioManager.Instance.PlaySFX("liangzhangpai");
            hasPlayedLiangzhangpaiSFX_Left = true;
        }
        if (view.ComputerRightControl.CardCount == 2 && !hasPlayedLiangzhangpaiSFX_Right)
        {
            AudioManager.Instance.PlaySFX("liangzhangpai");
            hasPlayedLiangzhangpaiSFX_Right = true;
        }

        // 一张牌音效每角色只触发一次
        if (view.PlayerControl.CardCount == 1 && !hasPlayedYizhangpaiSFX_Player)
        {
            AudioManager.Instance.PlaySFX("yizhangpai");
            hasPlayedYizhangpaiSFX_Player = true;
        }
        if (view.ComputerLeftControl.CardCount == 1 && !hasPlayedYizhangpaiSFX_Left)
        {
            AudioManager.Instance.PlaySFX("yizhangpai");
            hasPlayedYizhangpaiSFX_Left = true;
        }
        if (view.ComputerRightControl.CardCount == 1 && !hasPlayedYizhangpaiSFX_Right)
        {
            AudioManager.Instance.PlaySFX("yizhangpai");
            hasPlayedYizhangpaiSFX_Right = true;
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
