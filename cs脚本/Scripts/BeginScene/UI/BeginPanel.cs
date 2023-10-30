using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    protected override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //开始游戏按钮逻辑：隐藏自己显示选角面板
            //播放摄像机的左转动画(获取主摄像机的CameraAnimator脚本并调用对应函数，参数为委托函数“表示执行完函数的动作:显示选角面板”)
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            { 
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();//显示选角面板
            });
            //隐藏BeginPanel面板(不写入委托函数是因为不需要等待对应动画结束以后才隐藏)
            UIManager.Instance.HidePanel<BeginPanel>();
        });
        btnSetting.onClick.AddListener(() =>
        {
            //设置按钮逻辑：显示设置界面
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnAbout.onClick.AddListener(() =>
        {
            //关于按钮逻辑：提示面板(提示字)
        });
        btnQuit.onClick.AddListener(() =>
        {
            //退出按钮逻辑
            Application.Quit();
        });
    }

}
