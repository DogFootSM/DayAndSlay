using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SpiritRush : MonoBehaviour
{
    [SerializedDictionary][SerializeField] private SerializedDictionary<Direction, List<Sprite>> dirSpriteDict;
    
    [SerializeField] private GameObject colliderPrefab;

    public float moveSpeed = 5f;
    private Vector3 moveDirection;
    private bool isRushing = false;
    private ParticleSystem particleSystem;
    private Vector3 startPos;
    private void RushStart()
    {
        // Rush 시작할 때 현재 위치를 기준으로
        transform.position = transform.parent.position;

        // 시작 위치 기록
        startPos = transform.position;

        isRushing = true;
    }
    public void RushEnd()
    {
        isRushing = false;
    }

    private void Update()
    {
        if (!isRushing) return;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        
        colliderPrefab.transform.position = transform.position;

        float distance = Vector3.Distance(startPos, transform.position);
        if (distance >= 7f)
        {
            RushEnd();
        }
        
    }

    public void SetDirection(Direction dir)
    {
        if(particleSystem == null) 
            particleSystem = GetComponent<ParticleSystem>();
        

        for (int i = 0; i < dirSpriteDict[dir].Count; i++)
        {
            particleSystem.textureSheetAnimation.SetSprite(i, dirSpriteDict[dir][i]);
        }

        switch (dir)
        {
            case Direction.Up:    moveDirection = Vector3.up; break;
            case Direction.Down:  moveDirection = Vector3.down; break;
            case Direction.Left:  moveDirection = Vector3.left; break;
            case Direction.Right: moveDirection = Vector3.right; break;
        }
        
        RushStart();
    }

}