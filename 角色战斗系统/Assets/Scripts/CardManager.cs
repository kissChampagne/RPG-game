using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public List<BufferCard> commonPool;
    public List<BufferCard> rarePool;
    public List<BufferCard> epicPool;

    public GameObject cardPanel;
    public Transform cardParent;
    public GameObject cardPrefab;

    private List<BufferCard> _currentChoices = new List<BufferCard>();

    private void Awake()
    {
        Instance = this;
    }

    //在战斗胜利时调用，产生三选一
    public void ShowCardChoice(CardRarity minRarity)
    {
        cardPanel.SetActive(true);
        _currentChoices.Clear();

        //随机产生三张卡
        for (int i = 0; i < 3; i++)
        {
            BufferCard card = GetRandomCard(minRarity);
            _currentChoices.Add(card);
            GameObject obj = Instantiate(cardPrefab, cardParent);
            obj.GetComponent<CardItem>().Init(card, OnSelectCard);
        }
    }

    BufferCard GetRandomCard(CardRarity minRarity)
    {
        //普通70%， 稀有25%， 史诗5%
        int rand = Random.Range(0, 100);
        List<BufferCard> pool;
        if (rand < 70 || minRarity == CardRarity.Common)
            pool = commonPool;
        else if (rand < 95 || minRarity == CardRarity.Rare)
            pool = rarePool;
        else 
            pool = epicPool;

        return pool[Random.Range(0, pool.Count)];
    }

    void OnSelectCard(BufferCard card)
    {
        FightUnit player = FindObjectOfType<FightManager>().playerUnit;
        ApplyBuff(player, card);

        //关闭面板
        cardPanel.SetActive(false);
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }
        FindObjectOfType<MapManager>().ContinuAfterReward();
    }

    void ApplyBuff(FightUnit unit, BufferCard card)
    {
        switch (card.type)
        {
            case BufferType.AddAttack:
                unit.attackBonus += card.value;
                break;
            case BufferType.AddMaxHp:
                unit.maxHpBonus += card.value;
                break;
            case BufferType.AddDefense:
                unit.defendBonus += card.value;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
