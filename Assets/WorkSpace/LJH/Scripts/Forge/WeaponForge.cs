using UnityEngine;
using Zenject;

public class WeaponForge : InteractableObj
{
    [Inject(Id = "WeaponForge")]
    GameObject forgeUi;

    [Inject(Id = "PopUp")]
    GameObject popUp;
    
    [SerializeField] private SystemWindowController controller;
    
    public InventoryInteraction inventory;

    public override void Interaction()
    {
        forgeUi.SetActive(!forgeUi.activeSelf);
        controller.OpenSystemWindow(SystemType.WEAPON);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        popUp.GetComponent<PopUp>().objName = "무기 제작대";
        popUp.SetActive(!popUp.gameObject.activeSelf);

        if (inventory == null)
        {
            inventory = collision.gameObject.GetComponent<InventoryInteraction>();
        }
    }
}
