using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //ר�ſ����������
    private CanvasGroup canvasGroup;
    //��嵭�뵭���ٶ�
    private float alphaSpeed = 10;
    //��嵱ǰ�Ƿ���ʾ
    public bool isShow = false;
    //���������Ϻ�Ҫ��������
    private UnityAction hideCallBack = null;
    //Awake���������Զ���ȡCanvasGroup���
    //Ϊʲô��Awake��Startд�ɱ������麯���� �����ⲿ�����������������д
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
    /// ע��ؼ��¼��ķ��� ���е�����嶼Ҫ ע��һЩ�ؼ�����������ʵ�֣�
    /// </summary>
    protected abstract void Init();
    /// <summary>
    /// �����ʾ�Լ�ʱ�߼�
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }
    /// <summary>
    /// ��������Լ�ʱ �߼�
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallBack = callBack;
    }

    protected virtual void Update()
    {
        //��ʾ״̬(����):��Ϊ1,���� 
        if ( isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        //����״̬(����):��Ϊ0,�Լ�
        else if ( !isShow && canvasGroup.alpha != 0)//�˴�ʹ��else if����Ϊif����Ϊ�������������Ľ��
        {
            canvasGroup.alpha -= Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //��嵭����ɺ� hideCallBack��Ϊ�ռ�����
                hideCallBack?.Invoke();
            }
        }
    }
}
