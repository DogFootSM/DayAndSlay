using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager instance;
    
    public StageNum curStage;

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



}
