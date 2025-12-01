using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
    public static IngameManager instance;

    private int currentDay = 1;
    
    [SerializeField] private Button upkeepButton;
    [SerializeField] private GameObject upkeepUI;
    [SerializeField] private GameObject taxUI;


    [SerializeField] private TextMeshProUGUI dayText;
    
    //현재 며칠차인지
    public int GetCurrentDay() => currentDay;

    public void AddDay()
    {
        currentDay++;
        dayText.text = currentDay.ToString();
    }

    public bool IsTaxDay() => currentDay % 5 == 0;
    public void OnTaxUI() => taxUI.SetActive(true);
    
    
    public StageNum curStage;

    /// <summary>
    /// 테스트용 1억 골드인 상태로 시작
    /// </summary>
    [SerializeField] private int gold;
    public int GetCurrentGold() => gold;
    public void SetGold(int gold) => this.gold += gold;
    

    private int money;

    /// <summary>
    /// 빚
    /// </summary>
    public int debt;
    
    /// <summary>
    /// 이자
    /// </summary>
    public int interest;
    
    /*
    /// <summary>
    /// 시설물 이용료(DLC)
    /// </summary>
    //public int facilityCost;
    
    /// <summary>
    /// 인건비 (DLC)
    /// </summary>
    //public int manCost;
*/
    /// <summary>
    /// 총 유지비
    /// </summary>
    public int upkeepCost;

    public void PayTax(int value)
    {
        Debug.Log("Paytax");
        Debug.Log($"이전 빚 {debt}");
        int change = value - interest;

        debt -= change;
        
        Debug.Log($"이후 빚 {debt}");
        
    }
    
    private void Awake()
    {
        SingletonInit();
    }

    private void Start()
    {
        upkeepButton.onClick.AddListener(UpKeepUIOnOff);
        dayText.text = currentDay.ToString();
        SetUpKeep();
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
    public int GetInterest() => interest = (int)(debt * 0.005f);

    public int UpKeepCostCalc() => upkeepCost = GetInterest() /*+ facilityCost + manCost*/;

    /// <summary>
    /// 유지비 차감 함수(시스템에서 호출)
    /// </summary>
    public void PayWeeklyUpKeepCost() => money -= UpKeepCostCalc();


    public void SetUpKeep()
    { 
        interest = GetInterest();    
        upkeepCost = UpKeepCostCalc();
    }
    public void SetUpKeepText(SerializedDictionary<string, GameObject> textDict)
    {
        textDict["DebtValue"].GetComponent<TextMeshProUGUI>().text = debt.ToString("N0") + "Gold";        
        textDict["InterestValue"].GetComponent<TextMeshProUGUI>().text = GetInterest().ToString("N0") + "Gold";    
        textDict["TotalValue"].GetComponent<TextMeshProUGUI>().text = UpKeepCostCalc().ToString("N0") + "Gold";  
        textDict["GoldValue"].GetComponent<TextMeshProUGUI>().text = GetCurrentGold().ToString("N0") + "Gold";
        //facilityCostText.text = facilityCost + "Gold";
        //manCostText.text = manCost + "Gold";
    }


    public void PayTaxResult(bool isPass, GameObject upkeepPopUp, GameObject alertPopUp)
    {
        if (!isPass)
        {
            alertPopUp.SetActive(true);
        }

        else
        {
            upkeepPopUp.SetActive(false);
        }
    }



}
