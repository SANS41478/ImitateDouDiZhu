using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPlayCommand : EventCommand
{
    [Inject]
    public CardModel CardModel { get; set; }
    public DeskControl DeskControl { get { return GameObject.FindObjectOfType<DeskControl>(); } }

    public override void Execute()
    {
        //发牌操作
        //UnityEngine.Debug.Log("发牌");
        //洗牌
        CardModel.Shuffle();
        DeskControl.StartCoroutine(DealCard());


    }

    IEnumerator DealCard()
    {
        //给每个人17张
        CharacterType curr = CharacterType.Player;
        for (int i = 0; i < 51; i++)
        {
            if (curr == CharacterType.Library || curr == CharacterType.Desk)
                curr = CharacterType.Player;
            FaPai(curr);
            curr++;
            yield return new WaitForSeconds(0.1f);
        }

        //地主牌 桌面发

        for (int i = 0; i < 3; i++)
        {
            FaPai(CharacterType.Desk);
        }
        yield return null; // 等一帧
        CardUI[] cardUIs = DeskControl.GetComponentsInChildren<CardUI>();
        Debug.Log("找到的 CardUI 数量：" + cardUIs.Length);
        foreach (var ui in cardUIs)
        {
            Debug.Log(ui.ToString());

            ui.SetImageAgain();
        }

        //发牌结束
        dispatcher.Dispatch(ViewEvent.CompleteFaPai);
    }

    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="cType"></param>
    void FaPai(CharacterType cType)
    {
        Card card = CardModel.FaPai(cType);
        FaPaiArg e = new FaPaiArg()
        {
            card = card,
            cType = cType,
            isSlect = false
        };
        DeskControl.StartCoroutine(DelayDispatch());

        IEnumerator DelayDispatch()
        {
            yield return null; // 等一帧
            dispatcher.Dispatch(ViewEvent.FaPai,e);
        }

    }
}
