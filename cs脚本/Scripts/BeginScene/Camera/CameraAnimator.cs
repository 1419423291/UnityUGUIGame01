using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    /// <summary>
    /// ί�к���(�ⲿ����)�����붯���������Ҫ����ί�к���
    /// </summary>
    private UnityAction overAction;
    void Start()
    {
        animator = this.GetComponent<Animator>();//�õ�������ϵĶ���
    }
    //��ת��ע������������Ϻ�����ʾ��壬������ʾ������д��ί�к����ڣ�
    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");
        overAction = action;
    }
    //��ת
    public void TurnRight(UnityAction action)
    {
        animator.SetTrigger("Right");
        overAction = action;
    }
    /// <summary>
    /// �������������ʱ����õķ���������Ϊʹ�õ��ź���Trigger��ͨ������¼���ʽ����ʾTurnLeft\TurnRight����������ɣ�
    /// </summary>
    public void PlayerOver()
    {
        overAction?.Invoke();
        overAction = null;
    }
}
