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
        injectionBinder.Bind<CardModel>().To<CardModel>().ToSingleton();
        injectionBinder.Bind<RoundModel>().To<RoundModel>().ToSingleton();

        //command
        commandBinder.Bind(ContextEvent.START).To<StartCommand>();
        commandBinder.Bind(CommandEvent.ChangeMulitiple).To<ChangeMulitipleCommand>();
        commandBinder.Bind(CommandEvent.RequestPlay).To<RequestPlayCommand>();
        commandBinder.Bind(CommandEvent.GrabDiZhu).To<GrabDiZhuCommand>();
        commandBinder.Bind(CommandEvent.ChuPai).To<ChuPaiCommand>();
        commandBinder.Bind(CommandEvent.BuChu).To<BuChuCommand>();
        commandBinder.Bind(CommandEvent.GameOver).To<GameOverCommand>();
        commandBinder.Bind(CommandEvent.RequestUpdate).To<RequestUpdateCommand>();

        //view
        mediationBinder.Bind<StartView>().To<StartMediator>();
        mediationBinder.Bind<InteractionView>().To<InteractionMediator>();
        mediationBinder.Bind<CharacterView>().To<CharacterMediator>();
        mediationBinder.Bind<GameOverView>().To<GameOverMediator>();

    }
}

