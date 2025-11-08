using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkayButton : MonoBehaviour
{
    private Animator animator;
    private string animName = "ForgeOkayButton";

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void OnMousePointerEnter()
    {
        animator.speed = 1f;
        
        animator.Play(animName, 0, 0f);
    }

    // (선택 사항) Pointer Exit 이벤트에 연결할 메서드
    public void OnMousePointerExit()
    {
        animator.speed = 0f;
        
        animator.Play(animName, 0, 0f);
    }
}
