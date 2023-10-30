using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    /// <summary>
    /// 委托函数(外部传入)。传入动画播放完后要进行委托函数
    /// </summary>
    private UnityAction overAction;
    void Start()
    {
        animator = this.GetComponent<Animator>();//得到摄像机上的动画
    }
    //左转（注：动画播放完毕后再显示面板，所以显示面板可以写在委托函数内）
    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");
        overAction = action;
    }
    //右转
    public void TurnRight(UnityAction action)
    {
        animator.SetTrigger("Right");
        overAction = action;
    }
    /// <summary>
    /// 当动画播放完成时会调用的方法。（因为使用的信号是Trigger故通过添加事件形式来表示TurnLeft\TurnRight动画播放完成）
    /// </summary>
    public void PlayerOver()
    {
        overAction?.Invoke();
        overAction = null;
    }
}
