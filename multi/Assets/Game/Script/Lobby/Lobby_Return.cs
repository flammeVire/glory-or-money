using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Return : MonoBehaviour
{
    public bool hasPressedStart = false;

    public void ReturnToTheLobby()
    {
        if (!hasPressedStart)
        {
            Lobby_Manager.ReturnToLobby();
        }
    }
    public void IsReady()
    {
        if (!hasPressedStart)
        {
            hasPressedStart = true;
        }
        Debug.Log("CanLeave" + hasPressedStart);

    }
}
