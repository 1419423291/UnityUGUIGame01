using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// 专门管理数据结构体的游戏数据管理器（单例模式）
/// </summary>
public class GameDataMgr 
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    
    public RoleInfo nowSelRole;//记录选择的玩家数据 在游戏场景中创建
    public MusicData musicData;//音效相关数据
    public PlayerData playerData;//玩家本地数据
    public List<RoleInfo> roleInfoList;//所有的角色数据（只读不存）
    public List<SceneInfo> sceneInfoList;//所有的场景数据（只读不存）
    public List<MonsterInfo> monsterInfoList;//所有的怪物数据（只读不存）
    public List<TowerInfo> towerInfoList;//所有塔数据（只读不存）
    private GameDataMgr() 
    {
        //初始化默认数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");//读取本地音乐文本
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");//读取本地选角面板文本
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");//初始化玩家数据
        //防止删除playerData导致报错
        if (playerData.haveMoney == 0)
        {
            playerData.haveMoney = 400;
            SavePlayerData();//保存玩家数据 
        }
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");//初始化场景数据
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");//初始化怪物数据
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");//初始化怪物数据
    }

    /// <summary>
    /// 存储音效数据  
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData,"MusicData");//存储类名
    }

    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    /// <summary>
    /// 播放音效（GameDataMgr提供）
    /// </summary>
    /// <param name="resName">音效</param>
    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);//配置音效
        a.volume = musicData.soundValue;
        a.mute = !musicData.soundOpen;
        a.Play();//播放音效组件
        GameObject.Destroy(musicObj, 1);//删除音效组件
    }
}
