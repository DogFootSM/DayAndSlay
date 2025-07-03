using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public string objName;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        text.text = $"E키를 눌러서 \n {objName} 사용하기";
    }
}
