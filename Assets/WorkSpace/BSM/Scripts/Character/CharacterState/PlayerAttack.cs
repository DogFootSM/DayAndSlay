using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zenject;

public class PlayerAttack : PlayerState
{
    private const int MAX_HIT_COMBO = 3;
    
    private int attackHash;
    private int hitCombo;
    private float hitDuration = 0.5f;
    
    private Coroutine attackCo;
    
    public PlayerAttack(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
        //공격 콤보 초기화
        hitCombo = MAX_HIT_COMBO;
            
        attackCo = playerController.StartCoroutine(AttackExitRoutine());
    }

    public override void Exit()
    {
        if (attackCo != null)
        {
            playerController.StopCoroutine(attackCo);
            attackCo = null;
        }
    }

    /// <summary>
    /// 공격 종료 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackExitRoutine()
    {
        //1타 입력 후 흐른 시간
        float elapsedTime = 0f;
        
        //애니메이션 가져올 랜덤 인덱스
        int randomIndex = playerController.CurrentWeaponType == CharacterWeaponType.BOW ? 0 : UnityEngine.Random.Range(0, 3);
        
        //방향에 따른 애니메이션 키값
        int directionKey = 0;
        
        SetAttackHash(randomIndex);
        PlayAnimation();

        //콤보 횟수 감소
        hitCombo--;
        
        //장착 무기가 활이 아닌 상태에서만 연속 공격
        if (playerController.CurrentWeaponType != CharacterWeaponType.BOW)
        {
            directionKey = playerController.LastMoveKey switch
            {
                Direction.Down => 0,
                Direction.Up => 1,
                Direction.Right => 2,
                Direction.Left => 3,
                _ => 0
            };
        
            //1타 공격 애니메이션 사용 여부 True로 변경
            UpdateAnimationByDirection((directionKey, randomIndex), true);
            
            // 1Frame 이후 재입력 확인
            yield return null;
            
            //키 입력 가능 시간 And 3콤보 이하 상태인경우
            while (elapsedTime < hitDuration && hitCombo > 0)
            {
                elapsedTime += Time.deltaTime;
                
                if (Input.GetKeyDown(KeyCode.LeftControl))
                { 
                    // //공격 입력 시간 초기화
                    elapsedTime = 0;
                    
                    //현재 가져온 애니메이션이 사용한 애니메이션인지 체크
                    while (randHashIndexCheck[directionKey][randomIndex] && hitCombo > 0)
                    {
                        randomIndex = UnityEngine.Random.Range(0, 3);
                        yield return null;
                    } 
                    
                    //콤보 횟수 감소
                    hitCombo--;
                    
                    //미사용 애니메이션 공격 해시로 변경
                    SetAttackHash(randomIndex);
                    
                    //애니메이션 사용 여부 업데이트
                    UpdateAnimationByDirection((directionKey, randomIndex), true);
                    
                    // //공격 애니메이션 재생
                    PlayAnimation(); 
                }
  
                yield return null;
            }
        }
        
        //애니메이션 사용 여부 초기화
        for (int i = 0; i < randHashIndexCheck[directionKey].Length; i++)
        {
            UpdateAnimationByDirection((directionKey, i), false);
        }
        
        yield return WaitCache.GetWait(attackSpeed);

        playerController.ChangeState(CharacterStateType.IDLE);
    }

    /// <summary>
    /// 공격 애니메이션 실행
    /// </summary>
    private void PlayAnimation()
    {
        playerController.BodyAnimator.Rebind();
        playerController.BodyAnimator.Play(attackHash);
        playerController.WeaponAnimator.Rebind();
        playerController.WeaponAnimator.Play(attackHash);
    }

    /// <summary>
    /// 공격 애니메이션 해시 설정
    /// </summary>
    /// <param name="randomIndex"></param>
    private void SetAttackHash(int randomIndex)
    {
        attackHash = animationHashes[playerController.LastMoveKey][randomIndex];
    }
    
    /// <summary>
    /// 방향에 따른 애니메이션 사용 여부 갱신
    /// </summary>
    /// <param name="key">현재 바라보고 있는 방향, 애니메이션 순서</param>
    /// <param name="value">사용 여부</param>
    private void UpdateAnimationByDirection((int,int) key, bool value)
    {
        randHashIndexCheck[key.Item1][key.Item2] = value;
    }
    
}