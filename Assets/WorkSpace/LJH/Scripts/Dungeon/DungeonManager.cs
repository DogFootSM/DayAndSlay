using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class DungeonManager : MonoBehaviour
{
    //던젼에 입장 한적이 있는지
    public static bool hasDungeonEntered = false;
    
    //스테이지 클리어 여부
    public static bool is1StageCleared = false;
    public static bool is2StageCleared = false;
    public static bool is3StageCleared = false;

    public static DungeonManager Instance;
    public DropItemPool pool;

    private GameObject stone;

    private float fadeOutDelay = 0.02f; // 2초 동안 진행되어야해서 0.02초로 설정
    
    //카메라
    [SerializeField] private Camera doorCamera;
    [Inject] MapManager mapManager;
    
    [SerializeField] private Light2D light;
    [SerializeField] private Volume dungeonVolume;
    
    private SoundManager soundManager => SoundManager.Instance;
    private Bloom bloom;
    private ColorAdjustments colorAdjustments;
    
    private int _remainingBossCount;
    public int RemainingBossCount
    {
        get => _remainingBossCount;
        set
        {
            // 1. 실제 변수 값을 변경합니다.
            _remainingBossCount = value;

            // 2. 값이 변경된 후, 조건 체크를 수행합니다.
            if (_remainingBossCount <= 0) // 0 이하가 될 경우
            {
                // 3. 특정 함수를 실행합니다.
                BossDoorOpen();
                StageClearedCheck(DungeonRoomSpawner.stageNum);
            }

        }
    }

    public void SetStoneInBossDoor(GameObject stone)
    {
        this.stone = stone;
    }

    private void Awake()
    {
        Instance = this;
        
        if(!hasDungeonEntered) hasDungeonEntered = true;
    }

    private void Start()
    {
        Init();
        mapManager.MapChange(MapType.DUNGEON_0);
    }

    void Init()
    {
        pool = GetComponent<DropItemPool>();

        pool.InitPool(12);
        
        soundManager.PlayBGM(BGMSound.DENGEON_1_BGM);
        doorCamera.transform.position = Camera.main.transform.position;

        if (DungeonRoomSpawner.stageNum == 0)
        {
            light.intensity = 0.8f;
            dungeonVolume.profile.TryGet(out bloom);
            bloom.active = true;
            bloom.tint.value = new Color(1f, 0.3f, 0f);

            dungeonVolume.profile.TryGet(out colorAdjustments);
            colorAdjustments.active = true;
            colorAdjustments.contrast.overrideState = true;
            colorAdjustments.contrast.value = 17f;
            colorAdjustments.colorFilter.value = new Color(0.45f, 0.45f, 0.45f);
        }
        else if (DungeonRoomSpawner.stageNum == (StageNum)1)
        {
            light.intensity = 1f;
            
            dungeonVolume.profile.TryGet(out bloom);
            bloom.active = true;
            bloom.tint.value = new Color(1f, 1f, 1f);

            dungeonVolume.profile.TryGet(out colorAdjustments);
            colorAdjustments.active = true;
            colorAdjustments.contrast.overrideState = true;
            colorAdjustments.contrast.value = 17f;
            colorAdjustments.colorFilter.value = new Color(0.6f, 0.6f, 0.6f);
        }
        else if (DungeonRoomSpawner.stageNum == (StageNum)2)
        {
            light.intensity = 1f;
            
            dungeonVolume.profile.TryGet(out bloom); 
            bloom.active = true;
            bloom.tint.value = new Color(0f, 0.5f, 1f);

            dungeonVolume.profile.TryGet(out colorAdjustments);
            colorAdjustments.active = true;
            colorAdjustments.contrast.overrideState = true;
            colorAdjustments.contrast.value = 17f;
            colorAdjustments.colorFilter.value = new Color(1f, 1f, 1f);
        }
        
    }

    public void StageClearedCheck(StageNum stageNum)
    {
        switch (stageNum)
        {
            case StageNum.STAGE1 :
                is1StageCleared = true;
                break;
            
            case StageNum.STAGE2 :
                is2StageCleared = true;
                break;
            
            case StageNum.STAGE3 :
                is3StageCleared = true;
                break;
            
            default:
                break;
        }
    }

    public void BossDoorOpen()
    {
        StartCoroutine(BossDoorOpenCoroutine());
    }

    private IEnumerator BossDoorOpenCoroutine()
    {
        SpriteRenderer stoneRenderer = stone.GetComponent<SpriteRenderer>();
        Color stoneColor = stoneRenderer.color;

        doorCamera.transform.position = Camera.main.transform.position;

        doorCamera.depth = 0;
        while (Vector3.Distance(doorCamera.transform.position, stone.transform.position) > 0.1f)
        {
            doorCamera.transform.position = Vector3.MoveTowards(doorCamera.transform.position, stone.transform.position, Time.deltaTime * 100f);
            yield return null;
        }
    
        // 도착하면 위치를 정확히 맞춰줌
        doorCamera.transform.position = stone.transform.position + new Vector3(0f, 0f, -1f);

        StartCoroutine(ShakeCamera(2f, 0.2f));
        
        while (stoneColor.a > 0f)
        {
            stoneColor.a -= 0.01f;
            stoneRenderer.color = stoneColor;
            
            yield return new WaitForSeconds(fadeOutDelay);
        }
        //카메라 흔들림 끝나고 위치 보정
        doorCamera.transform.position = stone.transform.position/* + new Vector3(0f, 0f, -1f)*/;
    
        stone.SetActive(false);
        
        yield return new WaitForSeconds(fadeOutDelay / 2);
        doorCamera.depth = -2;
    }
    
    /// <summary>
    /// 카메라 흔들림 코루틴
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    private IEnumerator ShakeCamera(float duration, float amount)
    {
        Vector3 originalPos = doorCamera.transform.position;
        float timer = 0f;
        while (timer < duration)
        {
            doorCamera.transform.position = originalPos + (Random.insideUnitSphere * amount);
            timer += Time.deltaTime;
            yield return null;
        }
    
        doorCamera.transform.position = originalPos; // 원래 위치로 복귀
    }
    
}
