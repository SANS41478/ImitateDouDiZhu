using strange.extensions.command.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 处理结算界面
/// </summary>
public class UpdateGameOverCommand : EventCommand
{

    [Inject]
    public RoundModel RoundModel { get; set; }
    public override void Execute()
    {
        GameOverShowArg e = new GameOverShowArg()
        {
            isLandlord = RoundModel.isLandlord,
            isWin = RoundModel.isWin,
        };

        dispatcher.Dispatch(ViewEvent.UpdateGameOver, e);
    }
}