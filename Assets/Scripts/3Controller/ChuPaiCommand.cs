using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuPaiCommand : EventCommand
{
    [Inject]
    public RoundModel RoundModel { get; set; }
    [Inject]
    public IntergrationModel IntergrationModel { get; set; }
    public override void Execute() 
    {
        PlayCardArgs e = (PlayCardArgs)evt.data;

        if (e.CharacterType == CharacterType.Player)
        {
            if (e.CardType == RoundModel.CurrentType && e.Length == RoundModel.CurrentLength &&
                e.Weight > RoundModel.CurrentWeight)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else if (e.CardType == CardType.Bomb && RoundModel.CurrentType != CardType.Bomb)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else if (e.CardType == CardType.JokerBomb)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else if (e.CharacterType == RoundModel.BiggestCharacter)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else
            {
                UnityEngine.Debug.Log("重新选择");
                return;
            }

        }
        //更新数据
        RoundModel.BiggestCharacter = e.CharacterType;
        RoundModel.CurrentLength = e.Length;
        RoundModel.CurrentWeight = e.Weight;
        RoundModel.CurrentType = e.CardType;

        //积分翻倍
        if (e.CardType == CardType.Bomb)
            IntergrationModel.Mulitiple *= 2;
        else if (e.CardType == CardType.JokerBomb)
            IntergrationModel.Mulitiple *= 4;

        //换人
        RoundModel.Turn();


    }
}
