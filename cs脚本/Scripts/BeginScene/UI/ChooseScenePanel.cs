using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Button btnLeft;//����ť
    public Button btnRight;//���Ұ�ť
    public Button btnStart;//��ʼ��ť
    public Button btnBack;//���ذ�ť
     
    public Image imgScene;//��峡��ͼ
    public Text txtInfo;//����ı�

    private int nowIndex;//������������(0��ʼ)
    private SceneInfo nowSceneInfo;//������������
    protected override void Init()
    {
        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if( nowIndex < 0 )
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            ChangeScene();
        });
        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if ( nowIndex >= GameDataMgr.Instance.sceneInfoList.Count )
                nowIndex = 0;
            ChangeScene();
        });
        btnStart.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();//���س������
            //ͬ������bug�����ܱ�֤��һ����������Դ������Ͼ�ֱ���л�����,����HeroBornPosδ���־��л�������������ű�������
            //SceneManager.LoadScene(nowSceneInfo.sceneName);//�л�����
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);//�첽���ط���һ���¼�
            ao.completed += (obj) =>//�ؿ���ʼ��
            {
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);//��ʼ����Ϸ�����Ĺؿ�������
            };
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();//���س������
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();//��ʾѡ�����
        });
        ChangeScene();//�����ʱҲ����
    }
    /// <summary>
    /// ��������ֵ���³���
    /// </summary>
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];//��ȡ����
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);//����ͼƬ
        txtInfo.text = "���ƣ�\n" + nowSceneInfo.name + "\n" + "������\n" + nowSceneInfo.tips;//��������
    }
}
