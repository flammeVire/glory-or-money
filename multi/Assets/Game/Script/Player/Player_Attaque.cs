using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Player_Attaque : NetworkBehaviour
{
    public GameObject Arme;
    public Network_Player NetPlayer;
    public GameObject Target = null;
    public Collider Collider_Arme;
    public NetworkObject Projectile;
    float Delay;
    float Degat;

    


    private void Start()
    {
        if (!HasInputAuthority) {  return; }
        NetPlayer = GetComponentInParent<Network_Player>();
        StartCoroutine(Attaking());
        

        Degat = NetPlayer.PlayerScriptableClone.Degat;
        Delay = NetPlayer.PlayerScriptableClone.DelayWeapon;
    }

    IEnumerator Attaking() 
    {
        yield return new WaitUntil(() => NetPlayer.InputAttack() == true);
       // Debug.Log("IsATTAKING");
        switch (NetPlayer.CurrentClass) 
        {
            case PlayerScriptable.PossibleClass.None: // si classe == None, impossible d'avoir d'arme
                Destroy(this.gameObject); 
                break;

            case PlayerScriptable.PossibleClass.Guerrier:
                MeleWeapon();
                break;
            case PlayerScriptable.PossibleClass.Archer:
                RangeWeapon();
                break;
            case PlayerScriptable.PossibleClass.Voleur:
                MeleWeapon();
                break;
            case PlayerScriptable.PossibleClass.Mage:
                RangeWeapon();
                break;
            default: break;
        }
        yield return new WaitForSeconds(Delay);
       // Debug.Log("Peut attaquer");
        StartCoroutine(Attaking());
    }

    void MeleWeapon()
    {
        
        if(Target != null)
        {
            Target.GetComponent<IA_Network>().NetworkedLife -= Degat;
        }
      //  Debug.Log(Target);
    }

    void RangeWeapon()
    {        
        NetworkObject Proj = Runner.Spawn(Projectile,transform.position,transform.rotation,Runner.LocalPlayer);
        Proj.GetComponent<NetworkProjectile>().NetPlayer = NetPlayer;
    }

   


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Monster")
        {
            Target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Monster")
        {
            Target = null;
        }
    }







    /*
        si joueur = classe mélé
            si Fire1 -> attaque càc
        si joueur = classe distance
            si Fire1 -> spawn projectile
     */

}
