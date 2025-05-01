using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    
    [Inject] private DataManager dataManager;
    
    private GameObject playerInstance;
    private PlayerController playerController;
    
    
    private void Start()
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        playerController = playerInstance.GetComponent<PlayerController>();

        
        
        dataManager.LoadPresetData(playerController.PlayerSprites);
        
    }

    private void Update()
    {
        Debug.Log(dataManager == null);
    }
}
