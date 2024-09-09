using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector_Start : MonoBehaviour
{

    public Network_Player NetPlayer = null; 
    public Player_UI_Manager UIManager = null;
    Player_UiReady UIReadyClone = null;
    public List<GameObject> CurrentPlayerInBox = new List<GameObject>(0);
    int CurrentPlayerIndex = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            CurrentPlayerInBox.Add(other.gameObject);
            CurrentPlayerIndex += 1;
            NetPlayer = CurrentPlayerInBox[CurrentPlayerIndex].GetComponent<Network_Player>();
            if (NetPlayer.Player_CanvaClone != null)
            {
                UIManager = NetPlayer.Player_CanvaClone.GetComponent<Player_UI_Manager>();

                if (NetPlayer.CurrentClass != PlayerScriptable.PossibleClass.None)
                {
                    UIManager.Instantiate_Ready_Button();
                    UIReadyClone = UIManager.Ready_ButtonClone.GetComponent<Player_UiReady>();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            for (int i = 0; i < CurrentPlayerInBox.Count; i++)
            {
                if (other.gameObject == CurrentPlayerInBox[i].gameObject)
                {
                    CurrentPlayerInBox[i].GetComponent<Network_Player>().IsReady = false;
                    if (NetPlayer.Player_CanvaClone != null)
                    {
                        Destroy(CurrentPlayerInBox[i].gameObject.GetComponent<Network_Player>().Player_CanvaClone.GetComponent<Player_UI_Manager>().Ready_ButtonClone);
                    }
                    CurrentPlayerIndex -= 1;
                    CurrentPlayerInBox.RemoveAt(i);
                }
            }
        }
    }
}
