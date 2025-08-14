using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnButton : Button
{
    public event Action HighlightedBtn;
    public event Action PressedBtn;


    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        switch (state)
        {
            case SelectionState.Normal:
                break;
            case SelectionState.Highlighted:
                if (HighlightedBtn != null)
                    HighlightedBtn();
                break;
            case SelectionState.Pressed:
                if (PressedBtn != null)
                    PressedBtn();
                break;
            case SelectionState.Disabled:
                break;
            default:
                break;
        }
    }
}
