using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public ItemRecipe itemRecipe;

    private GameObject hider;
    private bool isOpend = false;

    private void OnEnable()
    {
        //테스트용이라 주석화, 테스트 이후 다시 온이너블에서 제어
        //HideCheck(isOpend);
    }

    private void Start()
    {
        hider = transform.GetChild(1).gameObject;

        //테스트 이후 다시 온이너블에서 제어
        HideCheck(isOpend);

    }
    

    //레시피의 isOpend에 따라 해당 레시피 가리개 온오프 설정
    void HideCheck(bool isOpend)
    {
        // 오픈됨 상태일 때 하이더가 걷혀야 하기에 !로 bool변수 입력
        hider.SetActive(!isOpend);
    }
}
