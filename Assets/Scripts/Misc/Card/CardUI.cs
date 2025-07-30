using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardUI : MonoBehaviour
{
    Card card;
    Image image;
    bool isSelected;
    Button btn;
    public Card Card
    {
        get
        {
            return card;
        }
        set
        {
            card = value;
            SetImage();
        }
    }
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            if (card.BelongTo != CharacterType.Player || isSelected == value)
                return;
            if (value)
                transform.localPosition += Vector3.up * 10;
            else
                transform.localPosition -= Vector3.up * 10;
            isSelected = value;
        }
    }
    public void SetImage() 
    {
        if (card.BelongTo == CharacterType.Player || card.BelongTo == CharacterType.Desk)
        {
            image.sprite = Resources.Load<Sprite>("Pokers/" + card.CardName);
        }
        else 
        {
            image.sprite = Resources.Load<Sprite>("Pokers/FixedBack");
        }
    }
    public void SetImageAgain() 
    {
        image.sprite = Resources.Load<Sprite>("Pokers/CardBack");
    }

    /// <summary>
    /// 设置位移以及偏移
    /// </summary>>
    /// <param name="parent">父物体</param>
    /// <param name="index">子物体索引</param>
    public void SetPosition(Transform parent, int index)
    {
        transform.SetParent(parent,false);
        transform.SetSiblingIndex(index);
        if (card.BelongTo == CharacterType.Desk || card.BelongTo == CharacterType.Player)
        {
            transform.localPosition = Vector3.right * index * 25;
            //防止还原
            if (IsSelected)
            {
                transform.localPosition += Vector3.up * 10;
            }
        }
        else if (card.BelongTo == CharacterType.Left || card.BelongTo == CharacterType.Right) 
        {
            transform.localPosition = - Vector3.up * 8 * index + Vector3.left * 8 * index;
        }
    }
    /// <summary>
    /// 初始化数据
    /// </summary>>
    public void OnSpawn() 
    {
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
    }
    public void OnDespawn()
    {
        card = null;
        isSelected = false;
        image.sprite = null;
        btn.onClick.RemoveAllListeners();
    }
}
