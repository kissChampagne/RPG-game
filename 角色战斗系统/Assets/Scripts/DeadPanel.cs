using UnityEngine;
using UnityEngine.UI;

public class DeadPanel : MonoBehaviour
{
    public GameObject BattlePanel;
    public GameObject MainMenuPanel;

    public Button RestartBtn;
    public Button ExitBtn;

    private void OnEnable()
    {
        //在每次面板弹出时，绑定按钮
        RestartBtn.onClick.RemoveAllListeners();
        RestartBtn.onClick.AddListener(onRestartGame);

        ExitBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.AddListener(onExitGame);
    }

    void onRestartGame()
    {
        Debug.Log("重新开始按钮已被点击");
        if (MainMenuPanel != null && BattlePanel != null)
        {
            BattlePanel.SetActive(true);
            FightManager fm = FindObjectOfType<FightManager>();
            if (fm != null) fm.ResetBattle();

            //重启战斗后，关闭死亡面板
            gameObject.SetActive(false);
        }
    }

    void onExitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
        
        //隐藏其他菜单
        MainMenuPanel.SetActive(false);
        BattlePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
