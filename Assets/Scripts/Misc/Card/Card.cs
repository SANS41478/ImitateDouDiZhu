using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    string cardName;
    Weight cardweight;
    ColorType colorType;
    CharacterType belongTo;

    public Weight Cardweight { get => cardweight;}
    public string CardName { get => cardName;}
    public ColorType ColorType1 { get => colorType;}
    public CharacterType BelongTo { get { return belongTo; }  set { belongTo = value; } }
    public Card(string name, ColorType color, Weight weight,CharacterType type)
    {
        cardName = name;
        colorType = color;
        cardweight = weight;
        belongTo = type; //ƒ¨»œ Ù”⁄≈∆ø‚
    }
}
