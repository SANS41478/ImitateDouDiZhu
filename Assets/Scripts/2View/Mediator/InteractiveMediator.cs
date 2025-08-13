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

    }

    private void OnPlayClick()
    {
        dispatcher.Dispatch(CommandEvent.RequestPlay);
        InteractionView.DeactiveAll();
    }

    override public void OnRemove()
    {
        InteractionView.Play.onClick.RemoveListener(OnPlayClick);

    }
}
