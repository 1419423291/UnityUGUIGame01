using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;
    public AudioSource bkSource;
    void Awake()
    {
        instance = this;
        bkSource = this.GetComponent<AudioSource>();
        //通过数据设置音乐大小和开关
        MusicData data = GameDataMgr.Instance.musicData;//会进入GameDataMgr中执行其构造函数（并读取本地数据）
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
        //SetIsOpen(data.soundOpen);
        //ChangeValue(data.soundValue);
    }

    //开关背景音乐方法
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }
    //调整背景音乐大小方法
    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }

}
