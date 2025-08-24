using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeskUI : MonoBehaviour
{
    Transform DiZhuPai;
    public Transform DiZhuPaiTransform
    {
        get
        {
            if (DiZhuPai == null)
                DiZhuPai = transform.Find("DiZhuPai").transform;
            return DiZhuPai;
        }
    }
    public CanvasGroup DiZhuPaiGroup { get { return DiZhuPaiTransform.GetComponent<CanvasGroup>(); } }
    /// <summary>
    /// 显示牌
    /// </summary>
    /// <param name="card">显示卡片信息</param>
    /// <param name="index">索引</param>"
    public void SetShowCard(Card card, int index)
    {
        Image[] diZhuCards = DiZhuPaiTransform.GetComponentsInChildren<Image>();
        diZhuCards[index].sprite = Resources.Load<Sprite>("Pokers/" + card.CardName);
        SetAlpha(1);
    }

    public void SetAlpha(int v)
    {
        DiZhuPaiGroup.alpha = v;
    }
}
