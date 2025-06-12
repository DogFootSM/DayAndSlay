using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictList<T>
{
    private List<T> buttonList = new List<T>();
    private Dictionary<string, T> buttonDict = new Dictionary<string, T>();

    public void Add(string key, T value)
    {
        buttonList.Add(value);
        buttonDict[key] = value;
    }

    //리무브는 생각 좀 해봐야할듯
    public void Remove(string key, T value)
    {
        buttonDict.Remove(key);
        buttonList.Remove(value);
    }

    public T this[int index] => buttonList[index];

    public T this[string key] => buttonDict[key];

    public int Count => buttonList.Count;
}
