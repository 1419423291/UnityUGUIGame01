using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    private GameObject towerObj = null;//���ϵ�������
    public TowerInfo nowTowerInfo = null;//���ϵ�������
    public List<int> chooseIDs= new List<int>{ 1, 4, 7 };//�ɽ���������id
    /// <summary>
    /// �����߼�
    /// </summary>
    /// <param name="id">������id</param>
    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];//������0��ʼ
        if (info.money > GameLevelMgr.Instance.player.money)//Ǯ������������
            return;
        GameLevelMgr.Instance.player.AddMoney(-info.money);//��Ǯ 
        if(towerObj != null)//�ж�֮ǰ�Ƿ��������о�ɾ��
        {
            Destroy(towerObj);
            towerObj = null;
        }
        towerObj = GameObject.Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);//ʵ����������
        towerObj.GetComponent<TowerObject>().InitInfo(info);//��ʼ����������
        nowTowerInfo = info;//��¼��ǰ������Ϣ���ж���һ�ȼ��������ܲ���������
        //�������Ҫ������Ϸ��������
        if (nowTowerInfo.nextLev != 0)//��������
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else//������߼�
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    //����������ʾ����
    private void OnTriggerEnter(Collider other)
    {
        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0)//��������߼�����ʾ
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);//��̬����������
    }
    private void OnTriggerExit(Collider other)
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);//������ʾ���������ֱ�Ӵ�null����
    }

}
