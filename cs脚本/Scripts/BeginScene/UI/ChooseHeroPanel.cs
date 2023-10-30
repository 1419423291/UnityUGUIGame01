using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //����ѡ��
    public Button btnLeft;
    public Button btnRight;

    public Button btnUnLock;//������ť
    public Text txtUnLock;//Ӣ�۽����۸�

    public Button btnStart;//��ʼ��ť
    public Button btnBack;//���ذ�ť
    public Text txtMoney;//���Ͻǽ�Ǯ��
    public Text txtName;//��ɫ���֣���˧�������� 
    private Transform heroPos;//ChooseHeroPanelӢ��Ԥ��λ��

    //��ʱ���������ڶ�̬����Ԥ����ɫ
    private GameObject heroObj;//��ǰѡ�н�ɫ
    private RoleInfo nowRoleData;//��ǰ����������ݣ���GameDataMgr.Instance.roleInfoList�ж�ȡ��
    private int nowIndex;//��ǰѡ�н�ɫ��id-1(������0��ʼ)
    protected override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;//�ҵ������з���Ԥ��������λ��
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();//�������Ͻǽ�Ǯ 
        btnLeft.onClick.AddListener(() =>//����ѡ��ť�߼�
        {
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            ChangeHero();//ģ�͸���
        });
        btnRight.onClick.AddListener(() =>//����ѡ��ť�߼�
        {
            ++nowIndex;
            if (nowIndex > GameDataMgr.Instance.roleInfoList.Count - 1)
                nowIndex = 0;
            ChangeHero();//ģ�͸���
        });
        btnUnLock.onClick.AddListener(() =>//������ť�߼�
        {
            //������ť�߼�
            PlayerData data = GameDataMgr.Instance.playerData;
            if (data.haveMoney >= nowRoleData.lockMoney)
            {
                data.haveMoney -= nowRoleData.lockMoney;//�������ݵ�Ǯ
                txtMoney.text = data.haveMoney.ToString();//���½����Ǯ
                data.buyHero.Add(nowRoleData.id);//��¼�����id
                GameDataMgr.Instance.SavePlayerData();//����������� 
                UpdateLockBtn();//���½�����ť
                //��ʾ��� ����ɹ�
                //print("����ɹ�");
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("����ɹ�");
            }
            else
            {
                //��ʾ��� ��Ǯ����
                //print("��Ǯ����");
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("��Ǯ����");
            }
        });
        btnStart.onClick.AddListener(() =>//��ʼ��ť�߼�
        {
            GameDataMgr.Instance.nowSelRole = nowRoleData;//��¼��ǰѡ�еĽ�ɫ 
            UIManager.Instance.HidePanel<ChooseHeroPanel>();//����ѡ�����
            UIManager.Instance.ShowPanel<ChooseScenePanel>();//��ʾ�������
        });
        btnBack.onClick.AddListener(() =>//���ذ�ť�߼�
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();//����ѡ�����
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>//�������ת�غ���ʾ��ʼ������
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        ChangeHero();//��ѡ�����ҲҪ����ģ��
    }

    /// <summary>
    /// ��̬����Ӣ��ģ��
    /// </summary>
    private void ChangeHero()
    {
        if(heroObj != null)//�л���ɫʱɾ��
        {
            Destroy(heroObj);
            heroObj = null;
        }
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];// ��ȡ������ɫ����
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos);//ʵ������ɫ
        txtName.text = nowRoleData.tips;//����Ӣ������
        //��ֹѡ�˽��津����Ӧ����GameScene1�У�PlayerObject�ϵİ����߼�
        Destroy(heroObj.GetComponent<PlayerObject>());
        UpdateLockBtn();//���ݽ������������ʾ�Ƿ���Ҫ����
    }
    /// <summary>
    /// ���½�����ť��ʾ���
    /// </summary>
    private void UpdateLockBtn()
    {
        //�����ɫδ�������ÿ�ʼ��ť(��ʾ������ť)
        if ( nowRoleData.lockMoney > 0  && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id ))
        {   //��Ҫ���� ��  �ѹ���Ӣ���б���û�ж�Ӧid
            btnUnLock.gameObject.SetActive(true);//��ʾ������ť
            btnStart.gameObject.SetActive(false);//���ؿ�ʼ��ť
            txtUnLock.text = "$" + nowRoleData.lockMoney;//���½����۸��ı�
        }
        else
        {
            btnStart.gameObject.SetActive(true);//��ʾ��ʼ��ť
            btnUnLock.gameObject.SetActive(false);//���ؽ�����ť    
        }
    }
    /// <summary>
    /// ����ChoosePanel��塣��ѡ�н�ɫģ��ɾ�� 
    /// </summary>
    /// <param name="callBack"></param>
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
