using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;//关闭按钮
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    protected override void Init()
    {
        //根据本地存储的数据 进行初始化SettingPanel面板数据
        MusicData data = GameDataMgr.Instance.musicData;
        //初始化开关控件的状态
        togMusic.isOn = data.musicOpen;
        togSound.isOn = data.soundOpen;
        //初始化拖动条控件大小
        sliderMusic.value = data.musicValue;
        sliderSound.value=data.soundValue;


        btnClose.onClick.AddListener(() =>
        {
            //关闭SettingPanel后再进行存储写入硬盘
            //理由：面板上多次调整都进行存储数据太消耗性能
            JsonMgr.Instance.SaveData(GameDataMgr.Instance.musicData, "MusicData"); 
            UIManager.Instance.HidePanel<SettingPanel>();//隐藏自己
        });
        togMusic.onValueChanged.AddListener((v) =>//音乐开关逻辑
        {
            //开关背景音乐
            BKMusic.Instance.SetIsOpen(v);
            //记录存储开关的数据
            GameDataMgr.Instance.musicData.musicOpen = v;
        });
        togSound.onValueChanged.AddListener((v) =>//音效开关逻辑
        {
            GameDataMgr.Instance.musicData.soundOpen = v;//音效播放仅需要记录即可
        });
        sliderMusic.onValueChanged.AddListener((v) =>//音乐大小逻辑
        {
            //改变背景音乐大小
            BKMusic.Instance.ChangeValue(v);
            //记录背景音乐数据
            GameDataMgr.Instance.musicData.musicValue = v;
        });
        sliderSound.onValueChanged.AddListener((v) =>//音效大小逻辑
        {
            //记录背景音效数据
            GameDataMgr.Instance.musicData.soundValue = v;
        });

    }
}
