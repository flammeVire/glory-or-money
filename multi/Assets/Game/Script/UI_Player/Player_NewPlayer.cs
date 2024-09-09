using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player_NewPlayer : MonoBehaviour
{
    public GameObject Toggle;

    public TextMeshProUGUI Name;


    public void PlayerNaming(string PlayerName)
    {
        Name.text = PlayerName;
    }

    public void PlayerReady(bool IsReady)
    {
        if(IsReady)
        {
            Toggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            Toggle.GetComponent<Toggle>().isOn = false;
        }
    }

}
