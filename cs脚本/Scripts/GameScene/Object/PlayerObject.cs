using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ҷ����ʼ��������������λ�߼����ۼƽ�Ǯ
/// </summary>
public class PlayerObject : MonoBehaviour
{
    private Animator animator;

    private int atk;//��ҹ�����
    public int money;//��ҽ�Ǯ��
    private float roundSpeed = 50;//��ɫ��ת�ٶ�
    public Transform gunPoint;//��ǹ��ɫ�����λ��
    
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    #region 1.��ʼ��һ�������
    /// <summary>
    /// ��ҹ������ͽ�Ǯ��PlayerObject�ṩ���ⲿ��
    /// </summary>
    /// <param name="atk">������</param>
    /// <param name="money">��Ǯ</param>
    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;
        UpdateMoney();//����GamePanel����Ľ�Ǯ����
    }
    #endregion

    void Update()
    {
        #region 2.�ƶ��仯 �����仯
        //2.�ƶ��仯 �����仯
        //�ƶ��߼�����������λ�ƣ�
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        //��ת�߼�
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.LeftShift))//�¶��߼���Shift����
            animator.SetLayerWeight(1, 1);
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            animator.SetLayerWeight(1, 0);
        if (Input.GetKeyDown(KeyCode.LeftControl))//�����߼���ctrl����
            animator.SetTrigger("Roll");
        if (Input.GetMouseButton(0))
            animator.SetTrigger("Fire");
        if (Input.GetMouseButtonUp(0))
            animator.ResetTrigger("Fire");
        #endregion
    }

    #region 3.��ͬ���������Ĵ���
    /// <summary>
    /// �ֵ���ɫ�����˺���⣨������Χ��⣩
    /// </summary>
    public void KnifeEvent()
    {
        //��Χ�߼�
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));//colliders������⵽�����ж���
        #region ��Ҷ���Ĭ���ڽŵ׵�λ�á�1<<LayerMask.NameToLayer("Monster")�� ����Ϊ"Monster"��ͼ������ת��Ϊ��Ӧ��λ���루���м��ɣ�

        #endregion
        GameDataMgr.Instance.PlaySound("Music/Knife");//���Ŵ̵���Ч���

        for (int i = 0; i < colliders.Length; i++)
        {
            //�õ���ײ���Ķ������ϵĽű�ʹ֮����
            MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
            if (colliders[i] != null && !monster.isDead)
            {
                monster.Wound(this.atk);
                break;//ֱ��break��Ϸ�ֹһ����������Ѫ
            }
        }
    }
    /// <summary>
    /// ��ǹ��ɫ�����˺�����߼���ǹ�����߼�⣩
    /// </summary>
    public void ShootEvent()
    {
        //���߼��
        //RaycastHit[] hits = Physics.RaycastAll(new Ray(this.transform.position, gunPoint.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1000, 1 << LayerMask.NameToLayer("Monster"));//�������λ��
        
        GameDataMgr.Instance.PlaySound("Music/Gun");//���ſ�ǹ��Ч���
        for (int i = 0; i < hits.Length; i++)
        {
            //�õ���ײ���Ķ������ϵĽű�ʹ֮����
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));//������Ч���
                effObj.transform.position = hits[i].point;//��λ��Чλ�ã��Ӵ��㣩
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1);//�ӳ�ɾ����Ч
                monster.Wound(this.atk);
                break;//ֱ��break��Ϸ�ֹһ����������Ѫ
            }
        }
    }
    #endregion

    #region 4.��Ǯ�����߼���Ѫ�������߼��ڰ�ȫ���Լ����߼��У�
    /// <summary>
    /// ����GamePanel���Ľ�Ǯ��PlayerObject�ṩ��
    /// </summary>
    public void UpdateMoney()
    {
        //�ı���Ϸ�����Ǯ
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    /// <summary>
    /// ��Ǯ��PlayerObject�ṩ��
    /// </summary>
    public void AddMoney(int money)
    {
        this.money += money;//��Ǯ
        UpdateMoney(); 
    }
    #endregion
}
