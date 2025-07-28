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
}
