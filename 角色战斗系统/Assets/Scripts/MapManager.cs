using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("引用")]
    public GameObject nodePrefab;
    public Transform nodeParent;
    public GameObject battlePanel;

    [Header("地图配置")]
    public List<MapNodeData> nodes;
    
    private Dictionary<int, MapNode> _nodeDict = new Dictionary<int, MapNode>();
    private int _currentfloor;
    private MapNodeData _currentNode;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        UnlockFloor(0);
    }
    //生成所以节点
    void GenerateMap()
    {
        foreach (var node in nodes)
        {
            GameObject obj = Instantiate(nodePrefab, nodeParent);
            MapNode nodeData = obj.GetComponent<MapNode>();
            nodeData.Init(node.nodeId, node.type, OnNodeClick);
            nodeData.SetInteractable(false);
            _nodeDict.Add(node.nodeId, nodeData);
        }
    }
    void OnNodeClick(int nodeId)
    {
        MapNodeData data = nodes.Find( n  => n.nodeId == nodeId );
        if (data == null)  return;

        switch (data.type)
        {
            case MapNodeData.NodeType.NormalBattle:
            case MapNodeData.NodeType.EliteBattle:
            case MapNodeData.NodeType.Boss:
                _currentNode = data;
                EnterBattle(data.monsterId);
                break;
            case MapNodeData.NodeType.Rest:
                //休息回血
                FightManager fm = FindObjectOfType<FightManager>();
                fm.playerUnit.CoverHeal(30);
                UnlockNextFloor(data);
                break;
            case MapNodeData.NodeType.Treasure:
                //直接给奖励
                UnlockNextFloor(data);
                break;
        }
    }
    void EnterBattle(int monsterId)
    {
        gameObject.SetActive(false);
        battlePanel.SetActive(true);

        FightManager fm = FindObjectOfType<FightManager>();
        fm.StartBattle(monsterId);
        fm.OnBattleWin += OnBattleWin;
    }
    //战斗胜利
    void OnBattleWin()
    {
        FightManager fm = FindObjectOfType<FightManager>();
        //取消 OnBattleWin 事件监听
        fm.OnBattleWin -= OnBattleWin;

        battlePanel.SetActive(false);

        if (CardManager.Instance != null)
        {
            CardRarity rarity = _currentNode.type == MapNodeData.NodeType.EliteBattle ? CardRarity.Rare : CardRarity.Common;
            CardManager.Instance.ShowCardChoice(rarity);
        }
        else
        {
            ContinuAfterReward();
        }

    }
    //解锁当前层的所有节点
    void UnlockFloor(int floor)
    {
        foreach (var node in nodes)
        {
            if (node.floor == floor)
                _nodeDict[node.nodeId].SetInteractable(true);
        }
        _currentfloor = floor;
    }
    //解锁下一层节点
    void UnlockNextFloor(MapNodeData currentNode)
    {
        foreach (int nextId in currentNode.nextNodeIds)
        {
            _nodeDict[nextId].SetInteractable(true);
        }
    }
    public void ContinuAfterReward()
    {
        gameObject.SetActive(true);
        if (_currentNode != null)
        {
            UnlockNextFloor(_currentNode);
            _currentfloor = _currentNode.floor + 1;
            _currentNode = null;
        }
    }
}
