using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private ForgeUI forgeUi;
    public ItemData itemData;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Tap_ItemButton);
    }
    public void SetButtonItem(ItemData itemData)
    {
        this.itemData = itemData;
        GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
    }

    public ItemData GetButtonItem() => itemData;

    public void Tap_ItemButton()
    {
        forgeUi.SetPreview(itemData);
    }

}
