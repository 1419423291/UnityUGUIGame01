using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //专门控制面板的组件
    private CanvasGroup canvasGroup;
    //面板淡入淡出速度
    private float alphaSpeed = 10;
    //面板当前是否显示
    public bool isShow = false;
    //面板隐藏完毕后要做的事情
    private UnityAction hideCallBack = null;
    //Awake函数里面自动获取CanvasGroup组件
    //为什么将Awake和Start写成保护的虚函数？ 不让外部调用且子类可自行重写
    protected virtual void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init(); 
    }
    /// <summary>
    /// 注册控件事件的方法 所有的子面板都要 注册一些控件（子类自行实现）
    /// </summary>
    protected abstract void Init();
    /// <summary>
    /// 面板显示自己时逻辑
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }
    /// <summary>
    /// 面板隐藏自己时 逻辑
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallBack = callBack;
    }

    protected virtual void Update()
    {
        //显示状态(淡入):不为1,自增 
        if ( isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        //隐藏状态(淡出):不为0,自减
        else if ( !isShow && canvasGroup.alpha != 0)//此处使用else if是因为if部分为两个独立条件的结合
        {
            canvasGroup.alpha -= Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //面板淡出完成后 hideCallBack不为空即调用
                hideCallBack?.Invoke();
            }
        }
    }
}
