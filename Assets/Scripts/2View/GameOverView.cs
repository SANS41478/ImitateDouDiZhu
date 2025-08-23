using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : View
{
    public Image showImg;
    public List<Sprite> showList;
    public Button Btn;
    /// <summary>   
    /// 显示结算界面
    /// </summary>
    /// <param name="isLandlord">是不是地主</param>
    /// <param name="isWin">赢了没</param>
    public void Init(bool isLandlord, bool isWin)
    {
        if (isLandlord)
        {
            if (isWin)
                showImg.sprite = showList[0];
            else
                showImg.sprite = showList[1];
        }
        else
        {
            if (isWin)
                showImg.sprite = showList[2];
            else
                showImg.sprite = showList[3];
        }
    }
}
