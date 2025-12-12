using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public string objName;
    [SerializeField] private TextMeshProUGUI text;


    public void SetText(string text) => this.text.text = text;
    private void OnEnable()
    {
        if (text.text == null)
        {
            text.text = $"스페이스바를 눌러서 \n {objName} 사용하기";
        }
    }

    private void OnDisable()
    {
        text.text = null;
    }
    
    
}
