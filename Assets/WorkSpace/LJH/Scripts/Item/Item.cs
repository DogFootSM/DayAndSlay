using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;
    private SpriteRenderer sprite;

    public int ItemId;
    
    
    
    [Tooltip("전체 왕복에 걸리는 시간 (초)")]
    [SerializeField] private float floatSpeed = 2; 

    [Tooltip("중앙 위치를 기준으로 위아래로 움직이는 최대 거리")]
    [SerializeField] private float floatHeight = 0;
    private Vector3 startPos;

    public void SetItem(ItemData itemData)
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        
        sprite.sprite = ItemDatabaseManager.instance.GetItemByID(itemData.ItemId).ItemImage;
        gameObject.name = itemData.Name;
        this.itemData = itemData;
        ItemId = itemData.ItemId;
    }

    private IEnumerator FloatRoutine()
    {
        // 시간을 측정하는 변수
        float timer = 0f; 

        while (true)
        {
            timer = Time.time; 

            float yOffset = Mathf.Sin(timer * floatSpeed) * floatHeight;

            transform.position = startPos + new Vector3(0, yOffset, 0);

            yield return null;
        }
    }
    
    private void Start()
    {
        itemData = ItemDatabaseManager.instance.GetItemByID(ItemId);
        sprite = gameObject.GetComponent<SpriteRenderer>();
        
        startPos = transform.position;

        // 2. 둥실거리는 코루틴을 시작합니다.
        StartCoroutine(FloatRoutine());
    }

}
