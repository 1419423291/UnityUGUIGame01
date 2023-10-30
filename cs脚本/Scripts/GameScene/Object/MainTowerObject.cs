using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主塔，内置游戏失败逻辑
/// </summary>
public class MainTowerObject : MonoBehaviour
{
    public int hp;//血量相关
    public int maxHp;

    private bool isDead;//是否死亡

    private static MainTowerObject instance;//继承了MonoBehaveior不能new（在Awake中初始化即可）
    public static MainTowerObject Instance => instance;
    void Awake()
    {
        instance = this;
    }

    //更新血量
    public void UpdateHp(int Hp, int maxHP)
    {
        this.hp = Hp;
        this.maxHp = maxHP;
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(Hp, maxHP);//更新界面上血量图标
    }

    //受伤逻辑
    public void Wound(int dam)
    {
        if (isDead)//死亡不再扣血
            return;
        hp -= dam;//扣血 
        if(hp < 0)//死亡逻辑
        {
            hp = 0;
            isDead = true;
            //游戏结束逻辑（失败逻辑）
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();//显示结束面板
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);//失败得到奖励的一半
        }
        UpdateHp(hp, maxHp);//更新血量
    }
  
    private void OnDestroy()
    {
        instance = null;//过场景时（会自动删除）将其置空（释放内存）
    }
}
