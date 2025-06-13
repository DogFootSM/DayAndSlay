using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public string objName;
    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"E키를 눌러서 \n {objName}열기";
    }
}
