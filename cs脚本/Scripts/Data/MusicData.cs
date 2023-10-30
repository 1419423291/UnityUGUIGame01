using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储和读取音效相关数据(SettingPanel)。
/// 背景音乐数据结构类
/// </summary>
public class MusicData 
{
    //背景音乐和音效开关
    public bool musicOpen = true;
    public bool soundOpen = true;
    //背景音乐和音效大小
    public float musicValue = 0.2f;
    public float soundValue = 0.2f;
}
