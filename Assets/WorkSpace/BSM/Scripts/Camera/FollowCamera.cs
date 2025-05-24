using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowCamera : MonoBehaviour
{
    private Vector3 cameraPosition;
    private Camera mainCam;
    private Vector3 camOffset = new Vector3(0f, 0f, -10f);
    private Tilemap tileMap;

    private Vector2 mapSize;

    private void Awake()
    {
        mainCam = Camera.main;
        mainCam.orthographicSize = 3f;

        tileMap = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Tilemap>();
    }

    private void Update()
    {
        cameraPosition = transform.position;
    }

    private void LateUpdate()
    {
        FollowCharacter();
    }

    private void FollowCharacter()
    {
        Debug.Log(tileMap.cellBounds);

        mainCam.transform.position =
            Vector3.Lerp(mainCam.transform.position, cameraPosition + camOffset, Time.deltaTime * 5f);
    }
}