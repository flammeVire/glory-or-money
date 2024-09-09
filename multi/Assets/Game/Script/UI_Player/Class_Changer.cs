using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Class_Changer : MonoBehaviour
{
    public PlayerScriptable PlayerScriptable;
    public Network_Player NetPlayer;

    private void Start()
    {
        PlayerScriptable = NetPlayer.PlayerScriptableClone;
    }


   void Class_Selector(PlayerScriptable.PossibleClass NewClass)
    {
            NetPlayer.SetCurrentClass(NewClass);
            Destroy(this.gameObject);
        
    }

    public void Warrior_choosen()
    {
        
        Class_Selector(PlayerScriptable.PossibleClass.Guerrier);
        Debug.Log("GUERRIER");
    }
    public void Archer_choosen()
    {
        
        Class_Selector(PlayerScriptable.PossibleClass.Archer);
        Debug.Log("ARCHER");
    }
    public void Rogue_choosen()
    {
        
        Class_Selector(PlayerScriptable.PossibleClass.Voleur);
        Debug.Log("VOLEUR");
    }
    public void Mage_choosen()
    {
        
        Class_Selector(PlayerScriptable.PossibleClass.Mage);
        Debug.Log("MAGE");
    }
}
