using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    public int maxWave;//���ﲨ��
    public int monsterNumOneWave;//ÿ������ֻ

    private int nowNum;//��ʱ��������ǰ�Ⲩ���ж��ٹ���δ����
    public List<int> monsterIDs;//����id 
    private int nowID;//��ǰ�Ⲩ����id
    public int createOffsetTime;//��ֻ���ﴴ�����ʱ��
    public float delayTime;//���ﲨ�����
    public float firstDelayTime;//��һ�����ﴴ�����
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);//�ӳٴ�����һ������
        GameLevelMgr.Instance.AddMonsterPoint(this);//��¼���ֵ�
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);//���������
    }
    /// <summary>
    /// ��ʼ����һ������
    /// </summary>
    private void CreateWave()
    {
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];//��ǰ�Ⲩ�����idΪ��������ֵΪ0-1��Ӧֵ�������0�Ҳ�����2��
        nowNum = monsterNumOneWave;//��ǰ�Ⲩ�������ֻ
        CreateMonster();//��������
        --maxWave;//���ٲ���
        GameLevelMgr.Instance.ChangeNowNum(1);//����ʣ�ನ����ÿ��һ����Ӧ����һ�Σ�
    }
    /// <summary>
    /// ����һֻ������ڣ�
    /// </summary>
    private void CreateMonster()
    {
        MonsterInfo monsterInfo = GameDataMgr.Instance.monsterInfoList[nowID - 1];//ȡ����������(�б��������0��ʼ)
        GameObject obj = Instantiate(Resources.Load<GameObject>(monsterInfo.res), this.transform.position, Quaternion.identity);//��������
        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();//��ӹ���ű�
        monsterObj.InitInfo(monsterInfo);//�����ʼ��Ѫ���ٶȵ�
        //GameLevelMgr.Instance.ChangeMonsterNum(1);//GameLevelMgr�д�������+1
        GameLevelMgr.Instance.AddMonster(monsterObj);//��������ӵ������������Ĺ����б���
        --nowNum;//����һֻ����������-1
        if(nowNum == 0)
        {
            if (maxWave > 0)
                Invoke("CreateWave", delayTime);//�ݹ鴴��һ��(�����������)
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);//�ݹ鴴��һֻ
        }
    }
    /// <summary>
    /// ���ֵ��Ƿ���ɳ��֣�MonsterPoint�ṩ��
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
