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
            //��ʼ��Ϸ��ť�߼��������Լ���ʾѡ�����
            //�������������ת����(��ȡ���������CameraAnimator�ű������ö�Ӧ����������Ϊί�к�������ʾִ���꺯���Ķ���:��ʾѡ����塱)
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            { 
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();//��ʾѡ�����
            });
            //����BeginPanel���(��д��ί�к�������Ϊ����Ҫ�ȴ���Ӧ���������Ժ������)
            UIManager.Instance.HidePanel<BeginPanel>();
        });
        btnSetting.onClick.AddListener(() =>
        {
            //���ð�ť�߼�����ʾ���ý���
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnAbout.onClick.AddListener(() =>
        {
            //���ڰ�ť�߼�����ʾ���(��ʾ��)
        });
        btnQuit.onClick.AddListener(() =>
        {
            //�˳���ť�߼�
            Application.Quit();
        });
    }

}
