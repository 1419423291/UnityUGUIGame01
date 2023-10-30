using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text txtWin;//游戏胜利
    public Text txtInfo;//恭喜获得胜利奖励
    public Text txtMoney;//$50
    public Button btnSure;//OK按钮

    protected override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GameOverPanel>();//隐藏GameOverPanel
            UIManager.Instance.HidePanel<GamePanel>();//隐藏GamePanel
            GameLevelMgr.Instance.ClearInfo();//清空GameLevelMgr数据方便下次使用
            SceneManager.LoadScene("BeginScene");//切换场景
        });
    }
    /// <summary>
    /// 初始化GameOverPanel面板，内置加钱逻辑
    /// </summary>
    public void InitInfo(int money,bool isWin)
    {
        txtWin.text = isWin ? "胜利" : "失败";
        txtInfo.text = isWin ? "获得胜利奖励" : "获得失败奖励";
        txtMoney.text = "$" + money;
        GameDataMgr.Instance.playerData.haveMoney += money; //增加金钱
        GameDataMgr.Instance.SavePlayerData();//保存增加金钱
    }
    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;//解锁鼠标
    }
}
