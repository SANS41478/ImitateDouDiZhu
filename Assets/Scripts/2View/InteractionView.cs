using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionView : View
{
    [SerializeField]
    public Button Play;
    [SerializeField]
    public Button Grab;
    [SerializeField]
    public Button DisGrab;
    [SerializeField]
    public Button Deal;
    [SerializeField]
    public Button Pass;

    //全部隐藏
    public void DeactiveAll()
    {
        Play.gameObject.SetActive(false);
        Grab.gameObject.SetActive(false);
        DisGrab.gameObject.SetActive(false);
        Deal.gameObject.SetActive(false);
        Pass.gameObject.SetActive(false);

    }
    //开始游戏
    public void KaiShiYouXi() 
    {
        Play.gameObject.SetActive(true);
        Grab.gameObject.SetActive(false);
        DisGrab.gameObject.SetActive(false);
        Deal.gameObject.SetActive(false);
        Pass.gameObject.SetActive(false);
    }
    public void QiangDiZhu() 
    {
        Play.gameObject.SetActive(false);
        Grab.gameObject.SetActive(true);
        DisGrab.gameObject.SetActive(true);
        Deal.gameObject.SetActive(false);
        Pass.gameObject.SetActive(false);
    }
    //出牌
    public void ChuPai(bool isActive = true)
    {
        Play.gameObject.SetActive(false);
        Grab.gameObject.SetActive(false);
        DisGrab.gameObject.SetActive(false);
        Deal.gameObject.SetActive(true);
        Pass.gameObject.SetActive(true);
        Pass.interactable = isActive;
    }
}
