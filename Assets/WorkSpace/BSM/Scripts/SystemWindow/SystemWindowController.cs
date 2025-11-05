using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SystemWindowController : MonoBehaviour
{
    [SerializedDictionary("WindowType", "WindowCanvas")] [SerializeField]
    private SerializedDictionary<SystemType, GameObject> systemWindows;

    [SerializeField] private GameObject parentCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject quickSlotCanvas;
    [Header("키 입력 가능 상태 여부")]
    [SerializeField] private bool canInputKey;
    [SerializeField] private Animator _bookAnimator;
    
    [SerializedDictionary("SystemType", "SystemWindowTabButton")] [SerializeField] 
    private SerializedDictionary<SystemType, SystemWindowButton> systemWindowButtons;
    
    private Stack<GameObject> canvasStack = new Stack<GameObject>();
    private SystemType currentSystemType = SystemType.SIZE;
    
    private int _bookFlippingAnimHash = Animator.StringToHash("BookFlipping");
    private int _bookFlippingReverseAnimHash = Animator.StringToHash("BookFlippingReverse");
    private int _bookResetTriggerHash = Animator.StringToHash("BookReset");
    
    private void Update()
    {
        if (!canInputKey) return;
        
        InputSystemKey();
    }

    /// <summary>
    /// 시스템 창 오픈
    /// </summary>
    /// <param name="systemType">시스템 창 종류</param>
    public void OpenSystemWindow(SystemType systemType)
    {
        GameObject openWindow = systemWindows[systemType];
        
        //클릭한 신규 탭 애니메이션 전환
        systemWindowButtons[systemType].SwitchTab(true);

        //시스템 창 최초 오픈 상태가 아닌 상태에서 탭 변경 시 기존 탭 애니메이션 전환
        if (currentSystemType != SystemType.SIZE)
        {
            systemWindowButtons[currentSystemType].SwitchTab(false);

            if ((int)systemType > (int)currentSystemType)
            {
                _bookAnimator.Play(_bookFlippingAnimHash);
            }
            else
            {
                _bookAnimator.Play(_bookFlippingReverseAnimHash);
            } 
        }
        
        //현재 오픈된 시스템 타입 할당
        currentSystemType = systemType;
        
        if (canvasStack.Count == 0)
        {
            canvasStack.Push(parentCanvas);
            parentCanvas.SetActive(true);
        }
        
        if (canvasStack.Peek().Equals(openWindow))
        {
            AllCloseSystemWindows();
        }
        else
        {
            SwitchSystemWindows(openWindow);
        } 
    }

    /// <summary>
    /// 시스템 창 종료
    /// </summary>
    private void AllCloseSystemWindows()
    {
        while (canvasStack.Count != 0)
        {
            canvasStack.Pop().SetActive(false);
    
            if (quickSlotCanvas != null && quickSlotCanvas.activeSelf)
            {
                quickSlotCanvas.SetActive(false);
            } 
        } 
    }

    /// <summary>
    /// 시스템 창 변경
    /// </summary>
    /// <param name="switchWindow">변경할 시스템 창</param>
    private void SwitchSystemWindows(GameObject switchWindow)
    {
        if (canvasStack.Count > 1)
        {
            canvasStack.Pop().SetActive(false);
            
            if (quickSlotCanvas != null && quickSlotCanvas.activeSelf)
            {
                quickSlotCanvas.SetActive(false);
            }
        }
        
        canvasStack.Push(switchWindow);

        StartCoroutine(CheckFlipingAnimationRoutine(switchWindow));
    }


    private IEnumerator CheckFlipingAnimationRoutine(GameObject switchWindow)
    {
        while (true)
        {
            yield return null;

            if (_bookAnimator.GetCurrentAnimatorStateInfo(0).IsName("BookFlipping") ||
                _bookAnimator.GetCurrentAnimatorStateInfo(0).IsName("BookFlippingReverse"))
            {
                float animTime = _bookAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animTime >= 1.0f)
                {
                    switchWindow.SetActive(true);
                    _bookAnimator.SetTrigger(_bookResetTriggerHash);
                    break;
                } 
            } 
        }
    }
    
    
    /// <summary>
    /// 시스템 키 입력 감지
    /// </summary>
    private void InputSystemKey()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenSystemWindow(SystemType.SKILL);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            OpenSystemWindow(SystemType.STATUS);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            OpenSystemWindow(SystemType.RECIPE);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            OpenSystemWindow(SystemType.INVENTORY);
        } 
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }

    public SystemType GetSystemType()
    {
        return currentSystemType;
    }
    
}