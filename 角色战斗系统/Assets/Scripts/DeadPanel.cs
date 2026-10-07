using UnityEngine;
using UnityEngine.UI;

public class DeadPanel : MonoBehaviour
{
    public GameObject BattlePanel;
    public GameObject MainMenuPanel;

    public Button RestartBtn;
    public Button ExitBtn;

    // Start is called before the first frame update
    void Start()
    {
        //绑定按钮事件
        RestartBtn.onClick.AddListener(onRestartGame);
        ExitBtn.onClick.AddListener(onExitGame);
        
        //隐藏其他菜单
        if (BattlePanel != null && MainMenuPanel != null)
        {
            MainMenuPanel.SetActive(false);
            BattlePanel.SetActive(false);
        }        
    }

    void onRestartGame()
    {
        if (MainMenuPanel != null && BattlePanel != null)
        {
            BattlePanel.SetActive(true);
            FightManager fm = FindObjectOfType<FightManager>();
            if (fm != null) fm.ResetBattle();
        }
    }
    void onExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
