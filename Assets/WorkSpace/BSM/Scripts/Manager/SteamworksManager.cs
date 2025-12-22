using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class SteamworksManager : MonoBehaviour
{
    private const uint AppId = 4276190;

    public static SteamworksManager SteamworksInstance;
    
    private int[] StatMaxValues = new int[(int)SteamStatAPI.SIZE] {100,500,1,1,1,1};
    private bool connectedToSteam = false;
    
    
    
    private void Awake()
    {
        if (SteamworksInstance == null)
        {
            SteamworksInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        try
        {
            Steamworks.SteamClient.Init(AppId); 
            connectedToSteam = true;
            Debug.Log($"연결됨. {Steamworks.SteamClient.State}");
        }
        catch (Exception e)
        {
            Debug.Log("연결 실패");
            connectedToSteam = false;
        }
    }

    private void Update()
    {
        if (connectedToSteam)
        {
            //스팀 연결 상태일 경우 프레임마다 Callback 호출
            Steamworks.SteamClient.RunCallbacks();
        } 
    }
    
    
    /// <summary>
    /// 스팀 연결 해제
    /// </summary>
    public void DisconnectFromSteam()
    {
        if (connectedToSteam)
        {
            Steamworks.SteamClient.Shutdown();
        } 
    } 
    
    /// <summary>
    /// 도전 과제 해금
    /// </summary>
    /// <param name="achievementAPI"></param>
    public void UnlockAchievement(SteamAchievementAPI achievementAPI)
    { 
        if (connectedToSteam)
        {
            var ach = new Steamworks.Data.Achievement("ACHIEVEMENT" + (int)achievementAPI);
            ach.Trigger();
        }
    }

    /// <summary>
    /// 현재 도전 과제가 해금된 상태인지 확인
    /// </summary>
    /// <param name="achievementAPI"></param>
    /// <returns></returns>
    public bool CheckUnlockAchievement(SteamAchievementAPI achievementAPI)
    {
        var ach = new Steamworks.Data.Achievement("ACHIEVEMENT" + (int)achievementAPI);
         
        return ach.State;
    }
 
    /// <summary>
    /// 도전 과제 클리어 조건 확인
    /// </summary>
    /// <param name="statsKey">통계 API 키 And Max Value 인덱스</param>
    /// <returns></returns> 
    public bool CheckUserStat(SteamStatAPI[] statsKey)
    {
        for (int i = 0; i < statsKey.Length; i++)
        {
            int value = SteamUserStats.GetStatInt(statsKey[i].ToString());

            if (value < StatMaxValues[(int)statsKey[i]])
            {
                return false;
            }
        }
        
        return true;
    }

    
}
