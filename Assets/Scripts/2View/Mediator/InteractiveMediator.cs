using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMediator : EventMediator
{
    [Inject]
    public InteractionView InteractionView { get; set; }

    [Inject]
    public RoundModel RoundModel { get; set; }
    override public void OnRegister()
    {

        InteractionView.Play.onClick.AddListener(OnPlayClick);
        InteractionView.Grab.onClick.AddListener(OnGrabDiZhu);
        InteractionView.DisGrab.onClick.AddListener(OnDisGrab);
        InteractionView.Deal.onClick.AddListener(OnDealClick);
        InteractionView.Pass.onClick.AddListener(OnPassClick);

        dispatcher.AddListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        dispatcher.AddListener(ViewEvent.SuccessDeal, OnSuccessDeal);
        dispatcher.AddListener(ViewEvent.RestartGame, OnRestartGame);

        RoundModel.PlayerHandler += RoundModel_PlayerHandler;

    }

    override public void OnRemove()
    {
        InteractionView.Play.onClick.RemoveListener(OnPlayClick);
        InteractionView.Grab.onClick.RemoveListener(OnGrabDiZhu);
        InteractionView.DisGrab.onClick.RemoveListener(OnDisGrab);
        InteractionView.Deal.onClick.RemoveListener(OnDealClick);
        InteractionView.Pass.onClick.RemoveListener(OnPassClick);

        dispatcher.RemoveListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        dispatcher.RemoveListener(ViewEvent.SuccessDeal, OnSuccessDeal);
        dispatcher.RemoveListener(ViewEvent.RestartGame, OnRestartGame);

        RoundModel.PlayerHandler -= RoundModel_PlayerHandler;

    }

    private void OnRestartGame(IEvent payload)
    {
        InteractionView.DeactiveAll();
        InteractionView.KaiShiYouXi();
    }

    private void OnPassClick()
    {
        InteractionView.DeactiveAll();
        dispatcher.Dispatch(CommandEvent.BuChu);

    }

    private void OnSuccessDeal(IEvent payload)
    {
        InteractionView.DeactiveAll();

    }

    private void OnDealClick()
    {
        dispatcher.Dispatch(ViewEvent.RequestDeal);
    }

    private void RoundModel_PlayerHandler(bool canClick)
    {
        InteractionView.ChuPai(canClick);
    }

    private void OnDisGrab()
    {
        InteractionView.DeactiveAll();
        CharacterType temp = (CharacterType)UnityEngine.Random.Range(2, 4);
        GrabAndDisGrabArg e = new GrabAndDisGrabArg()
        {
            cType = temp
        };
        dispatcher.Dispatch(CommandEvent.GrabDiZhu, e);
    }

    private void OnGrabDiZhu()
    {
        InteractionView.DeactiveAll();
        GrabAndDisGrabArg e = new GrabAndDisGrabArg()
        {
            cType = CharacterType.Player
        };
        dispatcher.Dispatch(CommandEvent.GrabDiZhu,e);
    }
    private void OnCompleteFaPai(IEvent payload)
    {
        InteractionView.QiangDiZhu();

    }

    private void OnPlayClick()
    {
        dispatcher.Dispatch(CommandEvent.RequestPlay);
        InteractionView.DeactiveAll();
    }


}
