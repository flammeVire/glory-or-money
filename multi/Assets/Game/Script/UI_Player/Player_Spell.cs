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

    private void Update()
    {
        InputManagement();
    }

    #region UI

    void InputManagement()
    {
        if (NetPlayer.PlayerPowerup.IsSpell1)
        {
            TimeSpell1 = UICooldown(CoolDown1, SpellFilter1, TimeSpell1);
        }
        if (NetPlayer.PlayerPowerup.IsSpell2)
        {
            TimeSpell2 = UICooldown(CoolDown2, SpellFilter2, TimeSpell2);
        }
        if (NetPlayer.PlayerPowerup.IsSpell3)
        {
            TimeSpell3 = UICooldown(CoolDown3, SpellFilter3, TimeSpell3);
        }
    }


    public float UICooldown(float CoolDown, GameObject SpellFilter,float TimeSpell)
    {

        TimeSpell = TimeSpell - Time.deltaTime;
        TimeInPercent = (100 / (CoolDown / TimeSpell))/100;
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
    #endregion
}
