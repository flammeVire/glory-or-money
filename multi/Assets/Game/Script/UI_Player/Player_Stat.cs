using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Stat : MonoBehaviour
{
    public Network_Player NetPlayer;
    public GameObject life;
    public GameObject level;

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI levelText;


    float MaxHealth;
    float LastCurrentHealth;

    float MaxXp;
    float LastCurrentXP;
    //float CurrentLevel;

    private void Start()
    {
        MaxHealth = NetPlayer.PlayerScriptableClone.Life;
        LastCurrentHealth = MaxHealth;
        MaxXp = NetPlayer.NextLevel;
        LastCurrentXP = NetPlayer.CurrentXP;
        level.transform.localScale = new Vector3(1, 0, 1);
        StartCoroutine(LifeText(LastCurrentHealth));
        StartCoroutine(XpText());
    }

    private void LateUpdate()
    {
        HealthUI();
        LevelUI();
    }

    #region health
    void HealthUI()
    {
        float currentHealth = NetPlayer.PlayerScriptableClone.Life;     // recuperer les dgt   

        Debug.Log("current health == " + currentHealth);
        if (currentHealth != LastCurrentHealth)
        {
           // float degatTaken = MaxHealth - currentHealth;
            float degatInPercent = 100 / (MaxHealth / (MaxHealth - currentHealth));
            
            float lifeInPercent = (100 - degatInPercent) / 100;

           // Debug.Log("dgt taken == " + degatTaken);
            //Debug.Log("degat en % == " + degatInPercent);
            //Debug.Log("life en % == " + lifeInPercent);
            life.transform.localScale = new Vector3(lifeInPercent,1, 1);
            StartCoroutine(LifeText(currentHealth));
            LastCurrentHealth = currentHealth;
        }
        
    

                 /*
                        100 -> taille barre
                      divisé (PVmax / dommage reçu)
                        == % barre a retirer
                 */
    }

    IEnumerator LifeText(float life)
    {
        yield return null;
        lifeText.text = life.ToString() + " / " + MaxHealth;
        
    }
    #endregion

    #region level
    void LevelUI() 
    {
        float currentXp = NetPlayer.CurrentXP;

        if(currentXp != LastCurrentXP)
        {


            float XpMissing = MaxXp - currentXp;
            float XpInPercent = 100 / (MaxXp / XpMissing);

            float BarreInPercent = (100 - XpInPercent) / 100;

            level.transform.localScale = new Vector3(1, BarreInPercent, 1);
            LastCurrentXP = currentXp;
  //          Debug.Log("MaxXp avant = " + MaxXp);
            if (MaxXp != NetPlayer.NextLevel)
            {
                MaxXp = NetPlayer.NextLevel;
//                Debug.Log("MaxXp apres = " +MaxXp);
            }
            StartCoroutine(XpText());
        }

    }

    IEnumerator XpText()
    {
        yield return null;
        levelText.text = "Level \n" + NetPlayer.currentLevel;

    }

    #endregion

}
