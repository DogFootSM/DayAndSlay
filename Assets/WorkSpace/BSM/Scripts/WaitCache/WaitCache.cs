using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WaitCache : MonoBehaviour
{

    private Dictionary<float, WaitForSeconds> WaitDict = new();

    /// <summary>
    /// WaitForSeconds 객체 반환
    /// </summary>
    /// <param name="waitTime">키로 사용할 대기 시간</param>
    /// <returns></returns>
    public WaitForSeconds GetWait(float waitTime)
    {
        if (!WaitDict.ContainsKey(waitTime))
        {
            WaitDict[waitTime] = new WaitForSeconds(waitTime); 
        } 
        
        return WaitDict[waitTime]; 
    }


}
