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
        Debug.Log("¹æ¾î·Â ¹öÇÁ ½ÇÇà");
        StoneSkin();
    }

    public override void Skill_Fourth()
    {
        Debug.Log("±Ã±Ø±â ½ÇÇà");
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
    }
    

    private void Gigantism()
    {
        sound.PlaySFX(SoundType.SKILL4);
        if (mino == null)
            mino = GetComponent<MinoAI>();

        mino.SetIsMinoGiga(true);

        model.Attack *= 2;
        model.AttackRange += 2f;
        model.MoveSpeed += 1;

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        StartCoroutine(GigantismForceCoroutine(playerRb));
        StartCoroutine(GigantismScaleCoroutine());
        
        //±Ã±Ø±â ¾µ¶§ ÈúÆÑ ÇÑ¹ø µå¶ø
        DropHealPack();
    }

    private IEnumerator GigantismForceCoroutine(Rigidbody2D rigid)
    {
        yield return new WaitForSeconds(0.5f);
        
        rigid.velocity = Vector2.zero;
    }
    
    private IEnumerator GigantismScaleCoroutine()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(4.5f, 4.5f, 4.5f);

        float duration = 0.25f;  // Ä¿Áö´Â ½Ã°£
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        transform.localScale = targetScale; // º¸Á¤
    }

    private void DisGigantism()
    {
        if (isDead) return; 
        
        if(mino == null) 
            mino = GetComponent<MinoAI>();

        mino.SetIsMinoGiga(false);
        
        transform.localScale = new Vector3(3, 3, 3);
        model.Attack /= 2;
        model.AttackRange -= 3;
        model.MoveSpeed -= 3;
    }

    private void StoneSkin()
    {
        sound.PlaySFX(SoundType.SKILL3);
        model.def += 10f;
    }

    public override void DieMethod()
    {
        DisGigantism();
        base.DieMethod();
        AchievementManager.Instance.TriggerAchievement(SteamAchievementAPI._5_MINOKILL);
    }


}