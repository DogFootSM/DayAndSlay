using System.Collections;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public DropItemPool pool;

    private GameObject stone;

    private float fadeOutDelay = 0.02f; // 2초 동안 진행되어야해서 0.02초로 설정

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
    }

    void Init()
    {
        pool = GetComponent<DropItemPool>();

        pool.InitPool(12);
    }

    public void BossDoorOpen()
    {
        StartCoroutine(BossDoorOpenCoroutine());
    }

    private IEnumerator BossDoorOpenCoroutine()
    {
        SpriteRenderer stoneRenderer = stone.GetComponent<SpriteRenderer>();
        Color stoneColor = stoneRenderer.color;

        while (stoneColor.a > 0f)
        {
            stoneColor.a -= 0.01f;
            stoneRenderer.color = stoneColor;
            yield return new WaitForSeconds(fadeOutDelay);
        }
    
        // 투명도가 0이 되면 오브젝트를 비활성화합니다.
        stone.SetActive(false);
    }
    
}
