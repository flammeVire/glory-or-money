using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class Network_Player : NetworkBehaviour, IDespawned
{
    #region Variable

    #region GameObject
    [Header("PlayerMesh")]

    public PlayerScriptable.PossibleClass SpawnClass;

    NetworkObject PlayerMesh;
    public NetworkObject Player;
    [SerializeField] private NetworkObject Mesh1;
    [SerializeField] private NetworkObject Mesh2;
    [SerializeField] private NetworkObject Mesh3;
    [SerializeField] private NetworkObject Mesh4;
    [SerializeField] private NetworkObject MeshNone;

    [Header("PlayerUtile")]
    public GameObject CamPrefab;
    GameObject CamPrefabClone;
    public GameObject Player_Canva;
    public GameObject Player_CanvaClone;

    [Header("Canva_UI")]
    public GameObject ReadyButtonClone;
    public GameObject Player_Score_Panel;
    public GameObject Player_Spell_Panel;
    public GameObject Player_Gold_Panel;
    public GameObject Player_AllPlayer;

    public GameObject PlayerNameCanva;
    public TextMeshProUGUI PlayerNameUI;
    #endregion
    #region ScriptRef
    [Header("ScriptRef")]
    [SerializeField] PlayerScriptable ScriptableObjRef;
    public PlayerScriptable PlayerScriptableClone;
    public Game_Network NetGame;
    Player_UI_Manager UIManager;
    public PowerUp PlayerPowerup;
    public Player_Attaque AttaqueScript = null;

    #endregion
    #region Boolean
    [Header("Boolean")]
    public bool IsLooking;
    [Networked]public NetworkBool IsAttaking { get; set; } = false;
    
    public bool UsingSpell1 = false;
    public bool UsingSpell2 = false;
    public bool UsingSpell3 = false;
    public bool IsInterracting = false;
    public bool IsAGhost;

    #endregion
    #region input
    public float MousePos;
    public string SpawnName;
    #endregion
    #region Level
    [Header("Level")]
    public float NextLevel = 10;
    int palier = 0;
   

    #endregion
    #region Class
    public PlayerScriptable.PossibleClass CurrentClass;
    #endregion
    #region Networked data

    [Networked] public string PlayerName {  get; set; }
    [Networked] public int PlayerScore { get; set; }
    [Networked] public int currentLevel { get; set; }
    [Networked] public float CurrentXP { get; set; }
    [Networked] public int CurrentGold { get; set; }
    [Networked] public int TotalGold { get; set; }
    [Networked] public float Speed {  get; set; }
    [Networked] public bool IsReady { get; set; }
    [Networked] public float maxHp { get; set; }
    #endregion

    #endregion

    #region Unity Function

    public override void Spawned()
    {

        if (HasStateAuthority)

        {
            ScriptableObjRef.Initialize();
            PlayerScriptableClone = Instantiate(ScriptableObjRef);
            CamPrefabClone = Instantiate(CamPrefab);
            CamPrefabClone.GetComponent<PlayerCamera>().target = this.transform;
            Player_CanvaClone = Instantiate(Player_Canva);
            Player_CanvaClone.GetComponent<Player_UI_Manager>().NetPlayer = this;
            UIManager = Player_CanvaClone.GetComponent<Player_UI_Manager>();

            UIManager.Instantiate_Gold_Panel();
            Player_Gold_Panel.GetComponent<Player_Gold>().SetGoldToZero();
           

            PlayerName = SpawnName;
            if(PlayerName == "")
            {
                PlayerName = Utilitaire.GetRandomName(5);
            }
              
            StartCoroutine(WaitToDie());
            

            
            

            UIManager.Instantiate_Spell_Panel();

            SetCurrentClass(SpawnClass);

            Rpc_NamingUI(PlayerName);
            NetGame = FindAnyObjectByType<Game_Network>();
            ShowPlayerToNetGame(Runner.LocalPlayer, this);

        }
    }
    void Update()
    {
        if (!HasStateAuthority) { return; }
        InputLooking();
        InputAttack();
        InputSpell1();
        InputSpell2();
        InputSpell3();
        InputInterracting();
        TestDeNiveauASuppr(); // ajoute lvl quand Y appuyer
        //Debug.Log("vie restante de " + gameObject.name + " == " + PlayerScriptableClone.Life);
        //Debug.Log("or in current : " + CurrentGold);
        //Debug.Log("Is ready == " + IsReady);
    }

    #endregion
    #region Gestionnaire d'input
    // laisser dans networkPlayer car si fantome : pas d'arme mais peut spell
    public bool InputAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            IsAttaking = true;
        }
        else { IsAttaking = false; }

        return IsAttaking;

    }

    public bool InputLooking()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            IsLooking = true;


        } else if (Input.GetButtonUp("Fire2")) { IsLooking = false; }
        if (IsLooking)
        {
            MousePos = CamPrefabClone.GetComponent<PlayerCamera>().GetMousePos();
        }

        return IsLooking;
    }

    public bool InputSpell1()
    {
        if (Input.GetButtonDown("Spell1"))
        {
             UsingSpell1 = true;
        }
        else { UsingSpell1 = false; }

        return UsingSpell1;
    }
    public bool InputSpell2()
    {
        if (Input.GetButtonDown("Spell2"))
        {
             UsingSpell2 = true;
        }
        else { UsingSpell2 = false; }

        return UsingSpell2;
    }
    public bool InputSpell3()
    {
        if (Input.GetButtonDown("Spell3"))
        {
             UsingSpell3 = true;
        }
        else { UsingSpell3 = false; }

        return UsingSpell3;
    }

    public bool InputInterracting()
    {
        if (Input.GetButtonDown("Interract"))
        {
            Debug.Log("Interracting");
            IsInterracting = true;
        }
        else { IsInterracting = false; }

        return IsInterracting;
    }




    #endregion

    #region Gestion de Niveau
    public void MonterDeNiveau()        // quand le joueur tue un monstre on lui donne de l'xp
    {

        if (CurrentXP >= NextLevel)      // si l'xp est assez, on passe un niveau
        {
            CurrentXP -= NextLevel;     // si on passe un niveau on retourne a 0xp
            NextLevel *= 1.5f;
            currentLevel+= 1;
           // Debug.Log("prochain palier = " + palier);
           // Debug.Log("prochain niveau = " + NextLevel);
            Debug.Log("niveau actuel = " + currentLevel);

        }
        switch (palier)
        {
            case 0: //si joueur est niveau 0
                if (currentLevel == palier)
                {
                    Debug.Log("ON A PASSER LE PALIER" + palier);
                    palier = 1;

                }
                break;

            case 1: //si joueur atteins le niveau 1
                if (currentLevel == palier)
                {
                    Debug.Log("ON A PASSER LE PALIER" + palier);
                    palier = 3;

                }
                break;
            case 3: //si le joueur atteins le niveau 3
                if (currentLevel == palier)
                {
                    Debug.Log("ON A PASSER LE PALIER" + palier);
                    palier = 6;

                }
                break;
            case 6:
                if (currentLevel == palier)
                {
                    Debug.Log("ON A PASSER LE PALIER" + palier);
                    palier = 10;

                }
                break;
            case 10:
                Debug.Log("Niveau Max ATTEINS");
                break;

            default:
                break;
        }
        if (Player_Score_Panel != null)
        {
            Player_Score_Panel.GetComponent<Player_Score>().ChangeScore();
        }
    }

    void TestDeNiveauASuppr()
    {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            CurrentXP += 1;
            PlayerScore += 1;
            NetGame.Rpc_BestPlayerScore();
            MonterDeNiveau();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerScriptableClone.Life -= 5;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CurrentGold += 1;
            SetMaxGold();
            
        }

       
        
       
    }




    // si le niveau est assez, on passe le palier
    // si on passe un palier on augmente les pouvoirs maximum



    #endregion

    #region Classe

    public void SetCurrentClass(PlayerScriptable.PossibleClass newClass)
    {
        CurrentClass = newClass;

        if (PlayerMesh != null) {
            Runner.Despawn(PlayerMesh);
            PlayerMesh = null;
        }
        switch (CurrentClass)
        {

            case PlayerScriptable.PossibleClass.None:
                IsAGhost = true;
                PlayerMesh = Runner.Spawn(MeshNone, this.transform.position, this.transform.rotation, Runner.LocalPlayer, BeforeSpawned);
                break;

            case PlayerScriptable.PossibleClass.Guerrier:

                PlayerMesh = Runner.Spawn(Mesh1, this.transform.position, this.transform.rotation, Runner.LocalPlayer, BeforeSpawned);
                break;

            case PlayerScriptable.PossibleClass.Archer:

                PlayerMesh = Runner.Spawn(Mesh2, this.transform.position, this.transform.rotation, Runner.LocalPlayer, BeforeSpawned);
                break;

            case PlayerScriptable.PossibleClass.Voleur:

                PlayerMesh = Runner.Spawn(Mesh3, this.transform.position, this.transform.rotation, Runner.LocalPlayer, BeforeSpawned);
                break;

            case PlayerScriptable.PossibleClass.Mage:

                PlayerMesh = Runner.Spawn(Mesh4, this.transform.position, this.transform.rotation, Runner.LocalPlayer, BeforeSpawned);
                break;

            default: break;
        }
        PlayerScriptableClone.ChangeStat(newClass);
        maxHp = PlayerScriptableClone.Life;


        if (CurrentClass != PlayerScriptable.PossibleClass.None)
        {
            IsAGhost = false;
            if (UIManager.Stat_PanelClone != null)
            {
                Debug.Log("Destroy stat panel");
                Destroy(UIManager.Stat_PanelClone);
            }
            if (UIManager.Score_PanelClone != null)
            {
                Destroy(UIManager.Score_PanelClone);
            }
           // Debug.Log("Instantiate");
            if (CurrentClass != PlayerScriptable.PossibleClass.None)
            {
                UIManager.Instantiate_Stat_Panel();
                UIManager.Instantiate_Score_Panel();
                Player_Gold_Panel.GetComponent<Player_Gold>().SetGoldToZero();
            }

        }
        Speed = PlayerScriptableClone.Speed;
        Player_Spell_Panel.GetComponent<Player_Spell>().SetTimer();
        AttaqueScript = GetComponentInChildren<Player_Attaque>();

        Debug.Log("le nouveau current mesh est " + PlayerMesh);
        
    }

    public void BeforeSpawned(NetworkRunner runner, NetworkObject networkObject)
    {
        var MeshScript = networkObject.GetComponent<OnSpawnedMesh>();
        if (MeshScript != null)
        {
            MeshScript.SetParentMesh(this.transform);
        }
    }


    #endregion

    #region Gestion de l'or

    public void SetMaxGold()
    {
        Debug.Log("MaxGoldSetting");
        if (CurrentGold > 5)
        {
            CurrentGold = 5;
            Debug.Log("Max Moulaga atteins");
        }
        if (Player_Gold_Panel != null)
        {
            Player_Gold_Panel.GetComponent<Player_Gold>().AddGold();
        }
    }

    

    
    public void GoldToBank()
    {
        TotalGold += CurrentGold;
        CurrentGold -= CurrentGold;
        Player_Gold_Panel.GetComponent<Player_Gold>().SetTotalGold();
        Player_Gold_Panel.GetComponent<Player_Gold>().SetGoldToZero();
        Debug.Log("total gold after bank = " + TotalGold);
        Debug.Log("current gold after bank = " + CurrentGold);
    }
    
    #endregion

    #region Gestion de Mort

    IEnumerator WaitToDie()
    {
        yield return new WaitUntil(() => CurrentClass != PlayerScriptable.PossibleClass.None);
        Debug.Log("La Classe a des PV");
        yield return new WaitUntil(() => PlayerScriptableClone.Life <= 0);
        Debug.Log("IsDead");
        SetCurrentClass(PlayerScriptable.PossibleClass.None);
    }

    #endregion

    #region Gestion de lancement de partie


    
    public void ImReady()
    {
        IsReady = true;
        Debug.Log(" LE JOUEUR EST READY");
        NetGame.Rpc_CanStart();
        NetGame.Rpc_ReadyAllPlayerUI(Runner.LocalPlayer, IsReady);
        DestroyReadyButton();
    }

    public void DestroyReadyButton()
    {
        Destroy(ReadyButtonClone);
    }


    #endregion

    #region OnSpawn

   
    void ShowPlayerToNetGame(PlayerRef PlayerRunner, Network_Player Netplayer)
    {
        if (NetGame == null)
        {
            Debug.Log("NetGame non trouver");
            NetGame = FindAnyObjectByType<Game_Network>();
            ShowPlayerToNetGame(Runner.LocalPlayer, this);
        }
        else
        {
            Debug.Log("NetGameTrouver");
            NetGame.Rpc_PlayerAdd(PlayerRunner, this);
            NetGame.Rpc_ShowAllPlayerUI(PlayerRunner);
        }
    }
    

    [Rpc(RpcSources.All,RpcTargets.All)]
    void Rpc_NamingUI(String Name)
    {
        PlayerNameUI.text = Name;
    }
    #endregion

    #region modifier Stat

    #region Speed
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_SpeedModifier(float NewSpeed,float delay)
    {
        
        Speed = NewSpeed;
        StartCoroutine(SpeedReset(delay));
    }


    IEnumerator SpeedReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("ResetSpeed");
        if (PlayerScriptableClone != null)
        {
            Speed = PlayerScriptableClone.Speed;
        }
    }
    #endregion
    #region Attack
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_AttackModifier(float multiply, float delay)
    {
        if (HasStateAuthority)
        {
            AttaqueScript.Degat *= multiply;
        }
        StartCoroutine(AttackReset(delay));
        
    }
    IEnumerator AttackReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("ResetSpeed");
        if (PlayerScriptableClone != null)
        {
            AttaqueScript.Degat = PlayerScriptableClone.Degat;
        }
    }

    #endregion
    #region Life
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_LifeModifier(float add,float delay)
    {
        if (HasInputAuthority)
        {
            PlayerScriptableClone.Life += add;
            maxHp += add;
            UIManager.Stat_PanelClone.GetComponent<Player_Stat>().MaxHealth = maxHp;
        }
        StartCoroutine(LifeReset(add,delay));

    }
    IEnumerator LifeReset(float add,float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("ResetLife");
        if (PlayerScriptableClone != null)
        {
            PlayerScriptableClone.Life -= add;
            maxHp -= add;
            UIManager.Stat_PanelClone.GetComponent<Player_Stat>().MaxHealth = maxHp;
        }
    }


    #endregion
    #region Delay
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_DelayModifier(float delay)
    {
        if (HasStateAuthority)
        {
            AttaqueScript.Delay = delay;
        }
        StartCoroutine(DelayReset());

    }
    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("ResetSpeed");
        if (PlayerScriptableClone != null)
        {
            AttaqueScript.Delay = PlayerScriptableClone.DelayWeapon;
        }
    }
    #endregion
    #endregion

    #region healing

    public void healing(float PV_restored)
    {
        if (PlayerScriptableClone.Life < maxHp)
        {
            PlayerScriptableClone.Life += PV_restored;
            
        }
        else
        {
            CheckLimitePv();
        }
    
    }

    void CheckLimitePv()
    {
        if(PlayerScriptableClone.Life >= maxHp)
        {
            PlayerScriptableClone.Life = maxHp;
        }
    }
    

    #endregion
}
