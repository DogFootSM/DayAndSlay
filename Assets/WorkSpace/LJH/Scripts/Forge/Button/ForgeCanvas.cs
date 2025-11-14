using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeCanvas : MonoBehaviour
{
    [SerializeField] private Parts_kr curParts;

    public void SetCurParts(Parts_kr parts)
    {
        curParts = parts;
    }
    
    public Parts_kr GetCurParts() => curParts;
}
