using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Text txtInfo;//��ʾ����
    public Button btnSure;//ȷ����ť
    protected override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }
    /// <summary>
    /// �޸���ʾ����ַ������ݣ�TipPanel�ṩ��
    /// </summary>
    /// <param name="info"></param>
    public void ChangeInfo(string str)
    {
        txtInfo.text = str;
    }
}
