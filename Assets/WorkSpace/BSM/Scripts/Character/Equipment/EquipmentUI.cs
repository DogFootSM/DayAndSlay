using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [SerializedDictionary("Parts Type", "Parts UI Object")] [SerializeField]
    private SerializedDictionary<Parts, Image> partsUIDict;
    
    
    


}
