using System.Collections;
using UnityEngine;
using Zenject;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public DropItemPool pool;

    private GameObject stone;

    private float fadeOutDelay = 0.02f; // 2초 동안 진행되어야해서 0.02초로 설정
    
    //카메라
    [SerializeField] private Camera doorCamera;
    [Inject] MapManager mapManager;

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
        
        
        doorCamera.transform.position = Camera.main.transform.position;
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
