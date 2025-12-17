using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreManager : InteractableObj
{
    [SerializeField] private GameObject popUp;
    /// <summary>
    /// 현재 빚이 얼마인지
    /// </summary>
    [SerializeField] private TextMeshProUGUI debtText;

    private int debt;
    
    //0  없음
    //1 ~ 20만 빚이 조금 있음
    //20만 1 ~ 40만 빚이 있음
    //40만 1 ~ 60만 빚이 조금 많음
    //60만 1 ~ 80만 빚이 많음
    //80 ~ 빚이 매우 많음
    private string[] debtRange = new string[]
    {
        "빚이 조금 있음", "빚이 있음", "빚이 조금 많음", "빚이 많음", "빚이 매우 많음",
    };

    private void Start()
    {
        SetDebtText();
    }

    /// <summary>
    /// 빚 변경 메서드
    /// </summary>
    /// <param name="debt"></param>
    public void SetDebt(int debt)
    {
        this.debt = debt;
    }
    
    /// <summary>
    /// DB에서 불러온 빚에 따른 빚 상태 텍스트 설정
    /// </summary>
    private void SetDebtText()
    {
        if (debt == 0)
        {
            debtText.text = "빚이 없음"; 
        }
        else
        {
            int index = (debt / 200000) > 4 ? 4 : debt / 200000;
            debtText.text = debtRange[index];
        }
    }


    public override void Interaction()
    {
        if (DayManager.instance.GetDayOrNight() != DayAndNight.MORNING) return;
        {
            popUp.SetActive(false);
            DayManager.instance.OpenStore();
        }
    }
    public override void UiOnOffMethod(Collision2D collision)
    {
        if (DayManager.instance.GetDayOrNight() != DayAndNight.MORNING) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            popUp.GetComponent<PopUp>().SetText("가게 오픈하기");
            popUp.SetActive(!popUp.gameObject.activeSelf);
        }
    }

}
