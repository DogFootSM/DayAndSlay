using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode
{
    public SkillData skillData;
    public List<SkillNode> PrerequisiteSkillNode => prerequisiteSkillNode;
    public PlayerModel PlayerModel => playerModel;
    public PlayerSkillReceiver PlayerSkillReceiver => playerSkillReceiver;
    
    private List<SkillNode> prerequisiteSkillNode = new();
    private List<SkillNode> nextSkillNode = new();
    private PlayerModel playerModel;
    private PlayerSkillReceiver playerSkillReceiver;
    
    private int curSkillLevel = 0;
    private bool unLocked; 
    public bool UnLocked => unLocked;
    public int CurSkillLevel => curSkillLevel;

    public bool IsCoolDownReset;
    public bool IsMarkOnTarget;

    private CharacterWeaponType curWeapon;
    private LayerMask monsterLayer;
    private Collider2D targetCollider;
    private Vector2 overlapSize;
    
    public SkillNode(SkillData skillData, PlayerModel playerModel, PlayerSkillReceiver playerSkillReceiver, CharacterWeaponType weaponType)
    {
        this.skillData = skillData;
        this.playerModel = playerModel;
        this.playerSkillReceiver = playerSkillReceiver;
        curWeapon = weaponType;
        monsterLayer = LayerMask.GetMask("Monster");
    }
 
    /// <summary>
    /// 선행 조건으로 찍혀있어야 할 스킬 리스트
    /// </summary>
    /// <param name="skillNode">선행 조건의 스킬 노드</param>
    public void AddPrerequisiteSkillNode(SkillNode skillNode)
    {
        prerequisiteSkillNode.Add(skillNode); 
    }

    /// <summary>
    /// 스킬 레벨 업그레이드 적용
    /// </summary> 
    public void ApplySkillLevel()
    {
        curSkillLevel++;
        playerModel.CurSkillPoint = -1;
        
        if (!skillData.IsActive && curSkillLevel > 0)
        {
            //TODO: 모델에 패시브 스킬 적용
            PassiveSkill passiveSkill = SkillFactoryManager.GetSkillFactory(this) as PassiveSkill;

            if (passiveSkill != null)
            {
                passiveSkill.ApplyPassiveEffects(curWeapon);
            } 
        }
    }

    /// <summary>
    /// 선행 스킬 확인 후 잠금 해제
    /// </summary>
    public void TryUnlockByPrerequisites()
    { 
        foreach (SkillNode preNode in prerequisiteSkillNode)
        {
            unLocked = preNode.curSkillLevel > 0;
        }  
    }

    /// <summary>
    /// 현재 스킬의 레벨 및 해금 정보 불러오기
    /// </summary>
    /// <param name="skillLevel">저장된 스킬 레벨</param>
    /// <param name="lockState">저장된 해금 상태</param>
    public void LoadSkillFromDB(int skillLevel, bool lockState)
    {
        curSkillLevel = skillLevel;
        unLocked = lockState;
        IsCoolDownReset = true;
        
        if (!skillData.IsActive && curSkillLevel > 0)
        {
            //모델에 패시브 스킬 적용
            PassiveSkill passiveSkill = SkillFactoryManager.GetSkillFactory(this) as PassiveSkill;

            if (passiveSkill != null)
            {
                passiveSkill.ApplyPassiveEffects(curWeapon);
            } 
        }
    }
    
    /// <summary>
    /// 타겟 몬스터에 표식 설정
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="playerPosition"></param>
    public void SetMarkOnTarget(Vector2 direction, Vector2 playerPosition)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            overlapSize = new Vector2(skillData.SkillRange, 1f);
        }
        else
        {
            overlapSize = new Vector2(1f, skillData.SkillRange);
        }

        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * (skillData.SkillRange / 2)), overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);
        
        if (cols.Length > 0)
        {
            targetCollider = cols[0];
            IEffectReceiver effectReceiver = cols[0].gameObject.GetComponent<IEffectReceiver>();
            effectReceiver.ReceiveMarkOnTarget();
            IsMarkOnTarget = true;
        }
        else
        {
            targetCollider = null;
            IsMarkOnTarget = false;
        }  
    }

    /// <summary>
    /// 타겟 몬스터의 콜라이더를 반환
    /// </summary>
    /// <returns></returns>
    public Collider2D GetMarkOnTarget()
    {
        return targetCollider;
    }
    
}
