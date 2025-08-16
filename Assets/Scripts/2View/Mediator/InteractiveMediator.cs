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

        dispatcher.AddListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        RoundModel.PlayerHandler += RoundModel_PlayerHandler;

    }

    override public void OnRemove()
    {
        InteractionView.Play.onClick.RemoveListener(OnPlayClick);
        InteractionView.Grab.onClick.RemoveListener(OnGrabDiZhu);
        InteractionView.DisGrab.onClick.RemoveListener(OnDisGrab);
        InteractionView.Deal.onClick.RemoveListener(OnDealClick);

        dispatcher.RemoveListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        RoundModel.PlayerHandler -= RoundModel_PlayerHandler;

    }

    private void OnDealClick()
    {
        dispatcher.Dispatch(ViewEvent.RequestDeal);
    }

    private void RoundModel_PlayerHandler(bool canClick)
    {
        InteractionView.ChuPai(canClick);
        Debug.Log("InteractionMediator: " + canClick);
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
