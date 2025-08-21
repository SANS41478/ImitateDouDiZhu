using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuChuCommand : EventCommand
{
    [Inject]
    public RoundModel RoundModel { get; set; }
    override public void Execute()
    {
        RoundModel.Turn();
    }
}
