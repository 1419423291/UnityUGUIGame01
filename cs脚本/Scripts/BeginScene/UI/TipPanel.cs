using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Text txtInfo;//提示内容
    public Button btnSure;//确定按钮
    protected override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }
    /// <summary>
    /// 修改提示面板字符串内容（TipPanel提供）
    /// </summary>
    /// <param name="info"></param>
    public void ChangeInfo(string str)
    {
        txtInfo.text = str;
    }
}
