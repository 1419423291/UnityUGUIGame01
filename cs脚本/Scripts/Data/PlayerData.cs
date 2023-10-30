using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家本地数据：money、解锁英雄
/// </summary>
public class PlayerData 
{
    //当前拥有money
    public int haveMoney = 0;
    //当前解锁英雄
    public List<int> buyHero = new List<int>();
}
