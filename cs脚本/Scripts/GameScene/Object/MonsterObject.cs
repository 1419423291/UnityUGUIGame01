using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 怪物用脚本，内置游戏胜利逻辑
/// </summary>
public class MonsterObject : MonoBehaviour
{
    
    private Animator animator;//动画相关
    private NavMeshAgent agent;//位移相关 寻路组件

    private MonsterInfo monsterInfo;//一些不变的数据
    private int hp;//当前血量
    public bool isDead = false;//是否死亡（外部可调）
    private float frontTime = 0;//上次攻击的时间（临时变量，用于攻击逻辑）
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();//寻路组件   
        animator = this.GetComponent<Animator>();//动画组件
    }

    //初始化方法
    // 初始化怪物
    public void InitInfo(MonsterInfo info)//做了出怪点后调用
    {
        monsterInfo = info;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);//状态机记载
        hp = info.hp;//初始化血量
        agent.speed = info.moveSpeed;//移动速度、加速度（保证没有明显的加速运动，看起来像匀速）
        agent.angularSpeed = info.roundSpeed;//旋转速度
    }

    //受伤
    // 受伤函数（MonsterObject提供，外部可调用）
    public void Wound(int dmg)
    {
        if (isDead)
            return;//死亡就不能再受伤
        hp -= dmg;//减少血量
        animator.SetTrigger("Wound");//播放受伤动画

        if (hp <= 0)
        {
            //血量为0死亡
            Dead();
        }
        else
        {
            GameDataMgr.Instance.PlaySound("Music/Wound");//受伤音效播放
        }
    }

    
    // 死亡函数
    public void Dead()
    {
        isDead = true;
        //agent.isStopped = true;//停止自动寻路的移动
        agent.enabled = false;//停止寻路
        animator.SetBool("Dead", true);//播放死亡动画
        GameDataMgr.Instance.PlaySound("Music/dead");//播放死亡音效
        //加钱（待处理）
        GameLevelMgr.Instance.player.AddMoney(15);
    }


    // 死亡动画结束后事件（胜利逻辑）
    public void DeadEvent()//移除尸体（减少内存）
    {
        //死亡动画播放完毕后移除尸体
        //有了关卡管理器再来处理
        //GameLevelMgr.Instance.ChangeMonsterNum(-1);//死掉一只怪物就减1
        GameLevelMgr.Instance.RemoveMonster(this);//从场景管理器的怪物列表移除
        Destroy(this.gameObject);//（动画播放完后）移除已经死亡的对象尸体
        if (GameLevelMgr.Instance.CheckOver())
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//显示胜利面板
            panel.InitInfo(GameLevelMgr.Instance.player.money, true);//初始化结束界面
        }
    }


    // 出生动画结束后事件
    public void BornOver()//出生后再进行移动（移动--寻路组件）
    {
        agent.SetDestination(MainTowerObject.Instance.transform.position);//MainTowerObject是单例模式脚本，可以从脚本获取坐标
        animator.SetBool("Run", true);//播放移动动画
    }

    //攻击
    void Update()//什么时候进行攻击检测（停止跑步）
    {
        if (isDead)
            return;
        animator.SetBool("Run", agent.velocity != Vector3.zero);//根据其在寻路系统中的速度决定是否播放
        if (Vector3.Distance(this.transform.position, MainTowerObject.Instance.transform.position) < 5
            && Time.time - frontTime >= monsterInfo.atkOffset)//攻击逻辑（和目标点在若干距离以内）
        {
            frontTime = Time.time;//记录这次攻击的时间
            animator.SetTrigger("Atk");
        }
    }

    //伤害检测
    public void AtkEvent()
    {
        //范围检测进行伤害判断
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,1, 1 << LayerMask.NameToLayer("MainTower"));//注：this.transform.position是脚底的点
        GameDataMgr.Instance.PlaySound("Music/Eat");//播放攻击音效

        for (int i = 0; i < colliders.Length; i++)
        {
            if (MainTowerObject.Instance.gameObject == colliders[i].gameObject)
            //或if (MainTowerObject.Instance.transform == colliders[i].transform)
            {
                MainTowerObject.Instance.Wound(monsterInfo.atk);//让当前保护区受伤
            }
        }
    }
}
