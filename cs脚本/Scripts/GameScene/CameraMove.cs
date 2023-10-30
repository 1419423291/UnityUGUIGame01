using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;//摄像机看向对象
    public Vector3 offsetPos;//摄像机相对目标对象在三轴上偏移位置（摄像机坐标-目标坐标）
    public float bodyHight;//看向位置的y偏移值
    public float moveSpeed;//平移速度
    public float rotationSpeed;//旋转速度
    private Vector3 targetPos;
    private Quaternion targetRotation;

    void Update()
    {
        //根据目标对象 计算摄像机当前 位置、角度
        if (target == null)
            return;//不存在目标对象 则不进行计算
        targetPos = target.position + target.forward * offsetPos.z;//z轴偏移（人物坐标系）
        targetPos += Vector3.up * offsetPos.y;//y轴偏移
        targetPos += target.right * offsetPos.x;//x轴偏移（人物坐标系）

        //插值运算实现摄像机平滑跟随(普通用lerp)
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);//跟随

        //摄像机角度跟随(四元数用Slerp)
        targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHight
            - this.transform.position);//摄像机看向角色脚部上方bodyHeight位置
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);//摄像机平滑看向目标
    }

    /// <summary>
    /// 设置摄像机跟随对象(CameraMove提供)
    /// </summary>
    /// <param name="player">角色</param>
    public void SetTarget(Transform player)
    {
        target = player;
    }
}
