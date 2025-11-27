using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreManager : InteractableObj
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI reputationTextObj;

    private int reputation = 0;

    public void PlusRepu(int plus) => reputationTextObj.text = $"평판 점수 : {reputation += plus}";
    public void MinusRepu(int minus) => reputationTextObj.text = $"평판 점수 : {reputation -= minus}";

    private void Start()
    {
        reputationTextObj.text = $"평판 점수 : {reputation}";
    }


    public override void Interaction()
    {
        //판매 현황을 넣던가 지우던가
    }
    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popUp.GetComponent<PopUp>().objName = "카운터";
            popUp.SetActive(!popUp.gameObject.activeSelf);
        }
    }

}
