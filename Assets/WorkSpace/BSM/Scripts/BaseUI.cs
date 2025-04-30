using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{

    private Dictionary<string, GameObject> goDict;
    private Dictionary<string, Component> compDict;

    private void Awake() => Bind();

    private void Bind()
    {
        
    }
    
    

}
