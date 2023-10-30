using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    public Transform head;//����ͷ�� ��תָ��Ŀ��
    public Transform gunPoint;//����㣬����������Ч
    private float roundSpeed = 20;//��̨��ת�ٶ�
    private TowerInfo info;//����ǰ��ȡ�ģ�������̨����
    private MonsterObject targetObj;//��ǰҪ����Ŀ��
    private List<MonsterObject> targetObjs;
    private float nowTime;//��ʱ���� ��ʱ�������ʱ��
    private Vector3 monsterPos;//��¼�����λ��

    /// <summary>
    /// ��ʼ����̨������ݣ�TowerObject��
    /// </summary>
    /// <param name="info">�ⲿ����TowerInfo</param>
    public void InitInfo(TowerInfo info)
    {
        this.info = info;//������info����Ϊ������info
    }

    
    void Update()//Ѱ��Ŀ��
    {
        if (info.atkType == 1)//���幥���߼���ÿ�ν���һ������
        {   //Ѱ��Ŀ���߼�
            if(targetObj == null //δѡ��Ŀ��
                || targetObj.isDead //ѡ��Ŀ��������
                || Vector3.Distance(this.transform.position, targetObj.transform.position)>info.atkRange )//ѡ��Ŀ���ڹ�����Χ��
            {
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);//�ҵ�һ������
            }
            if (targetObj == null)
                return;//���û���ҵ��ɹ������� ���ؿ�
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;//��Ŀ��λ�ú�����ͷ���ȸߣ�������ŵף�
            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos - head.position), Time.deltaTime * roundSpeed);
            //Slerp������������Ԫ����Ŀ����Ԫ������ת�ٶȣ�LookRotation������Vector3������ʱ��
            if(Vector3.Angle(head.forward, monsterPos - head.position) < 5 //����1��ͷ����Ŀ�����н�<5
                && Time.time - nowTime > info.offsetTime)   //����2�����㹥���������
            {
                targetObj.Wound(info.atk);//��Ŀ������(����Ҫ�����߼����)
                GameDataMgr.Instance.PlaySound("Music/Tower");//������Ч
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);//����������Ч
                Destroy(effObj, 1);//�Ƴ���Ч
                nowTime = Time.time;//��¼����ʱ��
            }

        }
        else//Ⱥ�幥���߼�
        {
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);
            if( targetObjs.Count > 0 
                && Time.time-nowTime >= info.offsetTime )
            {
                //������Ч
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);//����������Ч
                Destroy(effObj, 1);//�Ƴ���Ч
                for (int i = 0; i < targetObjs.Count; i++)//����������Ŀ���ܵ��˺�
                {
                    targetObjs[i].Wound(info.atk);
                }
                nowTime = Time.time;//��¼����ʱ��
            }
        }   
    }
}
