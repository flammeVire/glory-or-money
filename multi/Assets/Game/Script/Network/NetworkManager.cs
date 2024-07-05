using Fusion;

using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : NetworkBehaviour, IPlayerJoined , IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef _player;
    [Networked, Capacity(12)] private NetworkDictionary<PlayerRef, NetworkPlayer> Players => default;

    public void PlayerJoined(PlayerRef player)
    {
        if (HasStateAuthority)
        {
            NetworkObject playerObject = Runner.Spawn(_player, Vector3.up, Quaternion.identity, player);
            Players.Add(player, playerObject.GetComponent<NetworkPlayer>());
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!HasStateAuthority)
        {
            return;
        }
        if(Players.TryGet(player, out NetworkPlayer PlayerBehavior)) 
        {
            Players.Remove(player);
            Runner.Despawn(PlayerBehavior.Object);
        }
    }
}