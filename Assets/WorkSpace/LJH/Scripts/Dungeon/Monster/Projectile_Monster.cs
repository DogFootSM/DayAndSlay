using UnityEngine;

public class Projectile_Monster : MonoBehaviour
{
    [SerializeField] private BossAI monster;
    [SerializeField] private MonsterSkillData skillData;

    private float attack;
    public bool isRushing = false;

    private void Start()
    {
        attack = skillData.Damage;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isRushing)
        {
            isRushing = false;
            Debug.Log("플레이어가 투사체에 맞았음");
            other.GetComponent<PlayerController>().TakeDamage(monster, attack);
        }
    }
}
