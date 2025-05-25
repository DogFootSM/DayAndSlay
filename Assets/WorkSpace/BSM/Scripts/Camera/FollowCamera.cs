using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Random = UnityEngine.Random;

public class FollowCamera : MonoBehaviour
{
    [Inject] private MapManager mapManager;

    private Vector3 characterPosition;
    private Camera mainCam;
    private Vector3 camOffset = new Vector3(0f, 0f, -10f);
    
    private List<Vector2> mapBoundaries;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    
    private void Awake()
    {
        mainCam = Camera.main;
        mainCam.orthographicSize = 3f;
    }

    private void Start()
    {
        ProjectContext.Instance.Container.Inject(this);
        mapManager.ManChange(MapType.TOWN);
        OnMapChange();
    }

    private void Update()
    {
        characterPosition = transform.position;
    }

    private void LateUpdate()
    {
        FollowCharacter();
    }

    private void OnMapChange()
    {
        mapBoundaries = mapManager.GetMapBoundary();
        minX = mapBoundaries[0].x;
        minY = mapBoundaries[0].y;
        maxX = mapBoundaries[1].x;
        maxY = mapBoundaries[1].y; 
    }
    
    
    private void FollowCharacter()
    {
        mainCam.transform.position =
            Vector3.Lerp(mainCam.transform.position, characterPosition + camOffset, Time.deltaTime * 5f);
        
        float clmapX = Mathf.Clamp(mainCam.transform.position.x, minX, maxX);
        float clampY = Mathf.Clamp(mainCam.transform.position.y, minY, maxY);
        
        mainCam.transform.position = new Vector3(clmapX, clampY, mainCam.transform.position.z); 
    }
}