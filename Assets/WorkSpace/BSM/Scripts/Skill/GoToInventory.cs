using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoToInventory : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SystemWindowController _windowController;


    public void OnPointerClick(PointerEventData eventData)
    {
        _windowController.OpenSystemWindow(SystemType.INVENTORY);
    }
}
