using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//控制玩家，电脑角色，积分显示
public class CharacterUI : MonoBehaviour
{
    public Image image;
    public Text score;
    public Text shoupai;
    public void SetIdentity(Identity identity)
    {
        if (identity == Identity.Farmer)
        { image.sprite = Resources.Load<Sprite>("Pokers/Role_Farmer"); }
        else if (identity == Identity.Landlord) 
        { image.sprite = Resources.Load<Sprite>("Pokers/Role_Landlord"); }

    }
    public void SetScore(int score) { this.score.text = score.ToString(); }
    public void SetShouPai(int ShouPai) { shoupai.text = "剩余手牌" + ShouPai.ToString(); }
}
