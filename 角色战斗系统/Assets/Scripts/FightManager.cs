using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public FightUnit playerUnit;
    public FightUnit monsterUnit;

    //UI文本
    public TMP_Text playerHpText;
    public TMP_Text monsterHpText;
    public TMP_Text logText;

    public RectTransform logContent;
    public ScrollRect logScrollRect;

    //血条
    public Image playerHpFill;
    public Image monsterHpFill;

    //技能按钮控件
    public GameObject skillBtnPrefab;
    public Transform skillListParent;
    private SkillItem[] _skillItems;

    private List<Skills> playerSkills;
    private List<Skills> monsterSkills;
    private System.Random _random = new System.Random();
    private bool IsPlayerTurn = true;

    //面板
    public GameObject DeadPanel;

    //战斗胜利事件
    public event System.Action OnBattleWin;
    void Start()
    {
        _random = new System.Random();
        //玩家技能
        playerSkills = new List<Skills>()
        {
            new AttackSkill("挥砍", 20, 40),
            new DefendSkill("格挡")
        };
        //怪物技能
        monsterSkills = new List<Skills>()
        {
            new AttackSkill("爪击", 99, 100),
            new DefendSkill("硬皮")
        };

        //自动生成技能按钮
        GenerateSkillBtns();

        //绑定死亡回调
        playerUnit.OnDeath += PlayerDied;
        monsterUnit.OnDeath += MonsterDied;

        RefreshHpUI();
        AddText("战斗开始");
    }
    //刷新血条
    void RefreshHpUI()
    {
        float playerPercent = (float)playerUnit.Health / 100f;
        playerHpText.text = $"{playerUnit.Health}";
        playerHpFill.fillAmount = playerPercent;
        float monsterPercent = (float)monsterUnit.Health / 200f;
        monsterHpText.text = monsterUnit.Health.ToString();
        monsterHpFill.fillAmount = monsterPercent;
    }
    //添加战斗日志
    void AddText(string msg)
    {
        logText.text += msg + "\n";
        logText.ForceMeshUpdate();
        //把Content高度设置为文字的高度
        logContent.sizeDelta = new Vector2(logContent.sizeDelta.x, logText.preferredHeight);
        logScrollRect.verticalNormalizedPosition = 0f;
    }
    //生成按钮
    void GenerateSkillBtns()
    {
        _skillItems = new SkillItem[playerSkills.Count];

        for (int i = 0;  i < playerSkills.Count; i++)
        {
            int t = i;
            GameObject btngb = Instantiate(skillBtnPrefab, skillListParent);
            SkillItem item = btngb.GetComponent<SkillItem>();
            item.Init(t, playerSkills[t].SkillName, UseSkill);
            _skillItems[i] = item;
        }
    }
    //设置按钮的可交互状态
    public void SetAllSkillBtnsInteractable(bool enable)
    {
        if (_skillItems == null) return;
        foreach (var item  in _skillItems)
        {
            item.SetInteractable(enable);
        }
    }
    void UseSkill(int skillIndex)
    {
        if (playerUnit.Status == FightUnit.UnitStatus.Dead 
            || monsterUnit.Status == FightUnit.UnitStatus.Dead
            || !IsPlayerTurn)
        {
            return;
        }
        playerSkills[skillIndex].Execute(playerUnit, monsterUnit, AddText);
        RefreshHpUI();
        if (monsterUnit.Status != FightUnit.UnitStatus.Dead)
        {
            IsPlayerTurn = false;
            SetAllSkillBtnsInteractable(false);
            Invoke(nameof(MonsterTurn), 0.8f);
        }
    }
    //重置战斗状态
    public void ResetBattle()
    {
        //重置玩家状态
        playerUnit.Health = 100;
        playerUnit.Status = FightUnit.UnitStatus.Idle;
        playerUnit.IsDefending = false;

        //重置怪物状态
        monsterUnit.Health = 200;
        monsterUnit.Status = FightUnit.UnitStatus.Idle;
        monsterUnit.IsDefending = false;

        //重置回合
        IsPlayerTurn = true;

        //清空日志、刷新UI
        logText.text = "";
        RefreshHpUI();
        AddText("战斗开始");
    }

    //根据怪物ID开启战斗方法
    public void StartBattle(int monsterId)
    {
        ResetBattle();

        //根据配置表读取怪物属性
        MonsterData mconfig = MonsterConfig.Get(monsterId);
        monsterUnit.UnitName = mconfig.name;
        monsterUnit.Health = mconfig.health;
        monsterUnit.GetComponent<MonsterIdentifier>().monsterId = monsterId;
    }

    void PlayerDied(string name)
    {
        AddText($"{name} 被击败");
        DeadPanel.SetActive(true);
        SetAllSkillBtnsInteractable(false);
    }
    void MonsterDied(string name)
    {
        AddText($"{name} 被击败");
        OnBattleWin?.Invoke();
        SetAllSkillBtnsInteractable(false);
    }

    void Update()
    {
        if (IsPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseSkill(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UseSkill(1);
            }
        }
    }
    void MonsterTurn()
    {
        int rand = _random.Next(0, monsterSkills.Count);
        monsterSkills[rand].Execute(monsterUnit, playerUnit, AddText);

        RefreshHpUI() ;

        IsPlayerTurn = true;
        SetAllSkillBtnsInteractable(true);
    }
}
