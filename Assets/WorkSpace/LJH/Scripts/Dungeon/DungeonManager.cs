using System.Collections;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public DropItemPool pool;

    private bool isBossDie = false;
    private GameObject bossDoor;

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

        StartCoroutine(BossDoorFind());

    }
    
    private IEnumerator BossDoorFind()
    {
        yield return new WaitForSeconds(1f);
        
        bossDoor = GameObject.Find("Stone");
    }

    public void BossDoorOpen()
    {
        StartCoroutine(BossDoorOpenCoroutine());
    }

    private IEnumerator BossDoorOpenCoroutine()
    {
        yield return new WaitForSeconds(2f);
        
        bossDoor.SetActive(false);
    }
    
}
