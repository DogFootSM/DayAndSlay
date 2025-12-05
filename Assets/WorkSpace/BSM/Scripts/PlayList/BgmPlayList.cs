using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BgmPlayList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bgmTitleText;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button playAndPauseButton;
    [SerializeField] private Button nextButton;
    
    private Dictionary<BGMSound, string> bgmTitleDict  = new Dictionary<BGMSound, string>()
    {
        { BGMSound.TOWN_SCENE_BGM1, "Elf Waltz"},
        { BGMSound.TOWN_SCENE_BGM2, "Peaceful Sky"},
        { BGMSound.TOWN_SCENE_BGM3, "Shining"},
        { BGMSound.TOWN_SCENE_BGM4, "Endless Snow"},
        { BGMSound.TOWN_SCENE_BGM5, "Daydreaming"},
    };
    
    private SoundManager soundManager => SoundManager.Instance;
    
    private int firstBgmIndex = (int)BGMSound.TOWN_SCENE_BGM1;
    private int lastBgmIndex = (int)BGMSound.TOWN_SCENE_BGM5;
    private int currentIndex = 0;
    
    private bool isPlaying = true;
    public bool IsPlaying => isPlaying;
    
    private void Awake()
    {
        prevButton.onClick.AddListener(PrevBgmPlay);
        nextButton.onClick.AddListener(NextBgmPlay); 
        playAndPauseButton.onClick.AddListener(PauseOrPlayBgm); 
    }
    
    private void Start()
    {
        int randSeed = Random.Range(firstBgmIndex, lastBgmIndex +1);
        currentIndex = randSeed;
        soundManager.PlayBGM((BGMSound)randSeed);
        SetPlaylistText((BGMSound)currentIndex);  
    }

    /// <summary>
    /// 이전 순서의 BGM 재생
    /// </summary>
    private void PrevBgmPlay()
    {
        currentIndex = currentIndex - 1 <= (int)BGMSound.TOWN_SCENE_BGM1 -1
            ? (int)BGMSound.TOWN_SCENE_BGM5
            : currentIndex - 1;
        Debug.Log("이전");
        soundManager.PlayBGM((BGMSound)currentIndex);
        SetPlaylistText((BGMSound)currentIndex); 
    }

    /// <summary>
    /// 다음 순서의 BGM 재생
    /// </summary>
    private void NextBgmPlay()
    {
        currentIndex = currentIndex + 1 >= (int)BGMSound.TOWN_SCENE_BGM5 +1
            ? (int)BGMSound.TOWN_SCENE_BGM1
            : currentIndex + 1;
        Debug.Log("다음");
        soundManager.PlayBGM((BGMSound)currentIndex);
        SetPlaylistText((BGMSound)currentIndex);
    }

    private void SetPlaylistText(BGMSound bgmSound)
    {
        bgmTitleText.text = bgmTitleDict[bgmSound]; 
    } 
    /// <summary>
    /// BGM Pause Or Playing
    /// </summary>
    private void PauseOrPlayBgm()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            playAndPauseButton.transform.GetChild(0).gameObject.SetActive(true);
            playAndPauseButton.transform.GetChild(1).gameObject.SetActive(false); 
        }
        else
        {
            playAndPauseButton.transform.GetChild(0).gameObject.SetActive(false);
            playAndPauseButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        soundManager.PauseOrPlayBGM(isPlaying);
    }   
}
