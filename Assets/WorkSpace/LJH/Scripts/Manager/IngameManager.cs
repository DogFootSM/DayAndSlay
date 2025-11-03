using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager instance;
    
    public StageNum curStage;

    public float debt;
    public float interest;
           
    public float upkeepCost;
    public float facilityCost;

    private void Awake()
    {
        SingletonInit();
    }

    private void SingletonInit()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void SetStage(StageNum stage) 
    {
        curStage = stage;
    }

    public StageNum GetStage() => curStage;

    private float GetInterest() => interest = debt * 0.01f;
    
    



}
