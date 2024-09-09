using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using TMPro;

public class Game_Network : NetworkBehaviour
{
    #region variable
    [Networked] public int PlayerCount { get; set; }
    [Networked] public int PlayerReady { get; set; }

    [Networked] public NetworkDictionary<PlayerRef, Network_Player> PlayerInGame => default;

    bool IsStarting = false;

    #region NetObject
    [SerializeField] World_Generator World_Generator;
    [SerializeField] NetworkObject Door;
    #endregion
    #region Score
    [Networked] public float GoldScore { get; set; }
    [Networked] public float SilverScore { get; set; }
    [Networked] public float BronzeScore { get; set; }

    [Networked] public Network_Player GoldPlayer { get; set; }
    [Networked] public Network_Player SilverPlayer { get; set; }
    [Networked] public Network_Player BronzePlayer { get; set; }
    #endregion

    #region UI_Object

    public GameObject GlobalCanva;

    public GameObject ScoreBoard;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI SilverText;
    public TextMeshProUGUI BronzeText;

    public GameObject AllPlayer_Panel;
    public GameObject NewPlayerPrefab;

    #endregion
    #endregion

    private void Start()
    {
        StartCoroutine(Starting());
    }


    #region Gestion des joueurs

    [Rpc(RpcSources.All,RpcTargets.All)]
    public void Rpc_PlayerAdd(PlayerRef player, Network_Player NetPlayer)
    {
        Debug.Log("Un joueur a rejoins : " + player);
        PlayerCount++;
        PlayerInGame.Add(player, NetPlayer);
        Rpc_SyncNetPlayerToNetGame();
        
        //Rpc_ShowAllPlayerUI(player);
    }



    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_PlayerRemove(PlayerRef player, Network_Player NetPlayer)
    {
        PlayerCount--;
        PlayerInGame.Remove(player, out NetPlayer); // out -> on peut recuperer la valeur qui va être supprimé (a utilisé pour despawn de la list)

    }


    [Rpc(RpcSources.All,RpcTargets.All)]
    public void Rpc_SyncNetPlayerToNetGame()
    {
        foreach(var Player in PlayerInGame)
        {
            PlayerInGame.Set(Player.Key, Player.Value);
            if(Player.Value.NetGame == null)
            {
                Debug.Log("NetGame Sync for" +Player.Key);
                Player.Value.NetGame = this;
            }
            
        }
    }



    #endregion

    #region Gestion du Start
    

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_CanStart()
    {
        if (IsAllPlayerReady())
        {
            IsStarting = true;
        }
    }

    
    public bool IsAllPlayerReady()
    {
        bool AllPlayerReady = true;

        foreach (var PlayerRef in PlayerInGame)
        {
            Network_Player player = PlayerRef.Value;
            if (!player.IsReady)
            {
                Debug.Log(PlayerRef.Key + "est pas prêt");

                AllPlayerReady = false;
                break;
            }
            else
            {
                Debug.Log(PlayerRef.Key + "est prêt");
            }
        }
        Debug.Log(AllPlayerReady);
        return AllPlayerReady;

    }

    IEnumerator Starting()
    {
        yield return new WaitUntil(() => IsStarting);

        Debug.Log("ALL PLAYER IS READY");
        World_Generator.Generating();
        Runner.Despawn(Door);


    }
    

    #endregion

    


    #region Gestion du Score 

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_BestPlayerScore()
    {
        Network_Player AncienPlayerGold = null;
        Network_Player AncienPlayerSilver = null;

        foreach (var PlayerRef in PlayerInGame)      // pour chaque JoueurRef dans la list des joueurs
        {
            Network_Player player = PlayerRef.Value;
            if (player.PlayerScore >= GoldScore)     //si joueur + que score or
            {
                //Debug.Log(player + "Score or");
                if (AncienPlayerGold != player)
                {
                    //Debug.Log("retrograde 2 joueur");

                    WhoIsSilverPlayer(AncienPlayerGold);
                    WhoIsBronzePlayer(AncienPlayerSilver);
                }
                WhoIsGoldPlayer(player);
                AncienPlayerGold = player;
            }
            else if (player.PlayerScore >= SilverScore)  //si joueur n'a pas de score or mais + que score argent
            {
                //Debug.Log(player + "Score argent");

                if (AncienPlayerSilver != player)
                {
                   // Debug.Log("retrograde 1 joueur");
                    WhoIsBronzePlayer(AncienPlayerSilver);
                }
                WhoIsSilverPlayer(player);
                AncienPlayerSilver = player;
            }
            else if (player.PlayerScore >= BronzeScore) // si joueur n'a pas de score or/argent mais + que score bronze
            {
               // Debug.Log(player + "Score bronze");

                WhoIsBronzePlayer(player);

            }
        }
        Rpc_ChangeScoreUI();
    }


    public void WhoIsGoldPlayer(Network_Player player)
    {
        if (player == null) { return; }
        GoldScore = player.PlayerScore;
        GoldPlayer = player;
    }
    public void WhoIsSilverPlayer(Network_Player player)
    {
        if (player == null) { return; }
        SilverScore = player.PlayerScore;
        SilverPlayer = player;
    }
    public void WhoIsBronzePlayer(Network_Player player)
    {
        if (player == null) { return; }
        BronzeScore = player.PlayerScore;
        BronzePlayer = player;
    }
    #endregion

    #region Gobal UI Management
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_ChangeScoreUI()
    {
        Debug.Log("RPC Called on: " + Runner.LocalPlayer);

        if (GoldPlayer != null)
        {
            GoldText.text = GoldPlayer.PlayerName;
        }
        if (SilverPlayer != null)
        {
            SilverText.text = SilverPlayer.PlayerName;
        }
        if (BronzePlayer != null)
        {
            BronzeText.text = BronzePlayer.PlayerName;
        }
    }



    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_ShowAllPlayerUI(PlayerRef NewPlayer)
    {
        Player_PlayerInGame Script = AllPlayer_Panel.GetComponent<Player_PlayerInGame>();
        
        foreach(var player in PlayerInGame)
        {
            if (player.Key == Runner.LocalPlayer)
            {
                Debug.Log("C'est le joueur local" + Runner.LocalPlayer + "( " + player.Key +" )");

                if (NewPlayer == player.Key)
                {
                    foreach (var otherPlayer in PlayerInGame)
                    {
                        Script.InstantiateNewPlayer(otherPlayer.Value, otherPlayer.Key);
                    }
                    break;
                }
                else
                {
                    foreach (var theNewPlayer in PlayerInGame)
                    {
                        if (theNewPlayer.Key == NewPlayer)
                        {
                            Script.InstantiateNewPlayer(theNewPlayer.Value, theNewPlayer.Key);
                        }
                    }
                    break;
                }
            }
        }
        
    }


    [Rpc(RpcSources.All,RpcTargets.All)]
    public void Rpc_ReadyAllPlayerUI(PlayerRef PlayerReady, bool IsReady)
    {
        Player_PlayerInGame Script = AllPlayer_Panel.GetComponent<Player_PlayerInGame>();
        
        foreach (var player in PlayerInGame) 
        {
            if(PlayerReady == player.Key)
            {
                foreach(var ThePlayer in Script.PlayerPanel)
                {
                    if(ThePlayer.Key == PlayerReady)
                    {
                        ThePlayer.Value.GetComponent<Player_NewPlayer>().PlayerReady(IsReady);
                    }
                }
            }
        }
    
    
    }

    #endregion

}






