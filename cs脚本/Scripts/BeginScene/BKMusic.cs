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
        //ͨ�������������ִ�С�Ϳ���
        MusicData data = GameDataMgr.Instance.musicData;//�����GameDataMgr��ִ���乹�캯��������ȡ�������ݣ�
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
        //SetIsOpen(data.soundOpen);
        //ChangeValue(data.soundValue);
    }

    //���ر������ַ���
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }
    //�����������ִ�С����
    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }

}
