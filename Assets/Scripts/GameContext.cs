using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.context.impl;
using strange.extensions.context.api;

public class GameContext : MVCSContext
{
    public GameContext(MonoBehaviour view, bool autoMapping) : base(view, autoMapping)
    {
    }
    protected override void mapBindings()
    {
        //model
        injectionBinder.Bind<IntergrationModel>().To<IntergrationModel>().ToSingleton();
        //command
        commandBinder.Bind(ContextEvent.START).To<StartCommand>();
        commandBinder.Bind(CommandEvent.ChangeMulitiple).To<ChangeMulitipleCommand>();
        //view
        mediationBinder.Bind<StartView>().To<StartMediator>();
        
    }
}

