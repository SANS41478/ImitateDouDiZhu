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
    }



    override public void OnRemove()
    {
        dispatcher.RemoveListener(ViewEvent.FaPai, OnFaPai);
        dispatcher.RemoveListener(ViewEvent.CompleteFaPai, OnCompleteFaPai);
        dispatcher.RemoveListener(ViewEvent.FaDiZhu, OnFaDiZhu);

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
    /// ¥¶¿Ì∑¢≈∆
    /// </summary>
    /// <param name="evt"></param>
    private void OnFaPai(IEvent evt)
    {
        FaPaiArg e = (FaPaiArg)evt.data;
        view.AddCard(e.cType, e.card, e.isSlect, ShowPoint.Desk);
    }
}
