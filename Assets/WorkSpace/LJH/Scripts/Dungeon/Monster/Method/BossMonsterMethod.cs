using UnityEngine;
using Zenject;

public class BossMonsterMethod : MonoBehaviour
{
    [Inject]
    private DungeonManager dungeonManager;

    [SerializeField]
    private MonsterData monsterData;

    [SerializeField]
    private BoxCollider2D rangeCollider;

    public void MonsterDataInit(MonsterData data)
    {
        monsterData = data;
        if (monsterData == null)
        {
            Debug.LogWarning("MonsterData is null");
        }
    }

    public void Move()
    {
        // Implement movement logic if needed
    }

    public void Attack()
    {
        // Implement damage logic to player
        Debug.Log("Boss attacks");
    }

    public void BeforeAttack()
    {
        if (rangeCollider != null)
        {
            rangeCollider.enabled = true;
        }
    }

    public void AfterAttack()
    {
        if (rangeCollider != null)
        {
            rangeCollider.enabled = false;
        }
    }

    public void Die()
    {
        DropItem();
        Debug.Log("Boss died");
    }

    private void DropItem()
    {
        // Implement drop logic
    }
}