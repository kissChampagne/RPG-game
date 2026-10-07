using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public Button btn;
    public TMP_Text skillNameText;

    private int _skillIndex;
    private System.Action<int> _onClickCallback;

    //初始化按钮：技能索引、技能名、点击回调
    public void Init(int index, string skillName, System.Action<int> onClick)
    {
        _skillIndex = index;
        skillNameText.text = skillName;
        _onClickCallback = onClick;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        _onClickCallback?.Invoke(_skillIndex);
    }
    //设置按钮是否可交互
    public void SetInteractable(bool enable)
    {
        btn.interactable = enable;
    }
}
