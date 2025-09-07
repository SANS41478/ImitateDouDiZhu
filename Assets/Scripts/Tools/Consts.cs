using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consts 
{
    public static string DataPath = Application.dataPath + @"\score.xml";

}
public enum PanelType 
{
    StartPanel,
    CharacterPanel, //角色选择面板
    InteractionPanel, //积分面板
    GameOverPanel, //游戏结束面板
}
public enum CommandEvent 
{
    ChangeMulitiple, //改变倍数
    RequestPlay,//请求开始游戏
    GrabDiZhu, //抢地主
    ChuPai, //出牌
    BuChu, //不出牌
    GameOver, //游戏结束
    RequestUpdate, //请求更新数据
    UpdateGameOver, //更新结算界面

}
public enum ViewEvent 
{
    FaPai,
    CompleteFaPai,
    FaDiZhu, //发地主牌
    RequestDeal,
    SuccessDeal, //出牌成功
    UpdateIntegration,
    UpdateGameOver, //更新结算界面
    RestartGame, //重新开始游戏
    AISuccessDeal,//电脑成功出牌
}
/// <summary>
/// 出牌类型
/// </summary>
public enum CardType 
{
    None,
    Single, //单 
    Double, //对子
    Straight, //顺子 
    DoubleStraight, //双顺 
    TripleStraight, //三顺
    ThreeWithoutPair, //三不带 
    TripleWithSingle, //三带一 
    ThreeWithAPair, //三代二 
    PlaneWithSingleWings,// 飞机带单翅膀
    PlaneWithPairWings,// 飞机带双翅膀
    Bomb, //炸弹 4
    JokerBomb //王炸 2
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
public enum ShowPoint
{
    Desk, //桌面
    Player, //玩家
    Right, //右边
    Left //左边
}