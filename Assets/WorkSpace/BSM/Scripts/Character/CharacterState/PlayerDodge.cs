using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDodge : PlayerState
{
    private const float MAX_TELEPORT_DISTANCE = 2.5f;
    private const float BACKDASH_JUMP_POWER = 8f;
    
    private LayerMask obstacleLayer;

    private Dictionary<Direction, int> backDashHashMap = new Dictionary<Direction, int>()
    {
        {Direction.Down, Animator.StringToHash("DownBackDash")},
        {Direction.Up, Animator.StringToHash("UpBackDash")},
        {Direction.Left, Animator.StringToHash("RightBackDash")},
        {Direction.Right, Animator.StringToHash("LeftBackDash")},
    };
    
    public PlayerDodge(PlayerController playerController) : base(playerController)
    {   
        obstacleLayer = LayerMask.GetMask("Wall");
    }

    public override void Enter()
    {
        switch (playerController.CurrentWeaponType)
        {
            case CharacterWeaponType.BOW:
                BackDash();
                break;

            case CharacterWeaponType.WAND:
                Teleport();
                break;
        }
    }

    /// <summary>
    /// 현재 바라보고 있는 반대 방향으로 백대쉬
    /// </summary>
    private void BackDash()
    { 
        //회피기 사용 불가 변경
        playerController.CanDodge = false;        
        
        Vector2 direction = playerController.LastMoveKey switch
        {
            Direction.Down => Vector2.up,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.down,
            _ => Vector2.zero
        };

        playerController.DodgeCo = playerController.StartCoroutine(DodgeCoroutine(direction, BACKDASH_JUMP_POWER));
        playerController.CurWeapon.NormalAttack();
    }

    private IEnumerator DodgeCoroutine(Vector2 direction, float power)
    {
        playerController.CharacterRb.velocity = direction * power;
        playerController.BodyAnimator.Play(backDashHashMap[playerController.LastMoveKey]);
        playerController.WeaponAnimator.Play(backDashHashMap[playerController.LastMoveKey]);
        
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            yield return null;
            elapsedTime += Time.deltaTime * 5f;
        }
        
        //회피기 쿨다운 리셋 진행
        playerController.ResetDodgeCoolDown(BuffType.BACKDASH);
        playerController.CharacterRb.velocity = Vector2.zero;
    }
 
    /// <summary>
    /// 바라보고 있는 방향으로 텔레포트
    /// </summary>
    private void Teleport()
    {
        //회피기 사용 불가 상태 변경
        playerController.CanDodge = false;
        
        Vector2 teleportPos = Vector2.zero;
        Vector2 direction = Vector2.zero;
        
        switch (playerController.LastMoveKey)
        {
            case Direction.Up:
                direction = Vector2.up;
                break;
            
            case Direction.Down:
                direction = Vector2.down;
                break;
            
            case Direction.Left:
                direction = Vector2.right;
                break;
            
            case Direction.Right:
                direction = Vector2.left;
                break;
        }
           
        RaycastHit2D hit = Physics2D.Raycast(playerController.transform.position, direction, MAX_TELEPORT_DISTANCE, obstacleLayer);
        
        //전방 장애물 여부 확인
        if (hit.collider != null)
        {
            float offset = 0.25f;
            
            //장애물 충돌 위치 + 충돌 바깥 위치 + offset 위치로 보정
            teleportPos = hit.point + hit.normal * offset; 
        }
        else
        {
            //이동 가능한 최대 위치로 이동
            teleportPos = playerController.transform.position + (Vector3)direction * MAX_TELEPORT_DISTANCE;
        }
         
        playerController.teleportParticle.Play();
    
        //회피기 쿨다운 진행
        playerController.ResetDodgeCoolDown(BuffType.TELEPORT);
        playerController.transform.position = teleportPos;
    }
}