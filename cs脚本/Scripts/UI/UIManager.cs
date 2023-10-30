using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance = instance;
    //����Ƕ�̬���������ģ���ʾ��弴ֱ��ʵ�������õ���弴�ⲿ���Զ�ȡ��壬������弴 ���õ�������ɾ����
    //�ɼ�UIManager��Ҫ����һ������(�ֵ�)�������Ĵ洢���������ʹ��
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform canvasTrans;  //��ʾ���Transform
    private UIManager()
    {
        //�õ������е�Canvas����
        //��ô��֤Canvas����Ψһ�ԣ���������Ԥ������󱣴�����������ʱʹ֮���Ƴ�
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);//���������Ƴ��ö���(��֤��Ϸ�����н���һ��Canvas����)
    }
    public T ShowPanel<T>() where T : BasePanel//ע���˴���ShowPanel�ǽű�����
    {
        string panelName = typeof(T).Name;//ע������������Ҫ�����Ԥ������һ��
        if (panelDic.ContainsKey(panelName))//��ֹ��һ�����ֶ������ʹ��return���
            return panelDic[panelName] as T;
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));//����������
        panelObj.transform.SetParent(canvasTrans, false);//���������ø�����
        T panel = panelObj.GetComponent<T>();//��ȡ�������Ľű�
        panelDic.Add(panelName, panel);//�洢�ű����� 
        panel.ShowMe();//��ʾ�������
        return panel;//���� ��ȫ�߼�
    }
    
    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="isFade">�Ƿ񵭳�ɾ��</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel 
    {
        //isFade���Ƿ���Ҫ����Ч��
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            //ɾ���ֵ��ж�Ӧ����(��ֹ�ڴ�й¶������û�˻���������)
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>//����������ִ�е����������߼�
                {
                    GameObject.Destroy(panelDic[panelName].gameObject);//ɾ������
                    panelDic.Remove(panelName);//ɾ���ֵ��ж�Ӧ����
                }); 
            }
            else
            {
                GameObject.Destroy(panelDic[panelName].gameObject);//ɾ������
                panelDic.Remove(panelName);//ɾ���ֵ��ж�Ӧ����
            }
        }
    }
    //�õ����
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        return null;
    }
}
