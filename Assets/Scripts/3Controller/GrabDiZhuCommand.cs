using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDiZhuCommand : EventCommand
{
    [Inject]
    public RoundModel RoundModel { get; set; }

    public override void Execute()
    {
        AudioManager.Instance.PlaySFX("qiangdizhu");
        GrabAndDisGrabArg e = (GrabAndDisGrabArg)evt.data;
        //发地主牌
        dispatcher.Dispatch(ViewEvent.FaDiZhu, e);
        //地主开始游戏
        RoundModel.Start(e.cType);
    }
}
