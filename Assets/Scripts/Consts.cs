using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consts 
{

}
public enum PanelType 
{
    StartPanel,
}
public enum CommandEvent 
{
    ChangeMulitiple, //改变倍数
}
public enum ViewEvent 
{

}
/// <summary>
/// 出牌类型
/// </summary>
public enum CardType 
{
    None,
    Single, //单 1
    Double, //对儿 2
    Straight, //顺子 5 - 12
    DoubleStraight, //双顺 >= 6
    TripleStraight, //飞机 >= 6 
    Three, //三不带 3
    ThreeAndOne, //三带一 4
    ThreeAndTwo, //三代二 5
    Boom, //炸弹 4
    JokerBoom //王炸 2
}
///<summary>
///牌的大小
///</summary>>
public enum Weight
{
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    One,
    Two,
    SJoker, //小王
    BJoker //大王
}
/// <summary>
/// 拥有者类型
/// </summary>>
public enum CharacterType
{
    Library, //牌库
    Player, //玩家
    Right,
    Left,
    Desk
}
/// <summary>
/// 花色
/// </summary>
public enum ColorType
{
    None, //无
    Club, //梅花
    Heart, //红桃
    Spade, //黑桃
    Square //方块
}
/// <summary>
/// 牌主身份
/// </summary>
public enum Identity
{
    Farmer, //农民
    Landlord //地主
}