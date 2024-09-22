using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "Ennemis_Scriptable", menuName = "ScriptableObjects/EnnemisScriptable", order = 2)]
public class EnnemisScriptable : ScriptableObject
{
    public float Life;
    public float Speed;
    public float Degat;
    public Race MonsterType;
    public float XpLoot;
    public float GoldLoot;
    public float DelayAttack;



    [SerializeField] private float OG_Life;
    [SerializeField] public float OG_Speed;
    [SerializeField] public float OG_Degat;
    [SerializeField] private float MinXp;
    [SerializeField] private float MaxXp;
    [SerializeField] private float MinOr;
    [SerializeField] private float MaxOr;
    public enum Race
    {
        Monstre1,
        Monstre2,
        Monstre3,
        Monstre4,
        Monstre5,
    }

    public void Initialize()
    {
        Debug.Log("IsReset");
        Life = OG_Life;
        Speed = OG_Speed;
        Degat = OG_Degat;
        XpLoot = RandomLoot(MinXp, MaxXp);
        GoldLoot = RandomLoot(MinOr, MaxOr);

    }

    public float RandomLoot(float min,float max)
    {
        return Random.Range(min, max);
    }
    
}