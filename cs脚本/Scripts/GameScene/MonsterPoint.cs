using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    public int maxWave;//怪物波数
    public int monsterNumOneWave;//每波多少只

    private int nowNum;//临时变量：当前这波还有多少怪物未创建
    public List<int> monsterIDs;//怪物id 
    private int nowID;//当前这波怪物id
    public int createOffsetTime;//单只怪物创建间隔时间
    public float delayTime;//怪物波数间隔
    public float firstDelayTime;//第一波怪物创建间隔
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);//延迟创建第一波怪物
        GameLevelMgr.Instance.AddMonsterPoint(this);//记录出怪点
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);//更新最大波数
    }
    /// <summary>
    /// 开始创建一波怪物
    /// </summary>
    private void CreateWave()
    {
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];//当前这波怪物的id为数组索引值为0-1对应值（左包含0右不包含2）
        nowNum = monsterNumOneWave;//当前这波怪物多少只
        CreateMonster();//创建怪物
        --maxWave;//减少波数
        GameLevelMgr.Instance.ChangeNowNum(1);//更新剩余波数（每出一波对应减少一次）
    }
    /// <summary>
    /// 创建一只怪物（波内）
    /// </summary>
    private void CreateMonster()
    {
        MonsterInfo monsterInfo = GameDataMgr.Instance.monsterInfoList[nowID - 1];//取出怪物数据(列表的索引从0开始)
        GameObject obj = Instantiate(Resources.Load<GameObject>(monsterInfo.res), this.transform.position, Quaternion.identity);//创建怪物
        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();//添加怪物脚本
        monsterObj.InitInfo(monsterInfo);//怪物初始化血量速度等
        //GameLevelMgr.Instance.ChangeMonsterNum(1);//GameLevelMgr中存活怪物数+1
        GameLevelMgr.Instance.AddMonster(monsterObj);//将怪物添加到场景管理器的怪物列表中
        --nowNum;//创建一只怪物则数量-1
        if(nowNum == 0)
        {
            if (maxWave > 0)
                Invoke("CreateWave", delayTime);//递归创建一波(如果满足条件)
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);//递归创建一只
        }
    }
    /// <summary>
    /// 出怪点是否完成出怪（MonsterPoint提供）
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
