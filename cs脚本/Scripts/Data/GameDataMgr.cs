using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// ר�Ź������ݽṹ�����Ϸ���ݹ�����������ģʽ��
/// </summary>
public class GameDataMgr 
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    
    public RoleInfo nowSelRole;//��¼ѡ���������� ����Ϸ�����д���
    public MusicData musicData;//��Ч�������
    public PlayerData playerData;//��ұ�������
    public List<RoleInfo> roleInfoList;//���еĽ�ɫ���ݣ�ֻ�����棩
    public List<SceneInfo> sceneInfoList;//���еĳ������ݣ�ֻ�����棩
    public List<MonsterInfo> monsterInfoList;//���еĹ������ݣ�ֻ�����棩
    public List<TowerInfo> towerInfoList;//���������ݣ�ֻ�����棩
    private GameDataMgr() 
    {
        //��ʼ��Ĭ������
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");//��ȡ���������ı�
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");//��ȡ����ѡ������ı�
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");//��ʼ���������
        //��ֹɾ��playerData���±���
        if (playerData.haveMoney == 0)
        {
            playerData.haveMoney = 400;
            SavePlayerData();//����������� 
        }
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");//��ʼ����������
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");//��ʼ����������
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");//��ʼ����������
    }

    /// <summary>
    /// �洢��Ч����  
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData,"MusicData");//�洢����
    }

    /// <summary>
    /// �洢�������
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    /// <summary>
    /// ������Ч��GameDataMgr�ṩ��
    /// </summary>
    /// <param name="resName">��Ч</param>
    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);//������Ч
        a.volume = musicData.soundValue;
        a.mute = !musicData.soundOpen;
        a.Play();//������Ч���
        GameObject.Destroy(musicObj, 1);//ɾ����Ч���
    }
}
