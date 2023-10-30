using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ͽؼ�����������������UI�����߼���
/// </summary>
public class TowerBtn : MonoBehaviour
{
    public Image imgPic;//��ͼƬ
    public Text txtTip;//���ּ�λ
    public Text txtMoney;//���ѽ��
    /// <summary>
    /// ����Bot�����TowerBtn
    /// </summary>
    /// <param name="id">������id</param>
    /// <param name="inputStr">�������λ��ʾ</param>
    public void InitInfo(int id,string inputStr)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];//������0��ʼ
        imgPic.sprite = Resources.Load<Sprite>(info.imgRes);//����ͼƬ
        txtMoney.text = "$"+info.money;//�����ı���ʾ
        RectTransform transform = txtMoney.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(200, 57.297f);
        txtTip.text=inputStr.ToString();//��ʼ����λ��ʾ
        if(info.money > GameLevelMgr.Instance.player.money)//���Ǯ����
        {
            txtMoney.text = "��Ǯ����!";
        }
    }
}
