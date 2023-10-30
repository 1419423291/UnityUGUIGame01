using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �����ýű���������Ϸʤ���߼�
/// </summary>
public class MonsterObject : MonoBehaviour
{
    
    private Animator animator;//�������
    private NavMeshAgent agent;//λ����� Ѱ·���

    private MonsterInfo monsterInfo;//һЩ���������
    private int hp;//��ǰѪ��
    public bool isDead = false;//�Ƿ��������ⲿ�ɵ���
    private float frontTime = 0;//�ϴι�����ʱ�䣨��ʱ���������ڹ����߼���
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();//Ѱ·���   
        animator = this.GetComponent<Animator>();//�������
    }

    //��ʼ������
    // ��ʼ������
    public void InitInfo(MonsterInfo info)//���˳��ֵ�����
    {
        monsterInfo = info;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);//״̬������
        hp = info.hp;//��ʼ��Ѫ��
        agent.speed = info.moveSpeed;//�ƶ��ٶȡ����ٶȣ���֤û�����Եļ����˶��������������٣�
        agent.angularSpeed = info.roundSpeed;//��ת�ٶ�
    }

    //����
    // ���˺�����MonsterObject�ṩ���ⲿ�ɵ��ã�
    public void Wound(int dmg)
    {
        if (isDead)
            return;//�����Ͳ���������
        hp -= dmg;//����Ѫ��
        animator.SetTrigger("Wound");//�������˶���

        if (hp <= 0)
        {
            //Ѫ��Ϊ0����
            Dead();
        }
        else
        {
            GameDataMgr.Instance.PlaySound("Music/Wound");//������Ч����
        }
    }

    
    // ��������
    public void Dead()
    {
        isDead = true;
        //agent.isStopped = true;//ֹͣ�Զ�Ѱ·���ƶ�
        agent.enabled = false;//ֹͣѰ·
        animator.SetBool("Dead", true);//������������
        GameDataMgr.Instance.PlaySound("Music/dead");//����������Ч
        //��Ǯ��������
        GameLevelMgr.Instance.player.AddMoney(15);
    }


    // ���������������¼���ʤ���߼���
    public void DeadEvent()//�Ƴ�ʬ�壨�����ڴ棩
    {
        //��������������Ϻ��Ƴ�ʬ��
        //���˹ؿ���������������
        //GameLevelMgr.Instance.ChangeMonsterNum(-1);//����һֻ����ͼ�1
        GameLevelMgr.Instance.RemoveMonster(this);//�ӳ����������Ĺ����б��Ƴ�
        Destroy(this.gameObject);//��������������Ƴ��Ѿ������Ķ���ʬ��
        if (GameLevelMgr.Instance.CheckOver())
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//��ʾʤ�����
            panel.InitInfo(GameLevelMgr.Instance.player.money, true);//��ʼ����������
        }
    }


    // ���������������¼�
    public void BornOver()//�������ٽ����ƶ����ƶ�--Ѱ·�����
    {
        agent.SetDestination(MainTowerObject.Instance.transform.position);//MainTowerObject�ǵ���ģʽ�ű������Դӽű���ȡ����
        animator.SetBool("Run", true);//�����ƶ�����
    }

    //����
    void Update()//ʲôʱ����й�����⣨ֹͣ�ܲ���
    {
        if (isDead)
            return;
        animator.SetBool("Run", agent.velocity != Vector3.zero);//��������Ѱ·ϵͳ�е��ٶȾ����Ƿ񲥷�
        if (Vector3.Distance(this.transform.position, MainTowerObject.Instance.transform.position) < 5
            && Time.time - frontTime >= monsterInfo.atkOffset)//�����߼�����Ŀ��������ɾ������ڣ�
        {
            frontTime = Time.time;//��¼��ι�����ʱ��
            animator.SetTrigger("Atk");
        }
    }

    //�˺����
    public void AtkEvent()
    {
        //��Χ�������˺��ж�
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,1, 1 << LayerMask.NameToLayer("MainTower"));//ע��this.transform.position�ǽŵ׵ĵ�
        GameDataMgr.Instance.PlaySound("Music/Eat");//���Ź�����Ч

        for (int i = 0; i < colliders.Length; i++)
        {
            if (MainTowerObject.Instance.gameObject == colliders[i].gameObject)
            //��if (MainTowerObject.Instance.transform == colliders[i].transform)
            {
                MainTowerObject.Instance.Wound(monsterInfo.atk);//�õ�ǰ����������
            }
        }
    }
}
