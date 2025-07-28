using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCommand : Command
{
    [Inject]
    public IntergrationModel intergrationModel { get; set; }
    override public void Execute()
    {
        Tool.CreatedPanel(PanelType.StartPanel);
        //Initailize model
        intergrationModel.InitIntegration();

    }
}
