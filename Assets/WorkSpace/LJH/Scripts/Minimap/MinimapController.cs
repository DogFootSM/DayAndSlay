using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    private float fixedZpos = -145;
    public void CamPosSet(Vector3 pos) => gameObject.transform.position = pos + new Vector3(0, 0, fixedZpos);
}
