using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartCommand : Command
{
    [Inject]
    public IntergrationModel intergrationModel { get; set; }
    [Inject]
    public RoundModel RoundModel { get; set; }

    [Inject]
    public CardModel CardModel { get; set; }
    override public void Execute()
    {
        Tool.CreatedPanel(PanelType.StartPanel);
        //Initailize model
        intergrationModel.InitIntegration();
        RoundModel.InitRound();
        CardModel.InitCardLibrary();
    }
    public void GetData()
    {
        FileInfo info = new FileInfo(Consts.DataPath);
        if (info.Exists)
        {
            GameData data = Tool.GetData();
            intergrationModel.PlayerIntegration = data.playerIntegration;
            intergrationModel.ComputerRightIntegration = data.computerRightIntegration;
            intergrationModel.ComputerLeftIntegration = data.computerLeftIntegration;
        }
    }

}
