using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text txtWin;//��Ϸʤ��
    public Text txtInfo;//��ϲ���ʤ������
    public Text txtMoney;//$50
    public Button btnSure;//OK��ť

    protected override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GameOverPanel>();//����GameOverPanel
            UIManager.Instance.HidePanel<GamePanel>();//����GamePanel
            GameLevelMgr.Instance.ClearInfo();//���GameLevelMgr���ݷ����´�ʹ��
            SceneManager.LoadScene("BeginScene");//�л�����
        });
    }
    /// <summary>
    /// ��ʼ��GameOverPanel��壬���ü�Ǯ�߼�
    /// </summary>
    public void InitInfo(int money,bool isWin)
    {
        txtWin.text = isWin ? "ʤ��" : "ʧ��";
        txtInfo.text = isWin ? "���ʤ������" : "���ʧ�ܽ���";
        txtMoney.text = "$" + money;
        GameDataMgr.Instance.playerData.haveMoney += money; //���ӽ�Ǯ
        GameDataMgr.Instance.SavePlayerData();//�������ӽ�Ǯ
    }
    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;//�������
    }
}
