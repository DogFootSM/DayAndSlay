using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralMonsterAI : MonoBehaviour
{
    BehaviourTree tree;

    private void Start()
    {
        BTNode attack = new AttackNode();
    }



    private void Update()
    {
        
    }
}
