using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSkillTreePreview : MonoBehaviour
{
    [SerializeField] private GameObject skillTreePreview;
 
    private void OnEnable()
    {
        //스킬창 새로 오픈 시 스킬 미리보기 탭 비활성화
        skillTreePreview.SetActive(false);
    }
}
