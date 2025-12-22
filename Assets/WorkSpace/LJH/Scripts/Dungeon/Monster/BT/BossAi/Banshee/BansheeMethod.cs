using System.Collections;
using UnityEngine;

public class BansheeMethod : BossMethod
{
    private Direction dir;
    private Vector3 moveDirection;

    private Coroutine spiritRoutine;
    private SpiritRush spirit;
    
    private GameObject spiritRushWarningzone;
    public override void Skill_First()
    {
        Scream();
    }

    public override void Skill_Second()
    {
    }

    public override void Skill_Third()
    {
        StartCoroutine(MoveSpeedBuff());
    }

    public override void Skill_Fourth()
    {
        SpiritRushWarningZoneChange();
        DropHealPack();

    }

    private void SpiritRushWarningZoneChange()
    {
        if (spiritRushWarningzone == null)
            spiritRushWarningzone = skills.GetSkillWarning(fourthSkillData)[0].gameObject;
        
        dir = GetDirectionToTarget(transform.position, player.transform.position);
        
        Vector3 offset = Vector3.zero;
        Vector3 scale = Vector3.one;

        switch (dir)
        {
            case Direction.Up:
                offset = new Vector3(0, 2, 0);
                scale  = new Vector3(2, 4, 0);
                break;

            case Direction.Down:
                offset = new Vector3(0, -2, 0);
                scale  = new Vector3(2, 4, 0);
                break;

            case Direction.Right:
                offset = new Vector3(2, 0, 0);
                scale  = new Vector3(4, 2, 0);
                break;

            case Direction.Left:
                offset = new Vector3(-2, 0, 0);
                scale  = new Vector3(4, 2, 0);
                break;
        }

        // 적용
        spiritRushWarningzone.transform.localPosition = offset;
        spiritRushWarningzone.transform.localScale    = scale;
        
        
    }
    private void Scream()
    {
        sound.PlaySFX(SoundType.SKILL1);
        skills.SetAllEffectPos(secondSkillData, transform.position);
    }


    /// <summary>
    /// 이동속도 증가 버프
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveSpeedBuff()
    {
        sound.PlaySFX(SoundType.SKILL3);
        float preMoveSpeed = model.MoveSpeed;
        model.MoveSpeed *= 1.5f;
        yield return new WaitForSeconds(thirdSkillData.Duration);
        model.MoveSpeed = preMoveSpeed;
    }


    /// <summary>
    /// 텔레포트 스킬 / 애니메이션 이벤트로 사용
    /// </summary>
    public void Teleport_Banshee()
    {
        sound.PlaySFX(SoundType.SKILL2);
        Vector3 pos = player.transform.position;

        float randomX = Random.Range(pos.x - 3, pos.x + 3);
        float randomy = Random.Range(pos.y - 3, pos.y + 3);


        Vector3 randomPos = new Vector3(randomX, randomy, 0);

        transform.position = randomPos;
    }

    /// <summary>
    /// 애니메이션 이벤트로 호출
    /// </summary>
    public void SpiritRush()
    {
        sound.PlaySFX(SoundType.SKILL4);
        StartCoroutine(SpiritRushingCoroutine());
        if (spirit == null)
            spirit = skills.GetSkillVFX(fourthSkillData)[0].GetComponent<SpiritRush>();
        
        if(dir == null)
            dir = GetDirectionToTarget(transform.position, player.transform.position);

        spirit.SetDirection(dir);
    }

    private IEnumerator SpiritRushingCoroutine()
    {
        skills.GetSkillRadius(fourthSkillData)[0].GetComponent<Projectile_Monster>().isRushing = true;
        
        yield return new WaitForSeconds(1f);
        
        skills.GetSkillRadius(fourthSkillData)[0].GetComponent<Projectile_Monster>().isRushing = false;
    }
    
    public override void DieMethod()
    {
        base.DieMethod();
        AchievementManager.Instance.TriggerAchievement(SteamAchievementAPI._8_BANSHEEKILL);
    }

}