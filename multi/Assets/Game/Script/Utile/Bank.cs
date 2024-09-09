using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bank : NetworkBehaviour
{
    public Dictionary<GameObject, Network_Player> PlayerList = new Dictionary<GameObject, Network_Player>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            PlayerList.Add(other.gameObject, other.gameObject.GetComponent<Network_Player>());
            Debug.Log("le joueur " + other.gameObject + " est dans la bank");
            Debug.Log("il y a " + PlayerList.Count + "dans la banque");
           
            ManagePlayer(other.gameObject, true);
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            ManagePlayer(other.gameObject, false);
            PlayerList.Remove(other.gameObject);
            Debug.Log("le joueur " + other.gameObject + " a quitter la bank");
            Debug.Log("il y a " + PlayerList.Count + "dans la banque");
            
        }
    }
    
    void ManagePlayer(GameObject IsPlayer, bool InCollider)
    {
        foreach (var player in PlayerList)
        {
            if (player.Key == IsPlayer)
            {
                if (InCollider == true)
                {
                    StartCoroutine(DropGoldToBank(player.Value));
                    Debug.Log("Debut de la cooroutine");
                }
                else
                {
                    StopAllCoroutines();
                    foreach(var otherplayer in PlayerList) 
                    {
                        if(otherplayer.Key != IsPlayer)
                        {
                            StartCoroutine(DropGoldToBank(otherplayer.Value));
                        }
                    }
                }
            }
        }
    }

    IEnumerator DropGoldToBank(Network_Player player)
    {
        yield return new WaitUntil(() => player.InputInterracting() == true);
        player.GoldToBank();
    }
}
