using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Score : MonoBehaviour
{
    public Network_Player NetPlayer;

    public TextMeshProUGUI CurrentScore;

    public void ChangeScore()
    {
        CurrentScore.text = NetPlayer.PlayerScore.ToString();
    }

}
