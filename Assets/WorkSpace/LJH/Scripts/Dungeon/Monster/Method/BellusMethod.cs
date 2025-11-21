using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellusMethod : BossMonsterMethod
{
    private BossMonsterAI bellus;
    [SerializeField] private BossMonsterAI malus;
    
    [SerializeField] private ParticleSystem bellusHealEffect;
    [SerializeField] private ParticleSystem malusHealEffect;
    [SerializeField] private ParticleSystem poisonWarningEffect;
    [SerializeField] private ParticleSystem poisonEffect;
    [SerializeField] private ParticleSystem seedEffect;

    [SerializeField] private List<ParticleSystem> seedEffects;
    private void Start()
    {
        base.Start();
        bellus = GetComponent<BellusAI>();
        //malus = ((BellusAI)bellus).GetPartner();
        
        bellusHealEffect.transform.position = bellus.transform.position + new Vector3(0, 1, 0);
        malusHealEffect.transform.position = malus.transform.position + new Vector3(0, 1, 0);
    }
    public override void Skill_First()
    {
        Debug.Log("µ¶ÀåÆÇ ½ÇÇà");
        poisonEffect.transform.position = player.transform.position;
        poisonEffect.Play();
    }

    public override void Skill_Second()
    {

        Debug.Log("Èú ½ÇÇà");
        bellusHealEffect.Play();
        malusHealEffect.Play();

        if (bellus.GetMonsterModel().CurHp >= bellus.GetMonsterModel().MaxHp &&
            bellus.GetMonsterModel().CurHp >= bellus.GetMonsterModel().MaxHp)
        {
            bellus.TakeDamage(-25);
            malus.TakeDamage(-25);
        }
    }

    public override void Skill_Third()
    {
        Debug.Log("¾¾¾Ñ»Ñ¸®±â");
        StartCoroutine(SeedActiveCoroutine());
    }

    private IEnumerator SeedActiveCoroutine()
    {
        foreach (ParticleSystem seed in seedEffects)
        {
            Vector3 seedPos = seed.transform.position;
            
            seed.gameObject.transform.position = new Vector3(Random.Range(seedPos.x - 6, seedPos.x + 6), 
                Random.Range(seedPos.y - 6, seedPos.y + 6),0);
            
            seed.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    /// <summary>
    /// Bellus¿¡¼± »ç¿ëÇÏÁö ¾ÊÀ½
    /// </summary>
    public override void Skill_Fourth()
    {}
    
    public override void DieMethod()
    {
        Debug.Log("»ç¸Á");
        
        if(!malus.gameObject.activeSelf)
            DungeonManager.Instance.BossDoorOpen();
        
        DropItem();
        Destroy(gameObject);
    }

}

