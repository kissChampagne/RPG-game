using System;
using UnityEngine;

public class FightUnit : MonoBehaviour
{
    //单元属性
    public string UnitName;
    public int Health;
    public int maxHp;
    public UnitStatus Status;
    //状态
    public bool IsDefending = false;
    //基础属性
    public int baseAttack = 10;
    public int baseMaxHp = 100;
    public int baseDefense = 0;
    //buff加成
    public int attackBonus;
    public int defendBonus;
    public int maxHpBonus;
    //最终属性
    public int FinalAttack => baseAttack + attackBonus;
    public int FinalMaxHp => baseMaxHp + maxHpBonus;
    public int FinalDefense => baseDefense + defendBonus;

    public event Action<int> OnHurt;
    public event Action<string> OnDeath;

    public Animator animator;
    public SpriteRenderer sr;

    public enum UnitStatus
    {
        Idle,
        Walk,
        Attack,
        Dead
    }
    public void TakeDamage(int damage)
    {
        if (Status == UnitStatus.Dead)
        {
            return;
        }
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
        OnHurt?.Invoke(damage);
        //触发受击动画
        animator.SetTrigger("Hit");

        if (Health <= 0 && Status != UnitStatus.Dead)
        {
            Status = UnitStatus.Dead;
            string msg = $"{UnitName} 死亡";
            OnDeath?.Invoke(msg);
            animator.SetBool("Death", true);
            sr.flipX = false;
        }
    }
    public void PlayerAttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    
    //回血方法
    public void CoverHeal(int h)
    {
        Health += h;
        if (Health > maxHp)
            Health = maxHp;
    }
}
