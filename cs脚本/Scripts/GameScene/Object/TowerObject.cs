using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    public Transform head;//炮塔头部 旋转指向目标
    public Transform gunPoint;//开火点，用于生成特效
    private float roundSpeed = 20;//炮台旋转速度
    private TowerInfo info;//（当前读取的）单个炮台数据
    private MonsterObject targetObj;//当前要攻击目标
    private List<MonsterObject> targetObjs;
    private float nowTime;//临时变量 计时攻击间隔时间
    private Vector3 monsterPos;//记录怪物的位置

    /// <summary>
    /// 初始化炮台相关数据（TowerObject）
    /// </summary>
    /// <param name="info">外部传入TowerInfo</param>
    public void InitInfo(TowerInfo info)
    {
        this.info = info;//将本地info设置为参数的info
    }

    
    void Update()//寻找目标
    {
        if (info.atkType == 1)//单体攻击逻辑（每次仅找一个对象）
        {   //寻找目标逻辑
            if(targetObj == null //未选中目标
                || targetObj.isDead //选中目标已死亡
                || Vector3.Distance(this.transform.position, targetObj.transform.position)>info.atkRange )//选中目标在攻击范围内
            {
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);//找到一个对象
            }
            if (targetObj == null)
                return;//如果没有找到可攻击对象 返回空
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;//让目标位置和炮塔头部等高（否则看向脚底）
            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos - head.position), Time.deltaTime * roundSpeed);
            //Slerp参数：传入四元数，目标四元数，旋转速度；LookRotation参数：Vector3向量；时间
            if(Vector3.Angle(head.forward, monsterPos - head.position) < 5 //条件1：头部和目标对象夹角<5
                && Time.time - nowTime > info.offsetTime)   //条件2：满足攻击间隔条件
            {
                targetObj.Wound(info.atk);//让目标受伤(不需要做射线检测了)
                GameDataMgr.Instance.PlaySound("Music/Tower");//播放音效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);//创建开火特效
                Destroy(effObj, 1);//移除特效
                nowTime = Time.time;//记录开火时间
            }

        }
        else//群体攻击逻辑
        {
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);
            if( targetObjs.Count > 0 
                && Time.time-nowTime >= info.offsetTime )
            {
                //创建特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);//创建开火特效
                Destroy(effObj, 1);//移除特效
                for (int i = 0; i < targetObjs.Count; i++)//满足条件的目标受到伤害
                {
                    targetObjs[i].Wound(info.atk);
                }
                nowTime = Time.time;//记录开火时间
            }
        }   
    }
}
