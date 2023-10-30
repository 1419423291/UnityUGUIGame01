using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Image imgHP;//hp数字
    public Text txtHP;//波数

    public Text txtWave;
    public Text txtMoney;

    public float hpW = 500;//hp的初始宽

    public Button btnQuit;//退出按钮
    public Transform botTrans;//造塔组合控件父对象(显隐造塔面板)
    public List<TowerBtn> towerBtns = new List<TowerBtn>();//管理三个造塔复合控件
    private TowerPoint nowSelTowerPoint;//当前选中造塔点
    private bool checkInput = false;//标识是否检测造塔输入
    protected override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//显示结束面板
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);//失败得到奖励的一半
            //UIManager.Instance.HidePanel<GamePanel>();//隐藏游戏界面
            //SceneManager.LoadScene("BeginScene");//返回开始界面
        });
        botTrans.gameObject.SetActive(false);//默认隐藏造塔界面
        Cursor.lockState = CursorLockMode.Confined;//锁定鼠标
    }

    /// <summary>
    /// 更新安全区血量函数（GamePanel提供给外部）
    /// </summary>
    /// <param name="hp">当前血量</param>
    /// <param name="maxHP">最大血量</param>
    public void UpdateTowerHp(int hp, int maxHP)
    {
        txtHP.text = hp + "/" + maxHP;
        //更新血条长度
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpW, 38);
    }

    /// <summary>
    /// 更新剩余波数（GamePanel提供给外部）
    /// </summary>
    /// <param name="hp">当前波数</param>
    /// <param name="maxHP">最大波数</param>
    public void UpdateWaveNum(int nowNum, int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }

    /// <summary>
    /// 更新金币数量
    /// </summary>
    /// <param name="money">当前获得金币</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }
    /// <summary>
    /// 造塔逻辑（参数为造塔点）
    /// </summary>
    public void UpdateSelTower(TowerPoint point)
    {
        //根据造塔点信息决定界面显示内容
        nowSelTowerPoint = point;
        if (nowSelTowerPoint == null)//不在造塔点附近
        {
            checkInput = false;//隐藏造塔界面时不可输入键位造塔
            botTrans.gameObject.SetActive(false);//直接隐藏造塔界面即可
        }
        else//可以造塔
        {
            checkInput = true;//显示造塔界面时可以输入键位造塔
            botTrans.gameObject.SetActive(true);//显示造塔界面并处理对应逻辑
            if (nowSelTowerPoint.nowTowerInfo == null)//在造塔点附件且没有塔
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);//显示造塔点按钮
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIDs[i], "数字键" + (i + 1));//更新造塔点键位提示
                }
            }
            else//在造塔点附件且有塔
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);//隐藏所有造塔点按钮
                }
                towerBtns[1].gameObject.SetActive(true);//仅显示中间的造塔点
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLev, "空格键");
            }
        }
    }
    protected override void Update()//防止basePanel的update被覆盖不显示造塔点面板
    {
        base.Update();//用于检测造塔点键位输入
        if (!checkInput)
            return;
        if (nowSelTowerPoint.nowTowerInfo == null)//如果造塔点没塔（检测数字123键建造）
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[2]);
            }
        }
        else//检测空格键建造
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.nextLev);//建造下一等级的塔
            }
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//显示结束面板
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);//失败得到奖励的一半
        }
    }
}
