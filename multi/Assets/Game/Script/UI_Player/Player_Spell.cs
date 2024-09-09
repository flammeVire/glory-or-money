using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spell : MonoBehaviour
{
    public GameObject SpellPanel1;
    public GameObject SpellPanel2;
    public GameObject SpellPanel3;

    public GameObject SpellFilter1;
    public GameObject SpellFilter2;
    public GameObject SpellFilter3;

    public Network_Player NetPlayer;

    float TimeSpell1;
    float TimeSpell2;
    float TimeSpell3;

    float TimeInPercent;

    float CoolDown1;
    float CoolDown2;
    float CoolDown3;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (NetPlayer.PlayerPowerup.IsSpell1)
        {
            TimeSpell1 = UICooldown(CoolDown1,SpellFilter1,TimeSpell1);
        }
        if (NetPlayer.PlayerPowerup.IsSpell2)
        {
            TimeSpell2 = UICooldown(CoolDown2,SpellFilter2,TimeSpell2);
        }
        if (NetPlayer.PlayerPowerup.IsSpell3)
        {
            TimeSpell3 = UICooldown(CoolDown3,SpellFilter3,TimeSpell3);
        }
    }

    public float UICooldown(float CoolDown, GameObject SpellFilter,float TimeSpell)
    {
        /*
        float TimeLeft = TimeSpell - Time.deltaTime;
        float TimeInPercent = 100 / (CoolDown / TimeLeft);
        Debug.Log("time Left = " + TimeLeft);
        Debug.Log("Time in percent = " + TimeInPercent);
        float DelayInPercent = (100 - TimeInPercent) / 100;

        float DelayInPercent = 100;
        Debug.Log("delay in % = " + DelayInPercent);
        SpellFilter.transform.localScale = new Vector3(TimeInPercent, 1,1);

        TimeSpell1 = TimeLeft;

        if(DelayInPercent >= 1) 
        {
            TimeSpell1 = CoolDown;
            SpellFilter.transform.localScale = new Vector3(0, 1, 1);
        }
        */
        /*
        //CoolDown -> float qui change pas  + barre a 100%
        // TimeSpell -> float qui changera + barre a x%
        // TimeLeft -> float == au temps passé depuis la derniere fois
        // TimeLeftInPercent -> float == au temps passé en %
        // DelayInPercent -> float == temps restant de la barre en %

        //TimeSpell1 -> TimeSpell qui sortira de la boucle

        TimeSpell1 = TimeSpell;


       // SpellFilter.transform.localScale = new Vector3
        */

        //float TimeSpellInPercent;


        
        TimeSpell = TimeSpell - Time.deltaTime;


       // Debug.Log("TimeSpell = " + TimeSpell);
        
        TimeInPercent = (100 / (CoolDown / TimeSpell))/100;
        
      //  Debug.Log("TimeInPercent = " + TimeInPercent);

        SpellFilter.transform.localScale = new Vector3(TimeInPercent,1,1);

       

        if (TimeInPercent <= 0)
        {
            TimeSpell = CoolDown;
            SpellFilter.transform.localScale = new Vector3(0, 1, 1);
        } 
        return TimeSpell;
    
    
    
    
    }

    public void SetTimer()
    {
        TimeSpell1 = NetPlayer.PlayerScriptableClone.DelaySpell1;
        TimeSpell2 = NetPlayer.PlayerScriptableClone.DelaySpell2;
        TimeSpell3 = NetPlayer.PlayerScriptableClone.DelaySpell3;
        SetCoolDown();
    }
    
    void SetCoolDown()
    {
        CoolDown1 = TimeSpell1;
        CoolDown2 = TimeSpell2;
        CoolDown3 = TimeSpell3;
    }
    
}
