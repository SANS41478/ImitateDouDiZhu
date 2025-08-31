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
        Tool.CreatedPanel(PanelType.CharacterPanel);
        Tool.CreatedPanel(PanelType.InteractionPanel);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("kaijvbgm");
        AudioManager.Instance.SetMusicVolume(0.7f);
    }
}
