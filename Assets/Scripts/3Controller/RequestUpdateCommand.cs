using System;
using strange.extensions.command.impl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RequestUpdateCommand: EventCommand
{
    [Inject]
    public IntergrationModel IntergrationModel { get; set; }
    override public void Execute()
    {
        GameData data = new GameData()
        {
            playerIntegration = IntergrationModel.PlayerIntegration,
            computerLeftIntegration = IntergrationModel.ComputerLeftIntegration,
            computerRightIntegration = IntergrationModel.ComputerRightIntegration
        };
        dispatcher.Dispatch(ViewEvent.UpdateIntegration, data);
    }
}
