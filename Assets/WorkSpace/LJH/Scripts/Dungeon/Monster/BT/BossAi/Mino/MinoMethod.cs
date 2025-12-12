using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class MinoMethod : BossMethod
{
    private MinoAI mino;


    [SerializeField] private float buffDuration;

    public override void Skill_First()
    {
        HeadButt();
    }

    public override void Skill_Second()
    {
        Stomp();
    }

    public override void Skill_Third()
    {
        Debug.Log("방어력 버프 실행");
        StoneSkin();
    }

    public override void Skill_Fourth()
    {
        Debug.Log("궁극기 실행");
        Gigantism();
    }

    private void HeadButt()
    {
        sound.PlaySFX(SoundType.SKILL1);
        Vector3 skillPos;

        Direction direction = GetDirectionToTarget(transform.position, player.transform.position);

        switch (direction)
        {
            case Direction.Up:
                skillPos = transform.position + new Vector3(0, 1, 0);
                break;
            
            case Direction.Down:
                skillPos = transform.position + new Vector3(0, -1, 0);
                break;
            
            case Direction.Right:
                skillPos = transform.position + new Vector3(1, 0, 0);
                break;
            
            case Direction.Left:
                skillPos = transform.position + new Vector3(-1, 0, 0);
                break;
            
            default:
                skillPos = transform.position + new Vector3(0, -1, 0);
                break;
        }
        
        
        skills.SetAllEffectPos(firstSkillData, skillPos);
    }

    private void Stomp()
    {
        sound.PlaySFX(SoundType.SKILL2);
        skills.SetAllEffectPos(secondSkillData, transform.position);
    }
    

    private void Gigantism()
    {
        sound.PlaySFX(SoundType.SKILL4);
        if (mino == null)
            mino = GetComponent<MinoAI>();

        mino.SetIsMinoGiga(true);

        monsterData.Attack *= 2;
        monsterData.AttackRange += 3;
        monsterData.MoveSpeed += 3;

        StartCoroutine(GigantismScaleCoroutine());
    }
    
    private IEnumerator GigantismScaleCoroutine()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(4.5f, 4.5f, 4.5f);

        float duration = 0.25f;  // 커지는 시간
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        transform.localScale = targetScale; // 보정
    }

    private void DisGigantism()
    {
        if(mino == null) 
            mino = GetComponent<MinoAI>();

        mino.SetIsMinoGiga(false);
        
        transform.localScale = new Vector3(3, 3, 3);
        monsterData.Attack /= 2;
        monsterData.AttackRange -= 3;
        monsterData.MoveSpeed -= 3;
    }

    private void StoneSkin()
    {
        sound.PlaySFX(SoundType.SKILL3);
        model.def += 10f;
    }

    public override void DieMethod()
    {
        Debug.Log("사망");
        
        DisGigantism();
        base.DieMethod();
    }


}