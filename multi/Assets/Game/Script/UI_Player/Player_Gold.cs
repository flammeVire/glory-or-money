using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Gold : MonoBehaviour
{
    public GameObject[] GoldUI;

    public TextMeshProUGUI GoldTotal;
    public Network_Player NetPlayer;

    public void SetGoldToZero()
    {
        foreach (GameObject gold in GoldUI) 
        {
            gold.SetActive(false);
        }
    }

    public void SetTotalGold()
    {
        GoldTotal.text = NetPlayer.TotalGold.ToString();
    }

    public void AddGold()
    {
        for (int i = 0; i < GoldUI.Length; i++) 
        {
            if (NetPlayer.CurrentGold >= i + 1) 
            {
                Debug.Log("GoldUI" + i + " est maintenant actif");


                if (!GoldUI[i].activeSelf) 
                {
                    GoldUI[i].SetActive(true);
                }
            }
        }

    }
}
