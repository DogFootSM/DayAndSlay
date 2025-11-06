using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
    [SerializeField] private Button upkeepButton;
    [SerializeField] private GameObject upkeepUI;

    [SerializeField] private TextMeshProUGUI debtText;
    [SerializeField] private TextMeshProUGUI interestText;
    [SerializeField] private TextMeshProUGUI upKeepCostText;
    [SerializeField] private TextMeshProUGUI facilityCostText;
    [SerializeField] private TextMeshProUGUI manCostText;
    
    public static IngameManager instance;
    
    public StageNum curStage;

    private float money;

    /// <summary>
    /// 빚
    /// </summary>
    public float debt;
    
    /// <summary>
    /// 이자
    /// </summary>
    public float interest;
    
    /// <summary>
    /// 시설물 이용료
    /// </summary>
    public float facilityCost;
    
    /// <summary>
    /// 인건비
    /// </summary>
    public float manCost;

    /// <summary>
    /// 총 유지비
    /// </summary>
    public float upkeepCost;
    
    private void Awake()
    {
        SingletonInit();
    }

    private void Start()
    {
        upkeepButton.onClick.AddListener(UpKeepUIOnOff);
    }

    private void UpKeepUIOnOff()
    {
        UpkeepPopUp popup = upkeepUI.GetComponent<UpkeepPopUp>();
    
        if (upkeepUI.activeSelf)
        {
            popup.PlayClose();
        }
        else
        { 
            SetUpKeepText();
            upkeepUI.SetActive(true);
            popup.PlayOpen();
        }
    }


    private void SingletonInit()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void SetStage(StageNum stage) 
    {
        curStage = stage;
    }

    public StageNum GetStage() => curStage;

    /// <summary>
    /// 이자 계산
    /// </summary>
    /// <returns></returns>
    private float GetInterest() => interest = debt * 0.01f;

    private float UpKeepCostCalc() => upkeepCost = GetInterest() + facilityCost + manCost;

    /// <summary>
    /// 유지비 차감 함수(시스템에서 호출)
    /// </summary>
    public void PayWeeklyUpKeepCost() => money -= UpKeepCostCalc();


    public void SetUpKeepText()
    {
        debtText.text = debt + "Gold";        
        interestText.text = GetInterest().ToString() + "Gold";    
        upKeepCostText.text = UpKeepCostCalc().ToString() + "Gold";  
        facilityCostText.text = facilityCost.ToString() + "Gold";
        manCostText.text = manCost.ToString() + "Gold";
    }



}
