using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PoisonCloud : NetworkBehaviour
{
    RaycastHit[] hitsPlayer;
    RaycastHit[] hitsMonster;
    public float Radius;
    public NetworkObject myself;


    [SerializeField] float Damage;
    [SerializeField] float TickRate;
    public Dictionary<GameObject, Coroutine> ActiveCoroutines = new Dictionary<GameObject, Coroutine>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NetworkObject>() != myself)
        {
            if (other.gameObject.layer == 7 && !other.gameObject.GetComponent<Network_Player>().IsAGhost)
            {
                Debug.Log("In the Poison");
                Coroutine newCoroutine = StartCoroutine(GonnaPoisoning(player: other.gameObject));
                ListManager(true, other.gameObject,newCoroutine);


            }
            if(other.gameObject.layer == 8)
            {
                Coroutine newCoroutine = StartCoroutine(GonnaPoisoning(Monster: other.gameObject));
                ListManager(true, other.gameObject,newCoroutine);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != myself)
        {
            if (other.gameObject.layer == 7 && !other.gameObject.GetComponent<Network_Player>().IsAGhost)
            {
                Debug.Log("out the Poison");
                ListManager(false, other.gameObject);


            }
            if (other.gameObject.layer == 8)
            {
                ListManager(false, other.gameObject);
            }
        }
    }


    void ListManager(bool actif, GameObject player, Coroutine coroutine = null)
    {
        if(actif)
        {
            ActiveCoroutines.Add(player, coroutine);
        }
        else
        {
            foreach(var players in ActiveCoroutines)
            {
                if(players.Key == player)
                {
                    StopCoroutine(players.Value);
                    ActiveCoroutines.Remove(players.Key);
                }
            }
        }
    }

    IEnumerator GonnaPoisoning(GameObject player = null, GameObject Monster = null)
    {
        yield return new WaitForSeconds(TickRate);
        if(player != null)
        {
            NetworkObject playerObj = player.GetComponent<NetworkObject>();
            Rpc_PoisoningPlayer(playerObj);
        }
        else if(Monster != null)
        {
            NetworkObject MonsterObj = Monster.GetComponent<NetworkObject>();
            PoisoningMonster(MonsterObj);
        }
        StartCoroutine(GonnaPoisoning(player,Monster));
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    void Rpc_PoisoningPlayer(NetworkObject player)
    {
            Network_Player netplayer = player.GetComponent<Network_Player>();
            Debug.Log("Poisoning" + netplayer.PlayerName);
            if (netplayer.PlayerScriptableClone != null)
            {
                netplayer.PlayerScriptableClone.Life -= Damage;
            }
    }
    
    void PoisoningMonster(NetworkObject Monster)
    {
        if (HasStateAuthority)
        {
            Debug.Log("Inflict Poison");
            IA_Network netIA = Monster.GetComponent<IA_Network>();
            if(netIA.NetworkedLife >= 0)
            {
                netIA.Rpc_Damage(Damage);
            }
        }
    }
    
    [Rpc(RpcSources.All,RpcTargets.All)]
    public void Rpc_SetPlayerAuthority(NetworkObject obj)
    {
        myself = obj;
        Object.RequestStateAuthority();
    }

}