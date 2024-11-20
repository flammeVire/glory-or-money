using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Explodingbarrel : NetworkBehaviour
{
    public RaycastHit[] hitsPlayer {  get; set; }
    public RaycastHit[] hitsMonster { get; set; }
    public float Radius;
    public LayerMask PlayerLayer;
    public LayerMask MonsterLayer;
    [SerializeField] float Damage;
    public float delayBlow;
    public Dictionary<Network_Player,Coroutine> ActiveCoroutines = new Dictionary<Network_Player,Coroutine>();

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player_Attaque>())
        {
            Debug.Log("Touching barrel");
            Debug.Log("PlayerAttackScript : " + other.gameObject.GetComponent<Player_Attaque>());
            Debug.Log("PlayerAttackScript.Netplayer : " + other.gameObject.GetComponentInParent<Network_Player>());
            Coroutine newCoroutine = StartCoroutine(WaitToBlow(other.gameObject.GetComponentInParent<Network_Player>()));
            ListManager(true, other.gameObject.GetComponentInParent<Network_Player>(), newCoroutine);


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player_Attaque>())
        {
            Debug.Log("Not Touching barrel");
            ListManager(false, other.gameObject.GetComponentInParent<Network_Player>());
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void Rpc_gonnaBlow()
    {
        Debug.Log("Blow");
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, transform.forward);

        hitsPlayer = Physics.SphereCastAll(ray, Radius, 1, PlayerLayer, QueryTriggerInteraction.Collide);
        hitsMonster = Physics.SphereCastAll(ray, Radius, 1, MonsterLayer, QueryTriggerInteraction.Collide);

        
        Rpc_Blow();

        
        Runner.Despawn(Object);

    }
    [Rpc(RpcSources.All,RpcTargets.All)]
    void Rpc_Blow()
    {
        Debug.Log("nb of player == " + hitsPlayer.Length);
        foreach (RaycastHit hit in hitsPlayer)
        {
            
            GameObject hitObject = hit.collider.gameObject;

            Network_Player NetPlayer = hitObject.GetComponent<Network_Player>();
            Debug.Log("Netplayer =" + NetPlayer.PlayerName);

            if (NetPlayer.PlayerScriptableClone != null)
            {
                Debug.Log("Damaged");
                NetPlayer.PlayerScriptableClone.Life -= Damage;
            }
            Debug.Log("Touched: " + hitObject.name);
        }
        if (HasStateAuthority)
        {
            Debug.Log("Blow Has state authority");
            foreach (RaycastHit hit in hitsMonster)
            {
                Debug.Log("Inflict damage to " + hitsMonster.Length + "monster");
                GameObject hitObject = hit.collider.gameObject;
                IA_Network NetMonster = hitObject.GetComponent<IA_Network>();
                Debug.Log("Rpc_blow Monster");
                NetMonster.Rpc_Damage(Damage);
            }
        }
        
    }

    IEnumerator WaitToBlow(Network_Player netplayer)
    {
        yield return new WaitUntil(() => netplayer.IsAttaking);
        Debug.Log("ça va Pété!");
        StartCoroutine(GonnaBlow());
    }

    IEnumerator GonnaBlow()
    {
        yield return new WaitForSeconds(delayBlow);
        Rpc_gonnaBlow();
    }
    void ListManager(bool Actif, Network_Player player ,Coroutine coroutine = null)
    {
        if(Actif)
        {
            Debug.Log("Adding Coroutine");
            ActiveCoroutines.Add(player,coroutine);
        }
        else
        {
            foreach(var players in ActiveCoroutines)
            {
                if(players.Key == player)
                {
                    Debug.Log("Stop coroutine");
                    StopCoroutine(players.Value);
                    ActiveCoroutines.Remove(players.Key);
                    break;
                }
            }
        }
    }
    /*
    private void Start()
    {
        Rpc_SetStateAuthority();
    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    void Rpc_SetStateAuthority()
    {
        Object.RequestStateAuthority();
    }
    */
    //quand un joueur met son arme sur le barril : lance une coroutine : Attends que le joueur attaque
    // ajoute le joueur + la coroutine dans un dico
    // si le joueur sort arrete sa coroutine
    // si joueur attaque, attend x secondes 
    // fait exploser le baril et inflige des dégâts a tout les joueurs alentours



}

