using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Attaque : MonoBehaviour
{
    public IA_Network NetIA;
    public bool IsAttacking;
    public List<GameObject> AttackTarget;
    public Animator animator;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("can attack");
            AttackTarget.Add(other.gameObject);
            IsAttacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            AttackTarget.Remove(other.gameObject);


            if(AttackTarget.Count >= 0 )
            {
                IsAttacking =false;
            }
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitUntil(() => IsAttacking == true);

        animator.SetBool("Attaking", true);
        animator.SetTrigger("Punching");
        NetIA.GetComponent<IA_Mouvement>().CanMove = false;
        Debug.Log("Wait to Attacking");
        yield return new WaitForSeconds(NetIA.EnnemisScriptableClone.DelayAttack);

        animator.SetBool("Attaking", false);

        
        foreach (GameObject PossibleTarget in AttackTarget)
        {
            if (PossibleTarget != null)
            {
                Network_Player NetPlayer = PossibleTarget.GetComponentInParent<Network_Player>();         //recuperer le script
                NetPlayer.PlayerScriptableClone.Life -= NetIA.EnnemisScriptableClone.Degat; // inflige les degats
            }
        }


        NetIA.GetComponent<IA_Mouvement>().CanMove = true;
       

        StartCoroutine(Attack());
    }


    /*
     
        Quand un joueur Rentre dans la zone d'attaque -> attaque:
        on ne bouge plus
        on attends (speed attaque)(vitesse d'animation)
        si le joueur est dans le collider on inflige des dégâts
        on rebouge
        si le joueur est encore dans la zone d'attaque on retape
        
        
        

     */
}
