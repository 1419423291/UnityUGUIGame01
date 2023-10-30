using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Image imgHP;//hp����
    public Text txtHP;//����

    public Text txtWave;
    public Text txtMoney;

    public float hpW = 500;//hp�ĳ�ʼ��

    public Button btnQuit;//�˳���ť
    public Transform botTrans;//������Ͽؼ�������(�����������)
    public List<TowerBtn> towerBtns = new List<TowerBtn>();//���������������Ͽؼ�
    private TowerPoint nowSelTowerPoint;//��ǰѡ��������
    private bool checkInput = false;//��ʶ�Ƿ�����������
    protected override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//��ʾ�������
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);//ʧ�ܵõ�������һ��
            //UIManager.Instance.HidePanel<GamePanel>();//������Ϸ����
            //SceneManager.LoadScene("BeginScene");//���ؿ�ʼ����
        });
        botTrans.gameObject.SetActive(false);//Ĭ��������������
        Cursor.lockState = CursorLockMode.Confined;//�������
    }

    /// <summary>
    /// ���°�ȫ��Ѫ��������GamePanel�ṩ���ⲿ��
    /// </summary>
    /// <param name="hp">��ǰѪ��</param>
    /// <param name="maxHP">���Ѫ��</param>
    public void UpdateTowerHp(int hp, int maxHP)
    {
        txtHP.text = hp + "/" + maxHP;
        //����Ѫ������
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpW, 38);
    }

    /// <summary>
    /// ����ʣ�ನ����GamePanel�ṩ���ⲿ��
    /// </summary>
    /// <param name="hp">��ǰ����</param>
    /// <param name="maxHP">�����</param>
    public void UpdateWaveNum(int nowNum, int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }

    /// <summary>
    /// ���½������
    /// </summary>
    /// <param name="money">��ǰ��ý��</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }
    /// <summary>
    /// �����߼�������Ϊ�����㣩
    /// </summary>
    public void UpdateSelTower(TowerPoint point)
    {
        //������������Ϣ����������ʾ����
        nowSelTowerPoint = point;
        if (nowSelTowerPoint == null)//���������㸽��
        {
            checkInput = false;//������������ʱ���������λ����
            botTrans.gameObject.SetActive(false);//ֱ�������������漴��
        }
        else//��������
        {
            checkInput = true;//��ʾ��������ʱ���������λ����
            botTrans.gameObject.SetActive(true);//��ʾ�������沢�����Ӧ�߼�
            if (nowSelTowerPoint.nowTowerInfo == null)//�������㸽����û����
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);//��ʾ�����㰴ť
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIDs[i], "���ּ�" + (i + 1));//�����������λ��ʾ
                }
            }
            else//�������㸽��������
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);//�������������㰴ť
                }
                towerBtns[1].gameObject.SetActive(true);//����ʾ�м��������
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLev, "�ո��");
            }
        }
    }
    protected override void Update()//��ֹbasePanel��update�����ǲ���ʾ���������
    {
        base.Update();//���ڼ���������λ����
        if (!checkInput)
            return;
        if (nowSelTowerPoint.nowTowerInfo == null)//���������û�����������123�����죩
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[2]);
            }
        }
        else//���ո������
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.nextLev);//������һ�ȼ�����
            }
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//��ʾ�������
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);//ʧ�ܵõ�������һ��
        }
    }
}
