using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MapNodeData
{
    public int nodeId;
    public NodeType type;
    public int monsterId;
    public int floor;
    public List<int> nextNodeIds = new List<int>();

    public enum NodeType
    {
        NormalBattle,
        EliteBattle,
        Rest,
        Treasure,
        Boss
    }
}
