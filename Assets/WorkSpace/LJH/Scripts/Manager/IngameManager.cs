using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
    [SerializeField] private Button upkeepButton;
    [SerializeField] private GameObject upkeepUI;
    [SerializeField] private GameObject taxUI;
    [SerializeField] private TextMeshProUGUI dayText;
    
    public static IngameManager instance;
    public StageNum curStage;
    
    private int gold = 0;
    private int currentDay = 1;
    
    /// <summary>
    /// 빚
    /// </summary>
    private int debt;
    
    /// <summary>
    /// 이자
    /// </summary>
    private int interest;
    
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
    
    private void Awake()
    {
        SingletonInit();
    }

    private void Start()
    {
        upkeepButton.onClick.AddListener(UpKeepUIOnOff);
        SetUpKeep();
    }
    
    /// <summary>
    /// 싱글톤 초기화
    /// </summary>
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
    
    /// <summary>
    /// 게임 저장 시
    /// 현재 날짜에서 다음 날짜로 증가
    /// </summary>
    public void AddDay()
    {
        currentDay++;
        dayText.text = currentDay.ToString();
    }
    
    /// <summary>
    /// 현재 며칠차인지
    /// </summary>
    /// <returns></returns>
    public int GetCurrentDay() => currentDay;
    
    /// <summary>
    /// DB에서 불러온 날짜 설정
    /// </summary>
    /// <param name="day">현재까지 진행한 날짜</param>
    public void SetCurrentDay(int day) => currentDay = day;
    
    public bool IsTaxDay() => currentDay % 5 == 0;
    public void OnTaxUI() => taxUI.SetActive(true);
    
    public int GetCurrentGold() => gold;
    public void SetGold(int gold) => this.gold += gold;

    public void PayTax(int value)
    {
        Debug.Log("Paytax");
        Debug.Log($"이전 빚 {debt}");
        int change = value - interest;

        debt -= change;
        
        Debug.Log($"이후 빚 {debt}");
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
    public void PayWeeklyUpKeepCost() => gold -= UpKeepCostCalc();
    
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

    
    /// <summary>
    /// DB에서 불러온 Debt 값 초기화
    /// </summary>
    /// <param name="debt"></param>
    public void SetDebt(int debt)
    {
        this.debt = debt;
    }

    /// <summary>
    /// 현재 빚 정보 반환
    /// </summary>
    /// <returns></returns>
    public int GetDebt()
    {
        return debt;
    }
    
}
