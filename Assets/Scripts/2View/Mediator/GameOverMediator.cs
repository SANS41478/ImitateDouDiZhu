using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMediator : EventMediator
{
    [Inject]
    public GameOverView GameOverView { get; set; }

    public override void OnRegister()
    {
        dispatcher.AddListener(ViewEvent.UpdateGameOver, OnUpdateGameOver);
        GameOverView.Btn.onClick.AddListener(OnRestartClick);

        dispatcher.Dispatch(CommandEvent.UpdateGameOver);
    }


    public override void OnRemove()
    {
        dispatcher.RemoveListener(ViewEvent.UpdateGameOver, OnUpdateGameOver);
        GameOverView.Btn.onClick.RemoveListener(OnRestartClick);

    }

    private void OnUpdateGameOver(IEvent evt)
    {
        GameOverShowArg e = (GameOverShowArg)evt.data;

        GameOverView.Init(e.isLandlord, e.isWin);
    }
    /// <summary>
    /// 重新开始
    /// </summary>
    private void OnRestartClick()
    {
        Destroy(this.gameObject);
        dispatcher.Dispatch(ViewEvent.RestartGame);
    }

}
