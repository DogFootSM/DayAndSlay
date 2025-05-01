using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    
    private GameObject playerInstance;
     
    private void Awake()
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
