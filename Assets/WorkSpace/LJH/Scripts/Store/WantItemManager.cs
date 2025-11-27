using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantItemManager : MonoBehaviour
{
    [SerializeField] private GameObject _wantItemCanvas;
    private WantItemPool _wantItemPool => WantItemPool.Instance;

    

    /// <summary>
    /// 히트이펙트 켜주기
    /// NPC가 상점 입장시 호출
    /// </summary>
    /// <param name="damage"></param>
    public void ActiveWantItem()
    {
        //풀에서 텍스트 오브젝트를 꺼내옴
        GameObject instance = _wantItemPool.GetWantItemInPool();
        
        //부모 설정
        instance.transform.SetParent(_wantItemCanvas.transform);
        instance.transform.localPosition = Vector3.zero;
        
        instance.SetActive(true);
    }
}
