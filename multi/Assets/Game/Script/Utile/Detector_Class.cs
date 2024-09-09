using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector_Class : MonoBehaviour
{
    public Network_Player NetPlayer = null;
    public Player_UI_Manager UIManager = null;
    Class_Changer Class_ChangerClone = null;
    public List<GameObject> CurrentPlayerInBox = new List<GameObject>(0);
    int CurrentPlayerIndex = -1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            CurrentPlayerInBox.Add(other.gameObject);
            CurrentPlayerIndex += 1;
            NetPlayer = CurrentPlayerInBox[CurrentPlayerIndex].GetComponent<Network_Player>();
            if (NetPlayer.Player_CanvaClone != null)
            {
                UIManager = NetPlayer.Player_CanvaClone.GetComponent<Player_UI_Manager>();

                UIManager.Instantiate_Class_Selector();


                Class_ChangerClone = UIManager.Class_SelectorClone.GetComponent<Class_Changer>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            for(int i = 0; i < CurrentPlayerInBox.Count; i++) 
            {
               if(other.gameObject == CurrentPlayerInBox[i].gameObject)
                {
                    if (NetPlayer.Player_CanvaClone != null)
                    {
                        Destroy(CurrentPlayerInBox[i].gameObject.GetComponent<Network_Player>().Player_CanvaClone.GetComponent<Player_UI_Manager>().Class_SelectorClone);
                    }
                    CurrentPlayerIndex -= 1;
                    CurrentPlayerInBox.RemoveAt(i);
                }
            }
        }
    }
}
