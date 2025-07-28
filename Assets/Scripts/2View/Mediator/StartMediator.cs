using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMediator : EventMediator 
{
    [Inject]
    public StartView view { get; set; }
    public override void OnRegister()
    {
        view.BuJia.onClick.AddListener(OnBuJiaClick);
        view.JiaBei.onClick.AddListener(OnJiaBeiClick);
    }
    public override void OnRemove()
    {
        view.BuJia.onClick.RemoveListener(OnBuJiaClick);
        view.JiaBei.onClick.RemoveListener(OnJiaBeiClick);
    }
    private void OnBuJiaClick() 
    {
        dispatcher.Dispatch(CommandEvent.ChangeMulitiple, 1);
        Destroy(view.gameObject);

    }
    private void OnJiaBeiClick()
    {
        dispatcher.Dispatch(CommandEvent.ChangeMulitiple, 2);
        Destroy(view.gameObject);
    }
}
