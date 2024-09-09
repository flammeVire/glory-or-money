using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Fusion;

[CreateAssetMenu(fileName = "Player_Scriptable", menuName = "ScriptableObjects/PlayerScriptable", order = 1)]
public class PlayerScriptable : ScriptableObject
{
    #region variable
    [HideInInspector] public float Life;
    [HideInInspector]  public float Speed;
    [HideInInspector] public float Degat;

    [HideInInspector] public static PossibleClass ActiveClass;
    [HideInInspector] public List<PowerUp> ActivePowerUp = new List<PowerUp>();

    [Networked] public string PlayerName { get; set; } = "";
    [Networked] public float Score { get; set; }
    public float Xp;
    public int Level;
    public int TotalGold;
    public int CurrentGold;

    public float DelayWeapon;
    public float DelaySpell1;
    public float DelaySpell2;
    public float DelaySpell3;

    [Header("None Class")]
    [SerializeField] private float OG_Life;
    [SerializeField] private float OG_Speed;
    [SerializeField] private float OG_Degat;
    [SerializeField] private float OG_DelayWeapon;
    [SerializeField] private float OG_DelaySpell1;
    [SerializeField] private float OG_DelaySpell2;
    [SerializeField] private float OG_DelaySpell3;
    
    [Header("Warrior Class")]
    [SerializeField] private float War_Life;
    [SerializeField] private float War_Speed;
    [SerializeField] private float War_Degat;
    [SerializeField] private float War_DelayWeapon;
    [SerializeField] private float War_DelaySpell1;
    [SerializeField] private float War_DelaySpell2;
    [SerializeField] private float War_DelaySpell3;
    
    [Header("Ranger Class")]
    [SerializeField] private float Range_Life;
    [SerializeField] private float Range_Speed;
    [SerializeField] private float Range_Degat;
    [SerializeField] private float Range_DelayWeapon;
    [SerializeField] private float Range_DelaySpell1;
    [SerializeField] private float Range_DelaySpell2;
    [SerializeField] private float Range_DelaySpell3;
    
    [Header("Rogue Class")]
    [SerializeField] private float Rogue_Life;
    [SerializeField] private float Rogue_Speed;
    [SerializeField] private float Rogue_Degat;
    [SerializeField] private float Rogue_DelayWeapon;
    [SerializeField] private float Rogue_DelaySpell1;
    [SerializeField] private float Rogue_DelaySpell2;
    [SerializeField] private float Rogue_DelaySpell3; 

    [Header("Mage Class")]
    [SerializeField] private float Mage_Life;
    [SerializeField] private float Mage_Speed;
    [SerializeField] private float Mage_Degat;
    [SerializeField] private float Mage_DelayWeapon;
    [SerializeField] private float Mage_DelaySpell1;
    [SerializeField] private float Mage_DelaySpell2;
    [SerializeField] private float Mage_DelaySpell3;

    #endregion


    public enum PossibleClass
    {
        None,
        Guerrier,
        Archer,
        Mage,
        Voleur
    }

    public enum PowerUp
    {
        None,
        Power1,
        Power2,
        Power3,
        Power4,
        Power5,
        Power6
    }
    public void Initialize()
    {
        
        ActiveClass = PossibleClass.None;
        ActivePowerUp.Clear();
        ActivePowerUp.Add(PowerUp.None);
        Level = 0;
        Xp = 0;
    }

    public void ChangeStat(PossibleClass Class)
    {
        switch (Class)
        {
            case PossibleClass.None:

                Life = OG_Life;
                Speed = OG_Speed;
                Degat = OG_Degat;
                DelayWeapon = OG_DelayWeapon;
                DelaySpell1 = OG_DelaySpell1;
                DelaySpell2 = OG_DelaySpell2;
                DelaySpell3 = OG_DelaySpell3;   

                break;
            case PossibleClass.Guerrier:

                Life = War_Life;
                Speed = War_Speed;
                Degat = War_Degat;
                DelayWeapon = War_DelayWeapon;
                DelaySpell1 = War_DelaySpell1;
                DelaySpell2 = War_DelaySpell2;
                DelaySpell3 = War_DelaySpell3;

                break;
            case PossibleClass.Archer:

                Life = Range_Life;
                Speed = Range_Speed;
                Degat = Range_Degat;
                DelayWeapon = Range_DelayWeapon;
                DelaySpell1 = Range_DelaySpell1;
                DelaySpell2 = Range_DelaySpell2;
                DelaySpell3 = Range_DelaySpell3;

                break;
            case PossibleClass.Mage:

                Life = Mage_Life;
                Speed = Mage_Speed;
                Degat = Mage_Degat;
                DelayWeapon = Mage_DelayWeapon;
                DelaySpell1 = Mage_DelaySpell1;
                DelaySpell2 = Mage_DelaySpell2;
                DelaySpell3 = Mage_DelaySpell3;

                break;
            case PossibleClass.Voleur:

                Life = Rogue_Life;
                Speed = Rogue_Speed;
                Degat = Rogue_Degat;
                DelayWeapon = Rogue_DelayWeapon;
                DelaySpell1 = Rogue_DelaySpell1;
                DelaySpell2 = Rogue_DelaySpell2;
                DelaySpell3 = Rogue_DelaySpell3;

                break;
            default: break;
        }
    }
}