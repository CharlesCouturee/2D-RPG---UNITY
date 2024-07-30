using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Effect : ScriptableObject
{
    [TextArea]
    public string effectDescription;

    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("effect executed");
    }
}
