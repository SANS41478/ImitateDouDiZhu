using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool 
{
    static Transform uiParent;
    public static Transform UiParent
    {
        get
        {
            if (uiParent == null)
            {
                uiParent = GameObject.Find("Canvas").transform;
            }


            return uiParent;
        }
    }
    /// <summary>
    /// Creates a panel of the specified type and returns the GameObject instance.
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    public static GameObject CreatedPanel(PanelType panelType) 
    {
        GameObject go = Resources.Load<GameObject>(panelType.ToString());
        if (go == null)
        {
            Debug.LogError($"Panel {panelType} not found in Resources.");
            return null;
        }

        Transform parent = UiParent; // 确保属性触发
        if (parent == null)
        {
            Debug.LogError("UiParent is null. Panel will not be attached to Canvas.");
        }
        GameObject gameObject = GameObject.Instantiate(go, parent, false);
        gameObject.name = panelType.ToString();
        return gameObject;
    }
    /// <summary>
    /// 对牌进行排序
    /// </summary>
    /// <param name="cards">要排序的牌</param>"
    /// <param name="asc">是否升序排序</param>"
    public static void Sort(List<Card> cards, bool asc)
    {
        cards.Sort((Card a, Card b) =>
        {
            if (asc)
                return a.Cardweight.CompareTo(b.Cardweight);
            else
                return -a.Cardweight.CompareTo(b.Cardweight);
        });
    }
    /// <summary>
    /// 获取牌的大小
    /// </summary>
    /// <param name="card">牌的大小</param>"
    /// <param name="cardTyoe">出牌类型</param>
    
}
