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
        for (int i = 0; i <= GoldUI.Length; i++)
        {
            /*
         if (GoldUI[i - 1].activeSelf)
         {
                Debug.Log("GoldUI" + GoldUI[i-1] + " Est actif");
                return;

            }
            */


            if(NetPlayer.CurrentGold == i)
            {
                Debug.Log("GoldUI" + GoldUI[i - 1] + " Est now actif");
                GoldUI[i-1].SetActive(true);
            }
            


        }
        
    }


}
