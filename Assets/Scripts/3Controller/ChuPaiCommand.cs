using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuPaiCommand : EventCommand
{
    [Inject]
    public RoundModel RoundModel { get; set; }
    [Inject]
    public IntergrationModel IntergrationModel { get; set; }
    public override void Execute() 
    {
        PlayCardArgs e = (PlayCardArgs)evt.data;

        if (e.CharacterType == CharacterType.Player)
        {
            if (e.CardType == RoundModel.CurrentType && e.Length == RoundModel.CurrentLength &&
                e.Weight > RoundModel.CurrentWeight)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else if (e.CardType == CardType.Bomb && RoundModel.CurrentType != CardType.Bomb)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else if (e.CardType == CardType.JokerBomb)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else if (e.CharacterType == RoundModel.BiggestCharacter)
                dispatcher.Dispatch(ViewEvent.SuccessDeal);
            else
            {
                UnityEngine.Debug.Log("重新选择");
                Debug.Log(e.CardType);
                return;
            }

        }
        //更新数据
        RoundModel.BiggestCharacter = e.CharacterType;
        RoundModel.CurrentLength = e.Length;
        RoundModel.CurrentWeight = e.Weight;
        RoundModel.CurrentType = e.CardType;
        //Debug.Log("出牌更新数据后牌长 ：" + RoundModel.CurrentLength);
        //Debug.Log("出牌更新数据后最大出牌者 :" + RoundModel.BiggestCharacter);
        //Debug.Log("出牌更新数据后当前权重 :" + RoundModel.CurrentWeight);
        //Debug.Log("出牌更新数据后当前牌型 :" + RoundModel.CurrentType);
        if (RoundModel.BiggestCharacter == e.CharacterType)
        {
            //Debug.Log("放音效时当前出牌者" + RoundModel.CurrentCharacter);
            //Debug.Log("放音效时最大出牌者" + RoundModel.BiggestCharacter);
            //Debug.Log("放音效时当前牌型" + RoundModel.CurrentType);

            switch (RoundModel.CurrentType)
            {
                case CardType.Single:
                    switch (RoundModel.CurrentWeight)
                    {
                        case 0:
                            AudioManager.Instance.PlaySFX("3");
                            break;
                        case 1:
                            AudioManager.Instance.PlaySFX("4");
                            break;
                        case 2:
                            AudioManager.Instance.PlaySFX("5");
                            break;
                        case 3:
                            AudioManager.Instance.PlaySFX("6");
                            break;
                        case 4:
                            AudioManager.Instance.PlaySFX("7");
                            break;
                        case 5:
                            AudioManager.Instance.PlaySFX("8");
                            break;
                        case 6:
                            AudioManager.Instance.PlaySFX("9");
                            break;
                        case 7:
                            AudioManager.Instance.PlaySFX("10");
                            break;
                        case 8:
                            AudioManager.Instance.PlaySFX("11");
                            break;
                        case 9:
                            AudioManager.Instance.PlaySFX("12");
                            break;
                        case 10:
                            AudioManager.Instance.PlaySFX("13");
                            break;
                        case 11:
                            AudioManager.Instance.PlaySFX("1");
                            break;
                        case 12:
                            AudioManager.Instance.PlaySFX("2");
                            break;
                        case 13:
                            AudioManager.Instance.PlaySFX("xiaowang");
                            break;
                        case 14:
                            AudioManager.Instance.PlaySFX("dawang");
                            break;
                    }
                    break;
                case CardType.Double:
                    switch (RoundModel.CurrentWeight)
                    {
                        case 0:
                            AudioManager.Instance.PlaySFX("dui3");
                            break;
                        case 1:
                            AudioManager.Instance.PlaySFX("dui4");
                            break;
                        case 2:
                            AudioManager.Instance.PlaySFX("dui5");
                            break;
                        case 3:
                            AudioManager.Instance.PlaySFX("dui6");
                            break;
                        case 4:
                            AudioManager.Instance.PlaySFX("dui7");
                            break;
                        case 5:
                            AudioManager.Instance.PlaySFX("dui8");
                            break;
                        case 6:
                            AudioManager.Instance.PlaySFX("dui9");
                            break;
                        case 7:
                            AudioManager.Instance.PlaySFX("dui10");
                            break;
                        case 8:
                            AudioManager.Instance.PlaySFX("dui11");
                            break;
                        case 9:
                            AudioManager.Instance.PlaySFX("dui12");
                            break;
                        case 10:
                            AudioManager.Instance.PlaySFX("dui13");
                            break;
                        case 11:
                            AudioManager.Instance.PlaySFX("dui1");
                            break;
                        case 12:
                            AudioManager.Instance.PlaySFX("dui2");
                            break;
                    }
                    break;
                case CardType.Straight:
                    AudioManager.Instance.PlaySFX("shunzi");
                    break;
                case CardType.DoubleStraight:
                    AudioManager.Instance.PlaySFX("liandui");
                    break;
                case CardType.TripleStraight:
                case CardType.PlaneWithSingleWings:
                case CardType.PlaneWithPairWings:
                    AudioManager.Instance.PlaySFX("feiji");
                    break;
                case CardType.ThreeWithoutPair:
                    AudioManager.Instance.PlaySFX("sanbudai");
                    break;
                case CardType.TripleWithSingle:
                    AudioManager.Instance.PlaySFX("sandaiyi");
                    break;
                case CardType.ThreeWithAPair:
                    AudioManager.Instance.PlaySFX("sandaier");
                    break;
                case CardType.Bomb:
                    AudioManager.Instance.PlaySFX("zhadan");
                    break;
                case CardType.JokerBomb:
                    AudioManager.Instance.PlaySFX("wangzha");
                    break;
                default:
                    break;
            }
        }

        //积分翻倍
        if (e.CardType == CardType.Bomb)
            IntergrationModel.Mulitiple *= 2;
        else if (e.CardType == CardType.JokerBomb)
            IntergrationModel.Mulitiple *= 4;

        //换人
        RoundModel.Turn();


    }
}
