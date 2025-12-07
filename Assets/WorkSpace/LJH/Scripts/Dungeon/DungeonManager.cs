using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DungeonManager : MonoBehaviour
{
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
    
    private SoundManager soundManager => SoundManager.Instance;
    
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

            Debug.Log($"[Boss Count] 남은 보스 수: {_remainingBossCount}");
        }
    }

    public void SetStoneInBossDoor(GameObject stone)
    {
        this.stone = stone;
    }

    private void Awake()
    {
        Instance = this;
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
        
        Debug.Log(Camera.main.name);
        
        soundManager.PlayBGM(BGMSound.DENGEON_1_BGM);
        doorCamera.transform.position = Camera.main.transform.position;
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
