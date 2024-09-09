using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player_PlayerInGame : MonoBehaviour
{
    [SerializeField] GameObject NewPlayerUI;
    GameObject NewPlayerClone;
    public GameObject ParentPanel;
    Vector2 SpawnUI;
    float PrefabHeight;

    public Dictionary<PlayerRef,GameObject> PlayerPanel = new Dictionary<PlayerRef,GameObject>();


    private void Start()
    {
        SpawnUI = new Vector2(ParentPanel.transform.position.x, ParentPanel.transform.position.y);
    }

    /*
    public bool IsPlayerAlreadyInstantiate(PlayerRef player)
    {
        if (PlayerPanelRef.Count == 0)
        {
            Debug.Log("Dico vide");
            return false; 
        }
        bool isInstantiate = false;
        foreach(var playerRef in PlayerPanelRef) 
        {
            if(playerRef.Key == player)
            {
                isInstantiate = true;
                break;
            }
        }
        return isInstantiate;
    }
    */




    public void InstantiateNewPlayer(Network_Player Netplayer,PlayerRef player)
    {
        Debug.Log(" Appelé 1 fois " + player);
        NewPlayerClone = Instantiate(NewPlayerUI, SpawnUI,Quaternion.identity, ParentPanel.transform);
        NewPlayerClone.GetComponent<Player_NewPlayer>().PlayerNaming(Netplayer.PlayerName);
        PlayerPanel.Add(player, NewPlayerClone);
        PrefabHeight += 70;
        SpawnUI = new Vector2(ParentPanel.transform.position.x, ParentPanel.transform.position.y - PrefabHeight);
    }

    
    
}
