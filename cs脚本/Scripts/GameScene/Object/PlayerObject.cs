using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家对象初始化：攻击力、键位逻辑、累计金钱
/// </summary>
public class PlayerObject : MonoBehaviour
{
    private Animator animator;

    private int atk;//玩家攻击力
    public int money;//玩家金钱数
    private float roundSpeed = 50;//角色旋转速度
    public Transform gunPoint;//持枪角色开火点位置
    
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    #region 1.初始玩家基础属性
    /// <summary>
    /// 玩家攻击力和金钱（PlayerObject提供给外部）
    /// </summary>
    /// <param name="atk">攻击力</param>
    /// <param name="money">金钱</param>
    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;
        UpdateMoney();//更新GamePanel界面的金钱数量
    }
    #endregion

    void Update()
    {
        #region 2.移动变化 动作变化
        //2.移动变化 动作变化
        //移动逻辑（动画中有位移）
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        //旋转逻辑
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.LeftShift))//下蹲逻辑（Shift键）
            animator.SetLayerWeight(1, 1);
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            animator.SetLayerWeight(1, 0);
        if (Input.GetKeyDown(KeyCode.LeftControl))//滚动逻辑（ctrl键）
            animator.SetTrigger("Roll");
        if (Input.GetMouseButton(0))
            animator.SetTrigger("Fire");
        if (Input.GetMouseButtonUp(0))
            animator.ResetTrigger("Fire");
        #endregion
    }

    #region 3.不同攻击动作的处理
    /// <summary>
    /// 持刀角色攻击伤害检测（刀，范围检测）
    /// </summary>
    public void KnifeEvent()
    {
        //范围逻辑
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));//colliders即被检测到的所有对象
        #region 玩家对象默认在脚底的位置。1<<LayerMask.NameToLayer("Monster")即 把名为"Monster"的图层索引转换为对应的位掩码（背诵即可）

        #endregion
        GameDataMgr.Instance.PlaySound("Music/Knife");//播放刺刀音效组件

        for (int i = 0; i < colliders.Length; i++)
        {
            //得到碰撞到的对象身上的脚本使之受伤
            MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
            if (colliders[i] != null && !monster.isDead)
            {
                monster.Wound(this.atk);
                break;//直接break打断防止一刀多个怪物掉血
            }
        }
    }
    /// <summary>
    /// 持枪角色攻击伤害检测逻辑（枪，射线检测）
    /// </summary>
    public void ShootEvent()
    {
        //射线检测
        //RaycastHit[] hits = Physics.RaycastAll(new Ray(this.transform.position, gunPoint.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1000, 1 << LayerMask.NameToLayer("Monster"));//鼠标输入位置
        
        GameDataMgr.Instance.PlaySound("Music/Gun");//播放开枪音效组件
        for (int i = 0; i < hits.Length; i++)
        {
            //得到碰撞到的对象身上的脚本使之受伤
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));//播放特效组件
                effObj.transform.position = hits[i].point;//定位特效位置（接触点）
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1);//延迟删除特效
                monster.Wound(this.atk);
                break;//直接break打断防止一刀多个怪物掉血
            }
        }
    }
    #endregion

    #region 4.金钱更新逻辑（血量更新逻辑在安全区自己的逻辑中）
    /// <summary>
    /// 更新GamePanel面板的金钱（PlayerObject提供）
    /// </summary>
    public void UpdateMoney()
    {
        //改变游戏界面金钱
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    /// <summary>
    /// 加钱（PlayerObject提供）
    /// </summary>
    public void AddMoney(int money)
    {
        this.money += money;//加钱
        UpdateMoney(); 
    }
    #endregion
}
