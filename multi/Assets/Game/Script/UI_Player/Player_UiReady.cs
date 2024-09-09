using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UiReady : MonoBehaviour
{
    [SerializeField] Button ReadyButton;
    public Network_Player NetPlayer;

    public void InitializeClickEvent(Network_Player player)
    {
        Debug.Log("Click event" + player);
        ReadyButton.onClick.AddListener(player.ImReady);
        player.ReadyButtonClone = this.gameObject;
    }


}
