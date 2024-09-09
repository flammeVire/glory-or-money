 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UI_Manager : MonoBehaviour
{
    #region variable

    [Header("UI_Prefab")]
    [SerializeField] GameObject Class_Selector;
    public GameObject Class_SelectorClone;
    [SerializeField] GameObject Ready_Button;
    public GameObject Ready_ButtonClone;
    [SerializeField] GameObject Stat_Panel;
    public GameObject Stat_PanelClone;
    [SerializeField] GameObject Score_Panel;
    public GameObject Score_PanelClone;
    [SerializeField] GameObject Spell_Panel;
    public GameObject Spell_PanelClone;
    [SerializeField] GameObject Gold_Panel;
    public GameObject Gold_PanelClone;


    public Network_Player NetPlayer;

    #endregion

    public void Instantiate_Class_Selector()
    {
        Class_SelectorClone = Instantiate(Class_Selector,this.transform,false);
        Class_SelectorClone.GetComponent<Class_Changer>().NetPlayer = NetPlayer;
    }

    public void Instantiate_Ready_Button()
    {
        if (NetPlayer.CurrentClass != PlayerScriptable.PossibleClass.None)
        {
            Ready_ButtonClone = Instantiate(Ready_Button, new Vector3(200, 50, 0), Quaternion.identity, transform);
            Ready_ButtonClone.GetComponent<Player_UiReady>().NetPlayer = NetPlayer;
            Ready_ButtonClone.GetComponent<Player_UiReady>().InitializeClickEvent(NetPlayer);
            Debug.Log("ReadyButton instantier");
        }
        
    }

    public void Instantiate_Stat_Panel()
    {
        Stat_PanelClone = Instantiate(Stat_Panel, this.transform.position, Quaternion.identity,transform);
        Stat_PanelClone.GetComponent<Player_Stat>().NetPlayer = NetPlayer;
    }

    
    public void Instantiate_Score_Panel()
    {
        Score_PanelClone = Instantiate(Score_Panel, this.transform.position, Quaternion.identity, transform);
        Score_PanelClone.GetComponent<Player_Score>().NetPlayer = NetPlayer;
        NetPlayer.Player_Score_Panel = Score_PanelClone;
    }
    

    public void Instantiate_Spell_Panel()
    {
        Spell_PanelClone = Instantiate(Spell_Panel,this.transform.position, Quaternion.identity, transform);
        Spell_PanelClone.GetComponent<Player_Spell>().NetPlayer = NetPlayer;
        NetPlayer.Player_Spell_Panel = Spell_PanelClone;
    }

    public void Instantiate_Gold_Panel()
    {
        Gold_PanelClone = Instantiate(Gold_Panel, this.transform.position, Quaternion.identity, transform);
        Gold_PanelClone.GetComponent<Player_Gold>().NetPlayer = NetPlayer;
        NetPlayer.Player_Gold_Panel = Gold_PanelClone;
    }

}
