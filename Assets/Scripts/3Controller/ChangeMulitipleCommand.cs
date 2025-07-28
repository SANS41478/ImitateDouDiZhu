using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMulitipleCommand : EventCommand
{
    [Inject]
    public IntergrationModel IntegrationModel { get; set; }
    public override void Execute()
    {
        IntegrationModel.Mulitiple*=(int)evt.data;
    }
}
