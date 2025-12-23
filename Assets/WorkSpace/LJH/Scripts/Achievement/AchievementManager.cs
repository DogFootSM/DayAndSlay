using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;
    private SteamworksManager steamWorkInstance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        steamWorkInstance = SteamworksManager.SteamworksInstance;
    }

    /// <summary>
    /// 작업량 도전과제 달성
    /// </summary>
    /// <param name="achievement"></param>
    /// <param name="stat"></param>
    /// <param name="value"></param>
    public void TriggerAchievement(SteamAchievementAPI achievement, SteamStatAPI[] statArray, int value, SteamStatAPI stat)
    {
        if (!steamWorkInstance.CheckUserStat(statArray))
        {
            // 입력한 스탯에 값 전달
            Steamworks.SteamUserStats.AddStat(stat.ToString(), value);

            // 저장
            Steamworks.SteamUserStats.StoreStats();
        }

        if (steamWorkInstance.CheckUserStat(statArray))
        {
            if (!steamWorkInstance.CheckUnlockAchievement(achievement))
            {
                steamWorkInstance.UnlockAchievement(achievement);
            }
        }
    }

}
