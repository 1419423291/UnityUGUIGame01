using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 组合控件（方便控制造塔相关UI更新逻辑）
/// </summary>
public class TowerBtn : MonoBehaviour
{
    public Image imgPic;//塔图片
    public Text txtTip;//数字键位
    public Text txtMoney;//花费金额
    /// <summary>
    /// 更新Bot对象的TowerBtn
    /// </summary>
    /// <param name="id">造塔点id</param>
    /// <param name="inputStr">造塔点键位提示</param>
    public void InitInfo(int id,string inputStr)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];//索引从0开始
        imgPic.sprite = Resources.Load<Sprite>(info.imgRes);//加载图片
        txtMoney.text = "$"+info.money;//造塔文本提示
        RectTransform transform = txtMoney.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(200, 57.297f);
        txtTip.text=inputStr.ToString();//初始化键位提示
        if(info.money > GameLevelMgr.Instance.player.money)//如果钱不够
        {
            txtMoney.text = "金钱不足!";
        }
    }
}
