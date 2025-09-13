using strange.extensions.command.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameOverCommand : EventCommand
{

    [Inject]
    public RoundModel RoundModel { get; set; }

    [Inject]
    public IntergrationModel IntergrationModel { get; set; }

    [Inject]
    public CardModel CardModel { get; set; }

    public override void Execute()
    {
        int temp = IntergrationModel.Result;

        GameOverArg e = (GameOverArg)evt.data;
        //更新数据
        if (e.PlayerWin)
            IntergrationModel.PlayerIntegration += temp;
        else
            IntergrationModel.PlayerIntegration -= temp;
        if (e.ComputerLeftWin)
            IntergrationModel.ComputerLeftIntegration += temp;
        else
            IntergrationModel.ComputerLeftIntegration -= temp;
        if (e.ComputerRightWin)
            IntergrationModel.ComputerRightIntegration += temp;
        else
            IntergrationModel.ComputerRightIntegration -= temp;

        RoundModel.isLandlord = e.isLandlord;
        RoundModel.isWin = e.PlayerWin;

        //存储数据
        GameData data = new GameData()
        {
            computerLeftIntegration = IntergrationModel.ComputerLeftIntegration,
            computerRightIntegration = IntergrationModel.ComputerRightIntegration,
            playerIntegration = IntergrationModel.PlayerIntegration,
        };
        Tool.SaveData(data);


        //显示数据
        GameData gameDate = new GameData()
        {
            playerIntegration = IntergrationModel.PlayerIntegration,
            computerRightIntegration = IntergrationModel.ComputerRightIntegration,
            computerLeftIntegration = IntergrationModel.ComputerLeftIntegration,
        };
        dispatcher.Dispatch(ViewEvent.UpdateIntegration, gameDate);

        //添加面板
        Tool.CreatedPanel(PanelType.GameOverPanel);



        //清除游戏数据
        RoundModel.InitRound();
        CardModel.InitCardLibrary();
    }
}