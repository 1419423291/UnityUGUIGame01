using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// ��Ϸ�����Ĺؿ�������������߼����ؿ��Ƿ��������̬������ɫ��������Ǯ��������MonsterObject�ű���Ϸʤ���߼�
/// </summary>
public class GameLevelMgr 
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    public PlayerObject player;//��ǰ��Ҷ���ű�

    private List<MonsterPoint> points = new List<MonsterPoint>();//���ֵ�
    private int nowWaveNum = 0;//��ǰ����
    private int maxWaveNum = 0;//�����
    //private int nowMonsterNum = 0;//�����ϵĹ�����
    private List<MonsterObject> monstersList = new List<MonsterObject>();//��¼��ǰ�����ϵĹ�������б�
    private GameLevelMgr() //��ֹ�ⲿʵ����
    {

    }
    //1.�л�����Ϸ����ʱ��̬�������
    public void InitInfo(SceneInfo info)
    {
        UIManager.Instance.ShowPanel<GamePanel>();//��ʾGamePanel���
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;//��Ҵ���
        Transform heroPos = GameObject.Find("HeroBornPos").transform;//��ȡ��ҳ�����λHeroBornPos
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, heroPos.rotation);//ʵ������ҳ����� 
        player = heroObj.GetComponent<PlayerObject>();  
        player.InitPlayerInfo(roleInfo.atk, info.money);//��ʼ����Ҷ��󹥻���������Ĭ�ϸ��Ľ�Ǯ
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);//���������������� 
        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);//��ʼ������Ѫ��
        
    }
    //2.ͨ�����ж���Ϸ�Ƿ����
    //�������Ƿ��� δ�����Ĺ��δ�����Ĺ���

    /// <summary>
    /// ��ӳ��ֵ㣨GameLevelMgr�ű��ڣ�
    /// </summary>
    /// <param name="monsterPoint">�������ֵ�</param>
    public void AddMonsterPoint(MonsterPoint monsterPoint)
    {
        points.Add(monsterPoint);
    } 

    /// <summary>
    /// ��ʼ��һ�����ٲ��֣�ÿ����Ϸһ�Σ�
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);//���½�����ﲨ��
    }
    /// <summary>
    /// ���µ�ǰ���ж��ٲ��֣�ÿ����Ϸ��Σ�
    /// </summary>
    /// <param name="num"></param>
    public void ChangeNowNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);//���½�����ﲨ��
    }
    /// <summary>
    /// ����Ƿ�ʤ��
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver())//ֻҪ��һ�����ֵ�û�г���־Ͳ���ʤ��
                return false;
        }
        //if (nowMonsterNum > 0)//�����ϴ���������Ϊ0
        if( monstersList.Count >0 )
            return false;
        return true;
    }
    /// <summary>
    /// ��ӹ��ﵽ�б���
    /// </summary>
    /// <param name="Obj">�������</param>
    public void AddMonster(MonsterObject Obj)
    {
        monstersList.Add(Obj);//��ӹ��ﵽ�б���
    }
    /// <summary>
    /// ��������б����Ƴ�������ʱ����
    /// </summary>
    /// <param name="Obj"></param>
    public void RemoveMonster(MonsterObject Obj)
    {
        monstersList.Remove(Obj);//�ӹ��ﵽ�б����Ƴ�
    }
    /// <summary>
    /// Ѱ�����������Ĺ��GameLevelMgr��Ϸ�����������ṩ��
    /// </summary>
    /// <param name="pos">������</param>
    /// <param name="range">��������Χ</param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos,int range)//�����жϹ���ʹ����λ��֮��ľ����Ƿ�С��Range�������������ʹ��
    {
        for (int i = 0; i < monstersList.Count; i++)//�ڹ����б����ҵ�������������Ĺ��� �������Ĺ���
        {
            if ( !monstersList[i].isDead && Vector3.Distance(pos, monstersList[i].transform.position) <= range )//�ҵ�Ŀ�겢����
            {
                return monstersList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// Ѱ���������������й�����ҽ����Ǽ�¼��һ���б���
    /// </summary>
    /// <param name="pos">������</param>
    /// <param name="range">��������Χ</param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 pos,int range)
    {
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monstersList.Count; i++)//�ڹ����б����ҵ�������������Ĺ��� �������Ĺ���
        {
            if (!monstersList[i].isDead && Vector3.Distance(pos, monstersList[i].transform.position) <= range)//�ҵ�Ŀ�겢����
            {
                list.Add(monstersList[i]);
            }
        }
        return list;
    }
    public void ClearInfo()
    {
        points.Clear();//��ս�ʬ������
        monstersList.Clear();//��ճ����ϵĹ����б�
        //nowWaveNum = nowMonsterNum = maxWaveNum = 0;//��ճ����Ĳ������ݼ������ʱ����
        nowWaveNum = maxWaveNum = 0;//��ճ����Ĳ������ݼ������ʱ����
        player = null;//�ÿս�ɫ�ű�
    }
}
