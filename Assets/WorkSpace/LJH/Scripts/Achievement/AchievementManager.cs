using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;
    private SteamworksManager steamWorkInstance = SteamworksManager.SteamworksInstance;
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

    /// <summary>
    /// Y/N 도전과제 달성
    /// </summary>
    /// <param name="achievement"></param>
    public void TriggerAchievement(SteamAchievementAPI achievement)
    {
        if (steamWorkInstance.CheckUnlockAchievement(achievement))
        {
            steamWorkInstance.UnlockAchievement(achievement);
        }
    }
    
    /// <summary>
    /// 작업량 도전과제 달성
    /// </summary>
    /// <param name="achievement"></param>
    /// <param name="stat"></param>
    /// <param name="value"></param>
    public void TriggerAchievement(SteamAchievementAPI achievement, SteamStatAPI stat, int value)
    {
        SteamStatAPI[] stats = new SteamStatAPI[]{};
        
        //입력한  도전과제에 따라 배열 초기화
        if (achievement == SteamAchievementAPI._2_FIRSTSTEP)
        {
            stats = new SteamStatAPI[] { SteamStatAPI.MONSTERKILLS };
        }
        
        else if (achievement == SteamAchievementAPI._4_HUNDREDKILL)
        {
            stats = new SteamStatAPI[]{SteamStatAPI.MONSTERKILLS};
        }
        
        else if (achievement == SteamAchievementAPI._9_WEAPONMATSER)
        {
            stats = new SteamStatAPI[]{ SteamStatAPI.SWORD, SteamStatAPI.SPEAR, SteamStatAPI.BOW, SteamStatAPI.WAND };
        }

        else
        {
            Debug.LogWarning("스탯 포함된 도전과제가 아님");
        }
        
        // 입력한 스탯에 값 전달
        Steamworks.SteamUserStats.AddStat(stat.ToString(), value);
        
        // 저장
        Steamworks.SteamUserStats.StoreStats();
        
        //스텟을 만족하는지 체크 후 만족하면 도전 과제 해금
        if (steamWorkInstance.CheckUserStat(stats))
        {
            steamWorkInstance.UnlockAchievement(achievement);
        }
    }

}
