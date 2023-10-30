using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance = instance;
    //面板是动态创建出来的：显示面板即直接实例化，得到面板即外部可以读取面板，隐藏面板即 将得到面板进行删除。
    //可见UIManager中要命名一个容器(字典)进行面板的存储方便后两者使用
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform canvasTrans;  //显示面板Transform
    private UIManager()
    {
        //得到场景中的Canvas对象
        //怎么保证Canvas对象唯一性？将其做成预设体而后保存下来过场景时使之不移除
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);//过场景不移除该对象(保证游戏过程中仅有一个Canvas对象)
    }
    public T ShowPanel<T>() where T : BasePanel//注：此处的ShowPanel是脚本对象
    {
        string panelName = typeof(T).Name;//注：泛型类型名要和面板预设体名一致
        if (panelDic.ContainsKey(panelName))//防止单一面板出现多个，故使用return打断
            return panelDic[panelName] as T;
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));//具体面板对象
        panelObj.transform.SetParent(canvasTrans, false);//面板对象设置父对象
        T panel = panelObj.GetComponent<T>();//获取具体面板的脚本
        panelDic.Add(panelName, panel);//存储脚本对象 
        panel.ShowMe();//显示具体面板
        return panel;//结束 补全逻辑
    }
    
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <param name="isFade">是否淡出删除</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel 
    {
        //isFade：是否需要淡出效果
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            //删除字典中对应内容(防止内存泄露：对象没了还进行引用)
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>//淡出结束后执行的匿名函数逻辑
                {
                    GameObject.Destroy(panelDic[panelName].gameObject);//删除对象
                    panelDic.Remove(panelName);//删除字典中对应对象
                }); 
            }
            else
            {
                GameObject.Destroy(panelDic[panelName].gameObject);//删除对象
                panelDic.Remove(panelName);//删除字典中对应对象
            }
        }
    }
    //得到面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        return null;
    }
}
