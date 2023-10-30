using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //左右选择
    public Button btnLeft;
    public Button btnRight;

    public Button btnUnLock;//解锁按钮
    public Text txtUnLock;//英雄解锁价格

    public Button btnStart;//开始按钮
    public Button btnBack;//返回按钮
    public Text txtMoney;//左上角金钱数
    public Text txtName;//角色名字：最帅气的男人 
    private Transform heroPos;//ChooseHeroPanel英雄预览位置

    //临时变量，用于动态更新预览角色
    private GameObject heroObj;//当前选中角色
    private RoleInfo nowRoleData;//当前单条玩家数据（从GameDataMgr.Instance.roleInfoList中读取）
    private int nowIndex;//当前选中角色的id-1(索引从0开始)
    protected override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;//找到场景中放置预设体对象的位置
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();//更新左上角金钱 
        btnLeft.onClick.AddListener(() =>//向左选择按钮逻辑
        {
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            ChangeHero();//模型更新
        });
        btnRight.onClick.AddListener(() =>//向右选择按钮逻辑
        {
            ++nowIndex;
            if (nowIndex > GameDataMgr.Instance.roleInfoList.Count - 1)
                nowIndex = 0;
            ChangeHero();//模型更新
        });
        btnUnLock.onClick.AddListener(() =>//解锁按钮逻辑
        {
            //解锁按钮逻辑
            PlayerData data = GameDataMgr.Instance.playerData;
            if (data.haveMoney >= nowRoleData.lockMoney)
            {
                data.haveMoney -= nowRoleData.lockMoney;//更新数据的钱
                txtMoney.text = data.haveMoney.ToString();//更新界面的钱
                data.buyHero.Add(nowRoleData.id);//记录购买的id
                GameDataMgr.Instance.SavePlayerData();//保存玩家数据 
                UpdateLockBtn();//更新解锁按钮
                //提示面板 购买成功
                //print("购买成功");
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
            }
            else
            {
                //提示面板 金钱不足
                //print("金钱不足");
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足");
            }
        });
        btnStart.onClick.AddListener(() =>//开始按钮逻辑
        {
            GameDataMgr.Instance.nowSelRole = nowRoleData;//记录当前选中的角色 
            UIManager.Instance.HidePanel<ChooseHeroPanel>();//隐藏选角面板
            UIManager.Instance.ShowPanel<ChooseScenePanel>();//显示场景面板
        });
        btnBack.onClick.AddListener(() =>//返回按钮逻辑
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();//隐藏选角面板
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>//让摄像机转回后显示开始面板界面
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        ChangeHero();//打开选角面板也要更新模型
    }

    /// <summary>
    /// 动态更新英雄模型
    /// </summary>
    private void ChangeHero()
    {
        if(heroObj != null)//切换角色时删除
        {
            Destroy(heroObj);
            heroObj = null;
        }
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];// 读取单条角色数据
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos);//实例化角色
        txtName.text = nowRoleData.tips;//更新英雄名字
        //防止选人界面触发（应用于GameScene1中）PlayerObject上的按键逻辑
        Destroy(heroObj.GetComponent<PlayerObject>());
        UpdateLockBtn();//根据解锁相关数据显示是否需要解锁
    }
    /// <summary>
    /// 更新解锁按钮显示情况
    /// </summary>
    private void UpdateLockBtn()
    {
        //如果角色未解锁禁用开始按钮(显示解锁按钮)
        if ( nowRoleData.lockMoney > 0  && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id ))
        {   //需要购买 且  已购买英雄列表中没有对应id
            btnUnLock.gameObject.SetActive(true);//显示解锁按钮
            btnStart.gameObject.SetActive(false);//隐藏开始按钮
            txtUnLock.text = "$" + nowRoleData.lockMoney;//更新解锁价格文本
        }
        else
        {
            btnStart.gameObject.SetActive(true);//显示开始按钮
            btnUnLock.gameObject.SetActive(false);//隐藏解锁按钮    
        }
    }
    /// <summary>
    /// 隐藏ChoosePanel面板。将选中角色模型删除 
    /// </summary>
    /// <param name="callBack"></param>
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
