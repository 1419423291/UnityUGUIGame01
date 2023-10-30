using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������Ϸʧ���߼�
/// </summary>
public class MainTowerObject : MonoBehaviour
{
    public int hp;//Ѫ�����
    public int maxHp;

    private bool isDead;//�Ƿ�����

    private static MainTowerObject instance;//�̳���MonoBehaveior����new����Awake�г�ʼ�����ɣ�
    public static MainTowerObject Instance => instance;
    void Awake()
    {
        instance = this;
    }

    //����Ѫ��
    public void UpdateHp(int Hp, int maxHP)
    {
        this.hp = Hp;
        this.maxHp = maxHP;
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(Hp, maxHP);//���½�����Ѫ��ͼ��
    }

    //�����߼�
    public void Wound(int dam)
    {
        if (isDead)//�������ٿ�Ѫ
            return;
        hp -= dam;//��Ѫ 
        if(hp < 0)//�����߼�
        {
            hp = 0;
            isDead = true;
            //��Ϸ�����߼���ʧ���߼���
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//��ʾ�������
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);//ʧ�ܵõ�������һ��
        }
        UpdateHp(hp, maxHp);//����Ѫ��
    }
  
    private void OnDestroy()
    {
        instance = null;//������ʱ�����Զ�ɾ���������ÿգ��ͷ��ڴ棩
    }
}
