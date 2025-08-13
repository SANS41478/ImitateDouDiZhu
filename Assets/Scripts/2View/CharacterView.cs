using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : View
{
    public PlayerControl PlayerControl;
    public DeskControl DeskControl;
    public ComputerControl ComputerLeftControl;
    public ComputerControl ComputerRightControl;
    /// <summary>
    ///初始化UI
    /// </summary>
    public void Init()
    {
        PlayerControl.Identity = Identity.Farmer;
        ComputerLeftControl.Identity = Identity.Farmer;
        ComputerRightControl.Identity = Identity.Farmer;
    }
    /// <summary>
    /// 添加牌
    /// </summary>
    /// <param name="cType">给谁</param>
    /// <param name="card">发什么牌</param>
    /// <param name="isSelect">是否选中(地主牌</param>
    /// <param name="pos">桌子位置</param>
    public void AddCard(CharacterType cType, Card card, bool isSelect, ShowPoint pos)
    {
        switch (cType)
        {
            case CharacterType.Player:
                PlayerControl.AddCard(card, isSelect);
                PlayerControl.Sort(false);
                break;
            case CharacterType.Right:
                ComputerRightControl.AddCard(card, isSelect);
                break;
            case CharacterType.Left:
                ComputerLeftControl.AddCard(card, isSelect);
                break;
            case CharacterType.Desk:
                DeskControl.AddCard(card, isSelect, pos);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 添加三张地主牌
    /// </summary>
    /// <param name="cType"></param>
    public void FaDiZhuPai(CharacterType cType)
    {
        Card card = null;
        switch (cType)
        {
            case CharacterType.Player:
                for (int i = 0; i < 3; i++)
                {
                    card = DeskControl.DealCard();
                    //牌高出一截
                    PlayerControl.AddCard(card, true);
                    //更新到桌面上
                    DeskControl.SetShowCard(card, i);
                }
                PlayerControl.Identity = Identity.Landlord;
                PlayerControl.Sort(false);
                break;
            case CharacterType.Right:
                for (int i = 0; i < 3; i++)
                {
                    card = DeskControl.DealCard();
                    ComputerRightControl.AddCard(card, false);
                    DeskControl.SetShowCard(card, i);
                }
                ComputerRightControl.Identity = Identity.Landlord;
                ComputerRightControl.Sort(true);
                break;
            case CharacterType.Left:
                for (int i = 0; i < 3; i++)
                {
                    card = DeskControl.DealCard();
                    ComputerLeftControl.AddCard(card, false);
                    DeskControl.SetShowCard(card, i);
                }
                ComputerLeftControl.Identity = Identity.Landlord;
                ComputerLeftControl.Sort(true);
                break;

            default:
                break;
        }
        DeskControl.Clear(ShowPoint.Desk);
    }
}
