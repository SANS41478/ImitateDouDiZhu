using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuChuCommand : EventCommand
{
    [Inject]
    public RoundModel RoundModel { get; set; }
    private System.Random random = new System.Random();
    
    override public void Execute()
    {
        RoundModel.Turn();
        int delay = random.Next(0,3);
        switch (delay) 
        {
            case 0:
                AudioManager.Instance.PlaySFX("buyao");
                break;
            case 1:
                AudioManager.Instance.PlaySFX("guo");
                break;
            case 2:
                AudioManager.Instance.PlaySFX("yaobuqi");
                break;
        }
    }
}
