using UnityEngine;

public class BufferCard
{
    public int cardId;
    public string cardName;
    public string description;
    public BufferType type;
    public int value;
    public CardRarity rarity;
}

public enum BufferType
{
    AddAttack,      //增加攻击力
    AddMaxHp,       //增加最大血量
    AddDefense,     //减少受到伤害
    LifeSteal,      //攻击吸血
    HealAfterBettle //战后回血
}

public enum CardRarity
{
    Common,
    Rare,
    Epic
}
