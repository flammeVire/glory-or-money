using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IA_Network : NetworkBehaviour
{
    public EnnemisScriptable ScriptableRef;
    public EnnemisScriptable EnnemisScriptableClone;
    public IA_Attaque AttaqueScript;
    public IA_Detection DetectionScript;
    [Networked] public float NetworkedLife { get; set; }
    [Networked] public float Gold {  get; set; }
    [Networked] public float Xp {  get; set; }
    bool IsDead;
    [Networked] public NetworkDictionary<int, Network_Player> PlayerGonnaHaveLoot => default;
    public int NumberOfPlayers = 0;
    public Network_Player LastPlayer;
    public float Speed;

    public override void Spawned()
    {
        Rpc_AddTransform();
        ScriptableRef.Initialize();
        EnnemisScriptableClone = Instantiate(ScriptableRef);
        NetworkedLife = EnnemisScriptableClone.Life;
        Gold = EnnemisScriptableClone.GoldLoot;
        Xp = EnnemisScriptableClone.XpLoot;
        Speed = EnnemisScriptableClone.Speed;

    }
    
    public void Update()
    {
        Debug.Log("MonsterLife :" + NetworkedLife);
        Debug.Log("MonsterDgt = " + AttaqueScript.Damage);
    }

    #region manage player in mob

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_AddPlayer(Network_Player Player)
    {
        Debug.Log("Appel de la fonciton : AddPlayer (pour le joueur :" + Runner.LocalPlayer );
        bool AlreadyIn = false;

        if (PlayerGonnaHaveLoot.Count >= 0)
        {
            Debug.Log("searching for Player");
            foreach (var players in PlayerGonnaHaveLoot)
            {
                if(players.Value == Player)
                {
                    AlreadyIn = true;
                    break;
                }
            }
        }
        Debug.Log("Already In ==" +AlreadyIn);
        
        if(!AlreadyIn)
        {
            Debug.Log("Ajout d'un joueur" + Player.PlayerName);
            PlayerGonnaHaveLoot.Add(NumberOfPlayers +1,Player);
            NumberOfPlayers += 1;
        }

        if(Player.CurrentGold < 5)
        {
            LastPlayer = Player;
        }
        
        Debug.Log("TotalPlayer in dictonnary == " + PlayerGonnaHaveLoot.Count);
    }

    bool CanPlayerHaveLoot(Network_Player player)
    {
        if (player.IsAGhost == true)
        {
            Debug.Log("playerIsAGhost" + player.IsAGhost);
            return false;
        }
        else if (Vector3.Distance(player.transform.position, transform.position) > 15)
        {
            Debug.Log("Distance is not correct" + Vector3.Distance(player.transform.position, transform.position));
            return false;
        }
        else
        {
            return true;
        }
    }

    void PlayerLootManagement()
    {
        foreach (var player in PlayerGonnaHaveLoot)
        {
            if (CanPlayerHaveLoot(player.Value))
            {
                Debug.Log("PlayerCanHaveLoot" + player.Value.PlayerName);
            }
            else 
            { 
               Debug.Log("Player CAN NOT haveloot" + player.Value.PlayerName);
               PlayerGonnaHaveLoot.Remove(player.Key);
            }
        }

    }

    #endregion 

    #region Loot
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_Looting()
    {
        Debug.Log("general looting");
        Debug.Log("HowManyPlayerInList at dead" + PlayerGonnaHaveLoot.Count);

        
        if (PlayerGonnaHaveLoot.Count > 0)
        {
            PlayerLootManagement();
            DistributeGold();
            DistributeXP();
        }
        

    }

    void DistributeGold()
    {
        Debug.Log("Distriblition OR");

        float rest = Gold % PlayerGonnaHaveLoot.Count;
        int numberOfPlayerMaxGold = 0;
        foreach (var players in PlayerGonnaHaveLoot)        // combiens de joueurs on Max Gold 
        {
            if (players.Value.CurrentGold >= 5)
            {
                numberOfPlayerMaxGold++;
            }
        }

        // si assez d'argent pour tout le monde
        if (Gold >= PlayerGonnaHaveLoot.Count - numberOfPlayerMaxGold) 
        {
            Debug.Log("Assez d'argent : Distribution");
            
            Debug.Log("number of max gold = " + numberOfPlayerMaxGold);

            float quotient = Gold / (PlayerGonnaHaveLoot.Count - numberOfPlayerMaxGold);
            foreach (var players in PlayerGonnaHaveLoot)
            {
                if (players.Value.CurrentGold < 5)
                {
                    players.Value.CurrentGold += (int)Mathf.Floor(quotient);
                    players.Value.SetMaxGold();
                }
            }

            if (rest >= PlayerGonnaHaveLoot.Count - numberOfPlayerMaxGold)
            {
                foreach (var players in PlayerGonnaHaveLoot)
                {
                    if (players.Value.CurrentGold < 5)
                    {
                        players.Value.CurrentGold += (int)Mathf.Floor(rest);
                        players.Value.SetMaxGold();
                    }
                }
            }
            else if (rest < PlayerGonnaHaveLoot.Count - numberOfPlayerMaxGold)
            {
                LastPlayer.CurrentGold += (int)Mathf.Floor(rest);
                LastPlayer.SetMaxGold();
            }
        }
        
        // si pas assez d'argent pour tout le monde
        else if(Gold <= PlayerGonnaHaveLoot.Count - numberOfPlayerMaxGold)
        {
            Debug.Log("arrondis =" + Mathf.RoundToInt(Gold));
            //si Or arrondis == assez pour tout le monde
            if(Mathf.RoundToInt(Gold) >= PlayerGonnaHaveLoot.Count)     
            {
                
                float quotient = Mathf.RoundToInt(Gold) / (PlayerGonnaHaveLoot.Count - numberOfPlayerMaxGold);
                foreach (var player in PlayerGonnaHaveLoot)
                {
                    if (player.Value.CurrentGold < 5)
                    {
                        player.Value.CurrentGold += (int)Mathf.Floor(quotient);
                        player.Value.SetMaxGold();
                    }
                }
            }
            
            // sinon donne tout au dernier joueur 
            else
            {
               
                LastPlayer.CurrentGold += (int)Mathf.Floor(Gold);
                LastPlayer.SetMaxGold();
            }
        }

        
      

    }

    void DistributeXP()
    {
        Debug.Log("Distrubution Xp");
        float rest = Xp / PlayerGonnaHaveLoot.Count;
        foreach(var player in PlayerGonnaHaveLoot)
        {
            player.Value.CurrentXP += rest;
            player.Value.MonterDeNiveau();
            player.Value.NetGame.Rpc_BestPlayerScore();
        }
    }

   
    #endregion

    #region gestion de vie
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_Damage(float damage)
    {
        if (Object.HasStateAuthority)
        {
            NetworkedLife -= damage;

            if (NetworkedLife <= 0)
            {
                Rpc_Looting();

                StartCoroutine(DestroyAfterDelay());
            }
        }
    }

    
    

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Runner.Despawn(Object); 
    }
    

    #endregion


    [Rpc(RpcSources.All, RpcTargets.All)]
    void Rpc_AddTransform()
    {
        if(Object.GetComponent<NetworkTransform>() == null)
        {
            Debug.Log("TRSP add for player" + Runner.LocalPlayer);
            Object.AddComponent<NetworkTransform>();
        }
    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    public void Rpc_SpeedModifier(float newSpeed, float delay)
    {
        Speed = newSpeed;
        StartCoroutine(SpeedReset(delay));
    }

    IEnumerator SpeedReset(float Delay)
    {
        yield return new WaitForSeconds(3);
        if(EnnemisScriptableClone!=null)
        {
            Speed = EnnemisScriptableClone.Speed;
        }
    }

}
