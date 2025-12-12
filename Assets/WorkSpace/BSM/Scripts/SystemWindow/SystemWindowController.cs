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

    public bool IsRecipeActive;
    
    private Stack<GameObject> canvasStack = new Stack<GameObject>();
    private SystemType currentSystemType = SystemType.SIZE;
    private Coroutine _flippingCo;
    private SoundManager soundManager => SoundManager.Instance;
    
    private int _bookFlippingAnimHash = Animator.StringToHash("BookFlipping");
    private int _bookFlippingReverseAnimHash = Animator.StringToHash("BookFlippingReverse");
    
    private void Start()
    {
        //_bookAnimator.keepAnimatorStateOnDisable = true;
    }

    private void Update()
    {
        if (!canInputKey) return;
        
        InputSystemKey();
    }

    /// <summary>
    /// 시스템 창 오픈 같은 상호작용 키 입력 가능 여부 갱신
    /// </summary>
    /// <param name="isDead">플레이어 사망 상태</param>
    public void CanInputKeyUpdate(bool isDead)
    {
        canInputKey = !isDead;
    }
    
    /// <summary>
    /// 시스템 창 오픈
    /// </summary>
    /// <param name="systemType">시스템 창 종류</param>
    public void OpenSystemWindow(SystemType systemType)
    {
        GameObject openWindow = systemWindows[systemType];

        //새로 오픈된 시스템 타입 할당
        currentSystemType = systemType;
        
        if (canvasStack.Count == 0)
        {
            soundManager.PlaySfx(SFXSound.INVENTORY_OPEN);
            canvasStack.Push(parentCanvas);
            parentCanvas.SetActive(true);
            _bookAnimator.Play(_bookFlippingAnimHash);
        }
        
        if (canvasStack.Peek().Equals(openWindow))
        {
            soundManager.PlaySfx(SFXSound.INVENTORY_CLOSE);
            AllCloseSystemWindows();
        }
        else
        {
            if (canvasStack.Count > 1)
            {
                soundManager.PlaySfx(SFXSound.INVENTORY_FLIP);
            }
            
            SwitchSystemWindows(openWindow);
        } 
    }

    /// <summary>
    /// 시스템 창 종료
    /// </summary>
    public void AllCloseSystemWindows()
    {
        while (canvasStack.Count != 0)
        {
            canvasStack.Pop().SetActive(false);
    
            if (quickSlotCanvas != null && quickSlotCanvas.activeSelf)
            {
                quickSlotCanvas.SetActive(false);
            } 
        }
        
        foreach (var tabButton in systemWindowButtons)
        {
            if (tabButton.Value.isActive)
            {
                tabButton.Value.CloseTab();
            }
        }
        
        currentSystemType = SystemType.SIZE;
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
         
        //클릭한 신규 탭 애니메이션 전환
        systemWindowButtons[currentSystemType].SwitchTab(true);
        
        foreach (var tabButton in systemWindowButtons)
        {
            //기존에 열려있던 탭 종료 및 애니메이션 재생
            if (currentSystemType != tabButton.Key && tabButton.Value.isActive)
            {
                tabButton.Value.CloseTab();
                
                if ((int)currentSystemType > (int)tabButton.Key)
                {
                    _bookAnimator.Rebind();
                    _bookAnimator.Play(_bookFlippingAnimHash);
                }
                else
                {
                    _bookAnimator.Rebind();
                    _bookAnimator.Play(_bookFlippingReverseAnimHash);
                }  
            }
        }
         
        if (_flippingCo == null)
        {
            _flippingCo = StartCoroutine(CheckFlippingAnimationRoutine(switchWindow));
        }
        
    }


    private IEnumerator CheckFlippingAnimationRoutine(GameObject switchWindow)
    {
        while (true)
        {
            yield return null;

            if (_bookAnimator.GetCurrentAnimatorStateInfo(0).IsName("BookFlipping")
                || _bookAnimator.GetCurrentAnimatorStateInfo(0).IsName("BookFlippingReverse"))
            {
                float aniTime = _bookAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (aniTime >= 1.0f)
                {
                    canvasStack.Peek().SetActive(true);
                } 
            } 
        } 
    }
     
    /// <summary>
    /// 시스템 키 입력 감지
    /// </summary>
    private void InputSystemKey()
    {
        if (IsRecipeActive) return;
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenSystemWindow(SystemType.SKILL);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            OpenSystemWindow(SystemType.STATUS);
        }
        else if (Input.GetKeyDown(KeyCode.P))
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