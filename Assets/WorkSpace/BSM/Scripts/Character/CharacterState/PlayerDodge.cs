using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDodge : PlayerState
{
    private const float MAX_TELEPORT_DISTANCE = 2.5f;

    private LayerMask obstacleLayer;
    
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


    private void BackDash()
    { 
        Vector2 direction = playerController.LastMoveKey switch
        {
            Direction.Down => Vector2.up,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.down,
            _ => Vector2.zero
        };

        playerController.StartCoroutine(DodgeCoroutine(direction, 8f));
        playerController.CurWeapon.NormalAttack();
    }

    private IEnumerator DodgeCoroutine(Vector2 direction, float power)
    {
        playerController.CharacterRb.velocity = direction * power;

        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            yield return null;
            elapsedTime += Time.deltaTime * 5f;
        }
        
        playerController.CanDodge = false;
        playerController.ResetDodgeCoolDown();
        playerController.CharacterRb.velocity = Vector2.zero;
        playerController.ChangeState(CharacterStateType.IDLE);
    }
 
    private void Teleport()
    {
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
        
        if (hit.collider != null)
        {
            float offset = 0.25f;
 
            teleportPos = hit.point + hit.normal * offset; 
        }
        else
        {
            teleportPos = playerController.transform.position + (Vector3)direction * MAX_TELEPORT_DISTANCE;
        }
         
        playerController.teleportParticle.Play();
        playerController.CanDodge = false;
        playerController.ResetDodgeCoolDown();
        playerController.transform.position = teleportPos;
        playerController.ChangeState(CharacterStateType.IDLE);
    }
}