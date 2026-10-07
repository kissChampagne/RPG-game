using System;

public abstract class Skills
{
    public string SkillName;
    public abstract void Execute(FightUnit caster, FightUnit target, Action<string> onLogCallback);
}

public class AttackSkill : Skills
{
    private int _minDmg;
    private int _maxDmg;
    private System.Random _random = new System.Random();

    public AttackSkill(string name, int minDmg, int maxDmg)
    {
        SkillName = name;
        _minDmg = minDmg;
        _maxDmg = maxDmg;
    }

    public override string ToString()
    {
        return SkillName;
    }

    public override void Execute(FightUnit caster, FightUnit target, Action<string> onLogCallBack)
    {
        int damage = _random.Next(_minDmg, _maxDmg + 1);
        //如果目标防御
        if (target.IsDefending)
        {
            string msg = $"{caster.UnitName} 发动了 {SkillName}, {target.UnitName} 挡住了攻击，防御取消";
            onLogCallBack?.Invoke(msg);
            target.IsDefending = false;
            caster.PlayerAttackAnim();
        }
        else
        {
            string msg = $"{caster.UnitName} 发动了 {SkillName}, 对 {target.UnitName} 造成 {damage} 伤害";
            onLogCallBack?.Invoke(msg);
            caster.PlayerAttackAnim();
            target.TakeDamage(damage);
        }
    }
}
public class DefendSkill : Skills
{
    public DefendSkill(string name)
    {
        SkillName = name;
    }

    public override string ToString()
    {
        return SkillName;
    }

    public override void Execute(FightUnit caster, FightUnit target, Action<string> onLogCallBack)
    {
        caster.IsDefending = true;
        string msg = $"{caster.UnitName} 发动了{SkillName}, 防御生效";
        onLogCallBack?.Invoke(msg);
    }
}