using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public Button btn;
    public Image icon;
    public Color normalColor;
    public Color eliteColor;
    public Color restColor;
    public Color treasureColor;
    public Color bossColor;

    private int _nodeId;
    private System.Action<int> _onClick;

    public void Init(int nodeId, MapNodeData.NodeType type, System.Action<int> onClick)
    {
        _nodeId = nodeId;
        _onClick = onClick;

        //根据节点类型更改颜色
        switch (type)
        {
            case MapNodeData.NodeType.NormalBattle:
                icon.color = normalColor;
                break;
            case MapNodeData.NodeType.EliteBattle:
                icon.color = eliteColor;
                break;
            case MapNodeData.NodeType.Boss:
                icon.color = bossColor;
                break;
            case MapNodeData.NodeType.Treasure:
                icon.color = treasureColor;
                break;
            case MapNodeData.NodeType.Rest:
                icon.color = restColor;
                break;
        }

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => _onClick?.Invoke(_nodeId));
    }
    public void SetInteractable(bool enable)
    {
        btn.interactable = enable;
    }
}
