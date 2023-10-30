using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;//�رհ�ť
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    protected override void Init()
    {
        //���ݱ��ش洢������ ���г�ʼ��SettingPanel�������
        MusicData data = GameDataMgr.Instance.musicData;
        //��ʼ�����ؿؼ���״̬
        togMusic.isOn = data.musicOpen;
        togSound.isOn = data.soundOpen;
        //��ʼ���϶����ؼ���С
        sliderMusic.value = data.musicValue;
        sliderSound.value=data.soundValue;


        btnClose.onClick.AddListener(() =>
        {
            //�ر�SettingPanel���ٽ��д洢д��Ӳ��
            //���ɣ�����϶�ε��������д洢����̫��������
            JsonMgr.Instance.SaveData(GameDataMgr.Instance.musicData, "MusicData"); 
            UIManager.Instance.HidePanel<SettingPanel>();//�����Լ�
        });
        togMusic.onValueChanged.AddListener((v) =>//���ֿ����߼�
        {
            //���ر�������
            BKMusic.Instance.SetIsOpen(v);
            //��¼�洢���ص�����
            GameDataMgr.Instance.musicData.musicOpen = v;
        });
        togSound.onValueChanged.AddListener((v) =>//��Ч�����߼�
        {
            GameDataMgr.Instance.musicData.soundOpen = v;//��Ч���Ž���Ҫ��¼����
        });
        sliderMusic.onValueChanged.AddListener((v) =>//���ִ�С�߼�
        {
            //�ı䱳�����ִ�С
            BKMusic.Instance.ChangeValue(v);
            //��¼������������
            GameDataMgr.Instance.musicData.musicValue = v;
        });
        sliderSound.onValueChanged.AddListener((v) =>//��Ч��С�߼�
        {
            //��¼������Ч����
            GameDataMgr.Instance.musicData.soundValue = v;
        });

    }
}
