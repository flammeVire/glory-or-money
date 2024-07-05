using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.UI;


public class lobby_SessionListEntry : MonoBehaviour
{
    public TextMeshProUGUI roomName, PlayerCount;
    public Button JoinButton;

    public void JoinRoom()
    {
        Lobby_Manager.runnerInstance.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = roomName.text,
        }) ;
        Debug.Log(roomName.text);
    }
}
