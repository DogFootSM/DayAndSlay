using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamworksManager : MonoBehaviour
{
    public uint AppId;

    public static SteamworksManager SteamworksInstance;

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
        }
        catch (Exception e)
        {
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
    /// <param name="achievement"></param>
    public void UnlockAchievement(SteamAchievement achievement)
    { 
        if (connectedToSteam)
        {
            var ach = new Steamworks.Data.Achievement("TEST_ACHIEVEMENT_1_" + (int)achievement);
            ach.Trigger();
        }
    }

    /// <summary>
    /// 현재 도전 과제가 해금된 상태인지 확인
    /// </summary>
    /// <param name="achievement"></param>
    /// <returns></returns>
    public bool CheckUnlockAchievement(SteamAchievement achievement)
    {
        var ach = new Steamworks.Data.Achievement("TEST_ACHIEVEMENT_1_" + (int)achievement);

        return ach.State;
    }
    
    
    /// <summary>
    /// 도전 과제 클리어 조건 확인
    /// </summary>
    /// <param name="statsKey">통계 API 키</param>
    /// <param name="statsValue">통계 클리어 비교 값</param>
    /// <returns></returns> 
    public bool CheckUserStat(string[] statsKey, int[] statsValue)
    {
        if (statsKey.Length != statsValue.Length)
        {
            throw new Exception("Stats 체크 조건이 잘못 됨");
        }

        for (int i = 0; i < statsKey.Length; i++)
        {
            int value = SteamUserStats.GetStatInt(statsKey[i]);

            if (value < statsValue[i])
            {
                return false;
            }
        }
        
        return true;
    }
    
}
