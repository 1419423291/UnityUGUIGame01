using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo 
{
    public int id;//id
    public string name;//名字
    public int money;//金钱
    public int atk;//攻击力
    public int atkRange;//攻击范围
    public float offsetTime;//攻击间隔（攻速）
    public int nextLev;//下一级id
    public string imgRes;//图片路径
    public string res;//路径
    public int atkType;//攻击方式（1单体2范围）
    public string eff;//攻击特效路径
}
