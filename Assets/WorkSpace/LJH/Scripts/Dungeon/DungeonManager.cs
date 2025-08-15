using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public DropItemPool pool;



    private void Start()
    {
        Init();
    }












    void Init()
    {
        pool = GetComponent<DropItemPool>();

        pool.InitPool(12);
    }
}
