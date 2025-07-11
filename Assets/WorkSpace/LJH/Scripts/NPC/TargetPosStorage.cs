using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosStorage : MonoBehaviour
{
    public GameObject toStoreDoor;
    public GameObject toOutsideDoor;
    public GameObject table;
    public GameObject player;

    public Vector3 ToStoreDoorPos => toStoreDoor.transform.position;
    public Vector3 ToOutsideDoorPos => toOutsideDoor.transform.position;
    public Vector3 TablePos => table.transform.position;
    public Vector3 PlayerPos => player.transform.position;

    WaitForSeconds delay = new WaitForSeconds(1f);

    private void Start()
    {
        StartCoroutine(SearchPlayer());
    }

    private IEnumerator SearchPlayer()
    {
        int roofCount = 0;
        while (player == null)
        {
            yield return delay;
            player = GameObject.FindWithTag("Player");
            roofCount++;

            if(roofCount >= 5)
            {
                break;
            }
        }
    }

    public void Set_Table(Table table)
    {
        this.table = table.gameObject;
    }
}
