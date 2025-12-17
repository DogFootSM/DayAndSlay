using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WantItemManager : MonoBehaviour
{
    [SerializeField] private GameObject _wantItemCanvas;
    private WantItemPool _wantItemPool => WantItemPool.Instance;
    
    [SerializeField] private List<GameObject> activedWantItems = new List<GameObject>();

    

    /// <summary>
    /// 원트아이템 꺼내오는 메서드
    /// NPC가 상점 입장시 호출
    /// </summary>
    /// <param name="damage"></param>
    public void ActiveWantItem(Npc npc)
    {
        //풀에서 텍스트 오브젝트를 꺼내옴
        GameObject instance;
        activedWantItems.Add(instance = _wantItemPool.GetWantItemInPool());
        
        //부모 설정
        instance.transform.SetParent(_wantItemCanvas.transform);
        instance.transform.localPosition = Vector3.zero;
        
        instance.GetComponent<WantItem>().SetItemName(npc.wantItem.Name);
        
        instance.SetActive(true);
    }

    /// <summary>
    /// 원트아이템 돌아가게 하는 메서드
    /// NPC가 상점 나갈때 호출
    /// </summary>
    /// <param name="npc"></param>
    public void InActiveWantItem(Npc npc)
    {
        Debug.Log("원트아이템풀에서 제거");
        
        for (int i = 0; i < activedWantItems.Count; i++)
        {
            if (activedWantItems[i].GetComponentInChildren<TextMeshProUGUI>().text == npc.wantItemName)
            {
                //npc가 원하는 아이템과 동일한 아이템을 발견하면 제일 먼저들어온 아이템을 제거
                _wantItemPool.ReturnWantItemInPool(activedWantItems[i]);
                activedWantItems.RemoveAt(i);
                break;
            };
        }
    }
}
