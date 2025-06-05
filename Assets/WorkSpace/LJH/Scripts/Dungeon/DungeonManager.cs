using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
