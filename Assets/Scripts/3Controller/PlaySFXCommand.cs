using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlaySFXCommand : strange.extensions.command.impl.EventCommand
{
    [Inject]
    public RoundModel RoundModel { get; set; }
    public override void Execute()
    {
        if (RoundModel.BiggestCharacter == RoundModel.CurrentCharacter)
        {
            UnityEngine.Debug.Log("放音效时当前出牌者"+RoundModel.CurrentCharacter);
            UnityEngine.Debug.Log("放音效时最大出牌者"+RoundModel.BiggestCharacter);
            UnityEngine.Debug.Log("放音效时当前牌型"+RoundModel.CurrentType);

            switch (RoundModel.CurrentType) 
            {
                case CardType.Single:
                    switch (RoundModel.CurrentWeight) 
                    {
                        case 1:
                            AudioManager.Instance.PlaySFX("1");
                            UnityEngine.Debug.Log("一个A");
                            break;
                        case 2:
                            AudioManager.Instance.PlaySFX("2");
                            break;
                        case 3:
                            AudioManager.Instance.PlaySFX("3");
                            break;
                        case 4:
                            AudioManager.Instance.PlaySFX("4");
                            break;
                        case 5:
                            AudioManager.Instance.PlaySFX("5");
                            break;
                        case 6:
                            AudioManager.Instance.PlaySFX("6");
                            break;
                        case 7:
                            AudioManager.Instance.PlaySFX("7");
                            break;
                        case 8:
                            AudioManager.Instance.PlaySFX("8");
                            break;
                        case 9:
                            AudioManager.Instance.PlaySFX("9");
                            break;
                        case 10:
                            AudioManager.Instance.PlaySFX("10");
                            break;
                        case 11:
                            AudioManager.Instance.PlaySFX("11");
                            break;
                        case 12:
                            AudioManager.Instance.PlaySFX("12");
                            break;
                        case 13:
                            AudioManager.Instance.PlaySFX("13");
                            break;
                        case 14:
                            AudioManager.Instance.PlaySFX("xiaowang");
                            break;
                        case 15:
                            AudioManager.Instance.PlaySFX("dawang");
                            break;
                    }
                    break;
                case CardType.Double:
                    switch (RoundModel.CurrentWeight) 
                    {
                        case 1:
                            AudioManager.Instance.PlaySFX("dui1");
                            break;
                        case 2:
                            AudioManager.Instance.PlaySFX("dui2");
                            break;
                        case 3:
                            AudioManager.Instance.PlaySFX("dui3");
                            break;
                        case 4:
                            AudioManager.Instance.PlaySFX("dui4");
                            break;
                        case 5:
                            AudioManager.Instance.PlaySFX("dui5");
                            break;
                        case 6:
                            AudioManager.Instance.PlaySFX("dui6");
                            break;
                        case 7:
                            AudioManager.Instance.PlaySFX("dui7");
                            break;
                        case 8:
                            AudioManager.Instance.PlaySFX("dui8");
                            break;
                        case 9:
                            AudioManager.Instance.PlaySFX("dui9");
                            break;
                        case 10:
                            AudioManager.Instance.PlaySFX("dui10");
                            break;
                        case 11:
                            AudioManager.Instance.PlaySFX("dui11");
                            break;
                        case 12:
                            AudioManager.Instance.PlaySFX("dui12");
                            break;
                        case 13:
                            AudioManager.Instance.PlaySFX("dui13");
                            break;
                    }
                    break;
                case CardType.Straight:
                    AudioManager.Instance.PlaySFX("shunzi");
                    break;
                case CardType.DoubleStraight :
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
    }
}