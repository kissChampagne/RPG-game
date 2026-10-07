using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //引用面板
    public GameObject BattlePanel;
    public GameObject DeadPanel;

    //引用按钮
    public Button startBtn;
    public Button exitBtn;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("=== MainMenu  start 执行了 ===");

        //绑定按钮事件
        startBtn.onClick.AddListener(onStartGame);
        exitBtn.onClick.AddListener(onExitGame);

        //初始化主菜单，隐藏其他菜单
        if (BattlePanel != null && DeadPanel != null)
        {
            BattlePanel.SetActive(false);
            DeadPanel.SetActive(false);
        }
    }

    void onStartGame()
    {
        Debug.Log("startBtn 被点击");
        //隐藏主菜单
        gameObject.SetActive(false);
        DeadPanel.SetActive(false);

        if (BattlePanel != null)
        {
            gameObject.SetActive(false);
            BattlePanel.SetActive(true);
            FightManager fm = FindObjectOfType<FightManager>();
            if (fm != null) fm.ResetBattle();
        }
    }

    void onExitGame()
    {
        Application.Quit();
    }
}
