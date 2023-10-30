using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    private GameObject towerObj = null;//点上的塔对象
    public TowerInfo nowTowerInfo = null;//点上的塔数据
    public List<int> chooseIDs= new List<int>{ 1, 4, 7 };//可建造的塔点的id
    /// <summary>
    /// 造塔逻辑
    /// </summary>
    /// <param name="id">防御塔id</param>
    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];//索引从0开始
        if (info.money > GameLevelMgr.Instance.player.money)//钱不够不给造塔
            return;
        GameLevelMgr.Instance.player.AddMoney(-info.money);//扣钱 
        if(towerObj != null)//判断之前是否有塔，有就删掉
        {
            Destroy(towerObj);
            towerObj = null;
        }
        towerObj = GameObject.Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);//实例化塔对象
        towerObj.GetComponent<TowerObject>().InitInfo(info);//初始化塔的数据
        nowTowerInfo = info;//记录当前塔的信息（判断下一等级的塔或能不能造塔）
        //建塔完毕要更新游戏界面内容
        if (nowTowerInfo.nextLev != 0)//可以升级
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else//塔是最高级
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    //造塔面板的显示隐藏
    private void OnTriggerEnter(Collider other)
    {
        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0)//造塔点最高级不显示
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);//动态更新造塔点
    }
    private void OnTriggerExit(Collider other)
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);//不想显示造塔点界面直接传null即可
    }

}
