using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    GameObject monster;
    Rigidbody2D rb;
    
    [SerializeField] GameObject player;

    private void Start()
    {
        monster = gameObject;
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GetDirectionByAngle(player.transform.position, monster.transform.position));
            string str = GetDirectionByAngle(player.transform.position, transform.position).ToString();
            Vector2 dir = Vector2.down;
            
            if(str == "Right")
                dir = Vector2.right;
            
            if(str == "Left")
                dir = Vector2.left;
            
            if(str == "Up")
                dir = Vector2.up;
            
            if(str == "Down")
                dir = Vector2.down;
            
            
            rb.AddForce(-dir * 5f, ForceMode2D.Impulse);
        }
    }
    public Direction GetDirectionByAngle(Vector2 playerPos, Vector2 monsterPos)
    {
        // 몬스터 위치에서 플레이어 위치로 향하는 벡터
        Vector2 dir = playerPos - monsterPos;

        // 몬스터의 정면을 Vector2.right로 가정 (0도)
        float angle = Vector2.SignedAngle(Vector2.right, dir);
    
        // 각도에 따라 4방위 판별
        if (angle > -45f && angle <= 45f) 
        {
            return Direction.Right;
        }
        else if (angle > 45f && angle <= 135f) 
        {
            return Direction.Up;
        }
        else if (angle > 135f || angle <= -135f) // 180도 처리
        {
            return Direction.Left;
        }
        else // angle > -135f && angle <= -45f
        {
            return Direction.Down;
        }
    }
}
