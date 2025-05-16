using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{

    private Dictionary<string, GameObject> goDict;
    private Dictionary<(string, System.Type), Component> compDict;

    protected void Awake() => Bind();

    private void Bind()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        
        goDict = new Dictionary<string, GameObject>(transforms.Length << 1);

        foreach (Transform tr in transforms)
        {
            goDict.TryAdd(tr.gameObject.name, tr.gameObject);
        }
        
        compDict = new Dictionary<(string, System.Type), Component>();
        
    }

    protected GameObject GetUI(string name)
    {
        goDict.TryGetValue(name, out GameObject obj);
         
        return obj;
    }


    protected T GetUI<T>(in string name) where T : Component
    {
        (string, System.Type) key = (name, typeof(T));
        
        compDict.TryGetValue(key, out Component comp);

        if (comp != null) return comp as T;

        goDict.TryGetValue(name, out GameObject go);

        if (go != null)
        {
            comp = go.GetComponent<T>();

            if (comp != null)
            {
                compDict.TryAdd(key, comp);
                return comp as T;
            } 
        }

        return null;
    }   
    

}
