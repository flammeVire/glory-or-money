using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Item_Scriptable", menuName = "ScriptableObjects/ItemScriptable", order = 3)]
public class ItemScriptable : ScriptableObject
{

    public string Name;
    public string description;

    public int Price;

    public GameObject Image;
}
