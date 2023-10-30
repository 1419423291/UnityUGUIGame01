using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 游戏场景的关卡管理器（相关逻辑：关卡是否结束、动态创建角色、奖励金钱）。辅助MonsterObject脚本游戏胜利逻辑
/// </summary>
public class GameLevelMgr 
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    public PlayerObject player;//当前玩家对象脚本

    private List<MonsterPoint> points = new List<MonsterPoint>();//出怪点
    private int nowWaveNum = 0;//当前波数
    private int maxWaveNum = 0;//最大波数
    //private int nowMonsterNum = 0;//场景上的怪物数
    private List<MonsterObject> monstersList = new List<MonsterObject>();//记录当前场景上的怪物对象列表
    private GameLevelMgr() //防止外部实例化
    {

    }
    //1.切换到游戏场景时动态创建玩家
    public void InitInfo(SceneInfo info)
    {
        UIManager.Instance.ShowPanel<GamePanel>();//显示GamePanel面板
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;//玩家创建
        Transform heroPos = GameObject.Find("HeroBornPos").transform;//获取玩家出生点位HeroBornPos
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, heroPos.rotation);//实例化玩家出生点 
        player = heroObj.GetComponent<PlayerObject>();  
        player.InitPlayerInfo(roleInfo.atk, info.money);//初始化玩家对象攻击力、场景默认给的金钱
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);//让主摄像机看向玩家 
        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);//初始化主塔血量
        
    }
    //2.通过其判断游戏是否结束
    //场景中是否还有 未死亡的怪物、未出生的怪物

    /// <summary>
    /// 添加出怪点（GameLevelMgr脚本内）
    /// </summary>
    /// <param name="monsterPoint">单个出怪点</param>
    public void AddMonsterPoint(MonsterPoint monsterPoint)
    {
        points.Add(monsterPoint);
    } 

    /// <summary>
    /// 初始化一共多少波怪（每场游戏一次）
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);//更新界面怪物波数
    }
    /// <summary>
    /// 更新当前还有多少波怪（每场游戏多次）
    /// </summary>
    /// <param name="num"></param>
    public void ChangeNowNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);//更新界面怪物波数
    }
    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver())//只要有一个出怪点没有出完怪就不算胜利
                return false;
        }
        //if (nowMonsterNum > 0)//场景上存活怪物数不为0
        if( monstersList.Count >0 )
            return false;
        return true;
    }
    /// <summary>
    /// 添加怪物到列表中
    /// </summary>
    /// <param name="Obj">怪物对象</param>
    public void AddMonster(MonsterObject Obj)
    {
        monstersList.Add(Obj);//添加怪物到列表中
    }
    /// <summary>
    /// 将怪物从列表中移除，死亡时调用
    /// </summary>
    /// <param name="Obj"></param>
    public void RemoveMonster(MonsterObject Obj)
    {
        monstersList.Remove(Obj);//从怪物到列表中移除
    }
    /// <summary>
    /// 寻找满足条件的怪物（GameLevelMgr游戏场景管理器提供）
    /// </summary>
    /// <param name="pos">塔坐标</param>
    /// <param name="range">塔攻击范围</param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos,int range)//遍历判断怪物和传入的位置之间的距离是否小于Range参数，方便射程使用
    {
        for (int i = 0; i < monstersList.Count; i++)//在怪物列表中找到满足距离条件的怪物 用于塔的攻击
        {
            if ( !monstersList[i].isDead && Vector3.Distance(pos, monstersList[i].transform.position) <= range )//找到目标并返回
            {
                return monstersList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 寻找满足条件的所有怪物，并且将他们记录在一个列表中
    /// </summary>
    /// <param name="pos">塔坐标</param>
    /// <param name="range">塔攻击范围</param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 pos,int range)
    {
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monstersList.Count; i++)//在怪物列表中找到满足距离条件的怪物 用于塔的攻击
        {
            if (!monstersList[i].isDead && Vector3.Distance(pos, monstersList[i].transform.position) <= range)//找到目标并返回
            {
                list.Add(monstersList[i]);
            }
        }
        return list;
    }
    public void ClearInfo()
    {
        points.Clear();//清空僵尸出生点
        monstersList.Clear();//清空场景上的怪物列表
        //nowWaveNum = nowMonsterNum = maxWaveNum = 0;//清空场景的波数数据及相关临时变量
        nowWaveNum = maxWaveNum = 0;//清空场景的波数数据及相关临时变量
        player = null;//置空角色脚本
    }
}
