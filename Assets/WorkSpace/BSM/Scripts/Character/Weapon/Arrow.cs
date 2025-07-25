using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D arrowRb;
    
    private Dictionary<Vector2, Quaternion> arrowRotation = new Dictionary<Vector2, Quaternion>()
    {
        { Vector2.left, Quaternion.Euler(0, 0, 0) },
        { Vector2.right, Quaternion.Euler(0, 0, 180) },
        { Vector2.up, Quaternion.Euler(0, 0, 270) },
        { Vector2.down, Quaternion.Euler(0, 0, 90) }
    };
    
    private ArrowPool arrowPool => ArrowPool.Instance;
    private Coroutine returnCo;
    
    private LayerMask monsterLayer; 
    private Vector2 maximumPos;
    private Vector2 targetDir;
    private float damage;
    private float distanceX;
    private float distanceY;

    private void Awake()
    {
        monsterLayer = LayerMask.GetMask("Monster");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & monsterLayer) != 0)
        {
            Monster monster = other.gameObject.GetComponent<Monster>();
            monster.TakeDamage(damage);
            arrowPool.ReturnPoolArrow(this.gameObject);
            
            if (returnCo != null)
            {
                StopCoroutine(returnCo);
                returnCo = null;
            }
        }
    }
 
    /// <summary>
    /// 화살이 날라갈 방향 및 회전 설정
    /// </summary>
    /// <param name="pos">화살이 생성될 위치</param>
    /// <param name="dir">발사 방향에 따른 화살 회전</param>
    public void SetLaunchTransform(Vector2 pos, Vector2 dir, float weaponRange)
    {
        transform.position = pos;
        
        //공격 방향에 따른 화살의 rotation z값 수정
        transform.rotation = arrowRotation[dir];

        arrowRb.AddForce(dir * 10f, ForceMode2D.Impulse);

        if (returnCo != null)
        {
            StopCoroutine(returnCo);
            returnCo = null;
        }
        
        //화살이 날라갈 최대 공격 사거리 설정
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            distanceX = dir.x < 0 ? -weaponRange : weaponRange; 
        }
        else
        {
            distanceY = dir.y < 0 ? -weaponRange : weaponRange;
        }

        maximumPos = new Vector2(pos.x + distanceX, pos.y + distanceY);
        returnCo = StartCoroutine(ArrowReturnRoutine(maximumPos));
    }

    /// <summary>
    /// 화살이 공격 가능 최대 사거리 도달 시 풀에 반환하는 코루틴 
    /// </summary>
    /// <param name="maximumPos">방향에 따른 화살이 날라갈 최대 거리</param>
    /// <returns></returns>
    private IEnumerator ArrowReturnRoutine(Vector2 maximumPos)
    {
        while (Vector2.Distance(transform.position, maximumPos) >= 0.001f)
        {
            yield return null;
        }
        
        arrowPool.ReturnPoolArrow(this.gameObject);
    }
    
    /// <summary>
    /// 화살의 데미지 설정
    /// </summary>
    /// <param name="damage">캐릭터의 레벨 및 스탯에 따른 데미지를 화살에 적용</param>
    public void SetArrowDamage(float damage)
    {
        this.damage = damage;
    }
}