using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;//������������
    public Vector3 offsetPos;//��������Ŀ�������������ƫ��λ�ã����������-Ŀ�����꣩
    public float bodyHight;//����λ�õ�yƫ��ֵ
    public float moveSpeed;//ƽ���ٶ�
    public float rotationSpeed;//��ת�ٶ�
    private Vector3 targetPos;
    private Quaternion targetRotation;

    void Update()
    {
        //����Ŀ����� �����������ǰ λ�á��Ƕ�
        if (target == null)
            return;//������Ŀ����� �򲻽��м���
        targetPos = target.position + target.forward * offsetPos.z;//z��ƫ�ƣ���������ϵ��
        targetPos += Vector3.up * offsetPos.y;//y��ƫ��
        targetPos += target.right * offsetPos.x;//x��ƫ�ƣ���������ϵ��

        //��ֵ����ʵ�������ƽ������(��ͨ��lerp)
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);//����

        //������Ƕȸ���(��Ԫ����Slerp)
        targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHight
            - this.transform.position);//����������ɫ�Ų��Ϸ�bodyHeightλ��
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);//�����ƽ������Ŀ��
    }

    /// <summary>
    /// ����������������(CameraMove�ṩ)
    /// </summary>
    /// <param name="player">��ɫ</param>
    public void SetTarget(Transform player)
    {
        target = player;
    }
}
