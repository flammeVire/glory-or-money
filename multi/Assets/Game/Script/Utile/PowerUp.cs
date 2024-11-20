using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Fusion.NetworkRunner;
using UnityEngine.XR;
using Fusion;
public class PowerUp : NetworkBehaviour
{
    public Network_Player NetPlayer;

    [SerializeField] float Radius;
    [SerializeField] float DelayWarriorTank;
    [SerializeField] float DelayWarriorRage;
    [SerializeField] NetworkObject HealingCircleObj;
    [SerializeField] NetworkObject BearTrapObj;
    NetworkObject OldBearTrap;
    [SerializeField] NetworkObject Explosif;
    NetworkObject OldExplosif;

    [SerializeField] NetworkObject SmokeObj;
    [SerializeField] NetworkObject PoisonObj;
    RaycastHit[] hits;
    public LayerMask PlayerLayer;
    public LayerMask MonsterLayer;
    Collider myCollider;


    public bool IsSpell1;
    public bool IsSpell2;
    public bool IsSpell3;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
        StartingAllCoroutine();

    }

    void Class_Spell_Selector(PlayerScriptable.PossibleClass Player_Class, int SpellNumber)
    {
        switch (Player_Class)
        {

            case PlayerScriptable.PossibleClass.None:
                switch (SpellNumber)
                {
                    case 1:
                        Debug.Log("Lancement spell 1 de la class None");
                        Haunting();
                        break;
                    case 2:
                        Debug.Log("Lancement spell 2 de la class None");
                        BuffMonster();
                        break;
                    case 3:
                        Debug.Log("Lancement spell 3 de la class None");
                        SpookyGhost();
                        break;

                    default:
                        break;
                }
                break;
            case PlayerScriptable.PossibleClass.Guerrier:
                switch (SpellNumber)
                {
                    case 1:
                        Debug.Log("Lancement spell 1 de la class None");
                        Rage();
                        break;
                    case 2:
                        HealingCircle();
                        break;
                    case 3:
                        Tank();
                        break;

                    default:
                        break;
                }
                break;
            case PlayerScriptable.PossibleClass.Archer:
                switch (SpellNumber)
                {
                    case 1:
                        QuickShot();
                        break;
                    case 2:
                        BearTrap();
                        break;
                    case 3:
                        PoisonCloud();
                        break;

                    default:
                        break;
                }
                break;
            case PlayerScriptable.PossibleClass.Voleur:
                switch (SpellNumber)
                {
                    case 1:
                        Smoke();
                        break;
                    case 2:
                        QuickBank();
                        break;
                    case 3:
                        ExplodingBarrel();
                        break;

                    default:
                        break;
                }
                break;
            case PlayerScriptable.PossibleClass.Mage:
                switch (SpellNumber)
                {
                    case 1:
                        Invisibility();
                        break;
                    case 2:
                        HealingCircle();
                        break;
                    case 3:
                        Confusion();
                        break;

                    default:
                        break;
                }
                break;

            default: break;
        }
    }



    // créé fonction pour si changement de class IsSpell == false
    public void ChangingClass()
    {

        StopAllCoroutines();
        IsSpell1 = false;
        IsSpell2 = false;
        IsSpell3 = false;
        StartingAllCoroutine();
    }       // marche pas

    #region Ghost Spell

    // Haunting (spell1) reduit vitesse joueur alentour pendant 2.5secondes (vitesse a varier dans netplayer.SpeedReset)
    public void Haunting()
    {
        Debug.Log("Haunting");
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, transform.forward);
        hits = Physics.SphereCastAll(ray, Radius, 1, PlayerLayer, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != myCollider)
            {
                GameObject hitObject = hit.collider.gameObject;
                Network_Player NetPlayer = hitObject.GetComponent<Network_Player>();
                NetPlayer.Rpc_SpeedModifier(NetPlayer.Speed / 3,2.5f);
                Debug.Log("Touched: " + hitObject.name);
            }

        }

    }

    //buff monster augmente la vitesse des monstre et leurs dégâts 
    public void BuffMonster()
    {
        Debug.Log("Appel du buffMonster");
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, transform.forward);
        hits = Physics.SphereCastAll(ray, Radius, 1, MonsterLayer, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != myCollider)
            {
                GameObject hitObject = hit.collider.gameObject;
                IA_Network NetMonster = hitObject.GetComponent<IA_Network>();
                Debug.Log("hit" + hitObject);
                Debug.Log("Netmonster =" + NetMonster);
                Debug.Log("attaqueScript" + NetMonster.AttaqueScript);
                NetMonster.AttaqueScript.Rpc_ChangeScriptableStat(2);
            }
        }
    }
    //SpookyGhost (spell3) force les joueur alentour pendant 2secondes a regarder le fantôme
    public void SpookyGhost()
    {
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, transform.forward);
        hits = Physics.SphereCastAll(ray, Radius, 1, PlayerLayer, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != myCollider)
            {
                GameObject hitObject = hit.collider.gameObject;
                Player_mouvement NetPlayer = hitObject.GetComponent<Player_mouvement>();

                NetPlayer.Rpc_BeScared(this.Object);
                
                Debug.Log("Touched: " + hitObject.name);
            }

        }
    }

    #endregion

    #region Warrior Spell
    

    // Rage(Spell1) augmente les dégâts du joueurs pendant 2s
    void Rage()
    {
        Network_Player NetPlayer = this.GetComponent<Network_Player>();
        NetPlayer.Rpc_AttackModifier(1.5f, DelayWarriorRage);
    }

    // Healing Circle (Spell2) fait apparaitre un cercle de soin qui soigne tout les joueurs a l'interieur
    void HealingCircle()
    {
        Runner.Spawn(HealingCircleObj, transform.position);
    }

    //Tank (Spell3) reduit l'attaque mais augmente les PVs
    void Tank()
    {
        Network_Player NetPlayer = this.GetComponent<Network_Player>();
        NetPlayer.Rpc_AttackModifier(0.75f, DelayWarriorTank);
        NetPlayer.Rpc_LifeModifier(20, DelayWarriorTank);
    }

    #endregion
    #region Ranger Spell

    //QuickShot (Spell1) permet au joueur de ne pas avoir de delay d'arme  
    void QuickShot()
    {
        Debug.Log("QuickShot");
        NetPlayer = this.GetComponent<Network_Player>();
        NetPlayer.Rpc_DelayModifier(0);
    }

    //BearTrap (Spell2) fait apparaitre un piege a ours devant le joueur qui empeche le movement pendant quelque secondes
    void BearTrap()
    {
        if(OldBearTrap != null)
        {
            Runner.Despawn(OldBearTrap);
        }
        Vector3 spawnPosition = transform.position + transform.forward;
        OldBearTrap = Runner.Spawn(BearTrapObj, spawnPosition);
    }

    //PoisonCloud (Spell3) fait apparaitre un nuage toxique qui inflige des dégâts dans le temps
    void PoisonCloud()
    {
        Vector3 spawnPosition = transform.position + transform.forward;
        NetworkObject obj = Runner.Spawn(PoisonObj,spawnPosition);
        obj.GetComponent<PoisonCloud>().Rpc_SetPlayerAuthority(Object);
    }
    #endregion
    #region Rogue Spell

    //Smoke (Spell1) fait apparaitre un nuage de fumé sur le joueur
    void Smoke()
    {
        Runner.Spawn(SmokeObj,transform.position);
    }

    //QuickBank (Spell2) met l'or du joueur en bank immédiatement
    void QuickBank()
    {
        NetPlayer = this.GetComponent<Network_Player>();
        NetPlayer.GoldToBank();
    }


    //ExplodingBarrel (Spell3) fait apparaitre un tonneau explosif qui inflige des degat a tout les entité au alentour
    void ExplodingBarrel()
    {
        if(OldExplosif != null)
        {
            Runner.Despawn(OldExplosif);
        }
        Vector3 spawnPosition = transform.position + transform.forward;
        OldExplosif = Runner.Spawn(Explosif, spawnPosition);
    }
    #endregion
    #region Mage Spell


    //Invisibility (Spell1) retire le joueur de l'aggro du monstre
    void Invisibility()
    {
        Debug.Log("Invisibility");
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, transform.forward);
        hits = Physics.SphereCastAll(ray, Radius*2, 1, MonsterLayer, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != myCollider)
            {
                GameObject hitObject = hit.collider.gameObject;
                IA_Network NetMonster = hitObject.GetComponent<IA_Network>();

                if (NetMonster.DetectionScript.CurrentTarget != null)
                {
                    if (NetMonster.DetectionScript.CurrentTarget.GetComponentInParent<PowerUp>() == this)
                    {
                        GameObject myself = NetMonster.DetectionScript.CurrentTarget;
                        NetMonster.DetectionScript.Rpc_DisparitionOfPlayer(myself);
                        Debug.Log("i have disapear");
                    }
                }
                

                Debug.Log("Touched: " + hitObject.name);
            }

        }
    }
    
    //Spell 2 == healing spell (voir Warrior Spell Region)

    // Confusion (Spell3) Force le monstre a retourner a son spawn
    void Confusion()
    {
        Debug.Log("Confusion");
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, transform.forward);
        hits = Physics.SphereCastAll(ray, Radius*2, 1, MonsterLayer, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != myCollider)
            {
                GameObject hitObject = hit.collider.gameObject;
                IA_Mouvement_WalkToward NetMonster = hitObject.GetComponent<IA_Mouvement_WalkToward>();

                NetMonster.Rpc_ForceReturn();


                Debug.Log("Touched: " + hitObject.name);
            }

        }
    }
    #endregion


    #region Using Spell by Netplayer
    void StartingAllCoroutine()
    {
        StartCoroutine(UsingSpell1());
        StartCoroutine(UsingSpell2());
        StartCoroutine(UsingSpell3());
    }
    IEnumerator UsingSpell1()
    {
        IsSpell1 = false;
        yield return new WaitUntil(() => NetPlayer.UsingSpell1);
        IsSpell1 = true;
        Class_Spell_Selector(NetPlayer.CurrentClass,1);
        Debug.Log("using spell1 : " + NetPlayer.UsingSpell1);
        yield return new WaitForSeconds(NetPlayer.PlayerScriptableClone.DelaySpell1);
        StartCoroutine(UsingSpell1());
    }
    IEnumerator UsingSpell2()
    {
        IsSpell2 = false;
        yield return new WaitUntil(() => NetPlayer.UsingSpell2);
        IsSpell2 = true;
        Class_Spell_Selector(NetPlayer.CurrentClass,2);
        yield return new WaitForSeconds(NetPlayer.PlayerScriptableClone.DelaySpell2);

        StartCoroutine(UsingSpell2());
    }
    IEnumerator UsingSpell3()
    {
        IsSpell3 = false;
        yield return new WaitUntil(() => NetPlayer.UsingSpell3);
        IsSpell3 = true;
        Class_Spell_Selector(NetPlayer.CurrentClass,3);
        Debug.Log("using spell3 : " + NetPlayer.UsingSpell3);
        yield return new WaitForSeconds(NetPlayer.PlayerScriptableClone.DelaySpell3);

        StartCoroutine(UsingSpell3());
    }
    #endregion
}
