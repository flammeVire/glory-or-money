using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.ComponentModel;
using System;
public class IA_Mouvement : NetworkBehaviour
{
    [SerializeField] IA_Detection Detection;
    [SerializeField] IA_Network NetIA;

    [SerializeField] Rigidbody rb;

    public float StopDistance = 0.2f;
    public float DistanceLimite;

    public bool CanMove = true;
    public bool Retour = false;
    public bool IsPatrol = false;

    public NetworkObject Mesh;
    public GameObject Target;

    public Animator animator;

    public Vector3 SpawnPoint;
    
    Vector3 PatrolPoint;

    #region Unity Function
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        SpawnPoint = transform.position;
        PatrolPoint = Utilitaire.GetRandomVectorFromOrigin(DistanceLimite / 2, 0.08f, SpawnPoint);
    }

    public void FixedUpdate()
    {

        if (CanMove)
        {
            Target = Detection.CurrentTarget;
            if (Retour)
            {
               // Debug.Log("BoucleRetour");
                if (!LimiteReach(SpawnPoint, StopDistance))
                {
                    Retour = false;
                }
                else
                {
                    Follow(SpawnPoint);
                }
            }
            else
            {
                if (LimiteReach(SpawnPoint, DistanceLimite))
                {
                    Retour = true;
                }
                else
                {
                    if (Target == null)
                    {
                        if (!LimiteReach(SpawnPoint, StopDistance))         
                        {
                            IsPatrol = true;
                        }
                        else 
                        {
                            Follow(SpawnPoint); 
                        }
                    }
                    else
                    {
                       // Debug.Log("boucle target = true");
                        IsPatrol = false;
                        Follow(Target.transform.position);
                    }

                }

            }
            if (IsPatrol)
            {
                Patrol();
            }

        }
       // Debug.Log("Point de patrouille = " + PatrolPoint);
    }

    #endregion

    private void Follow(Vector3 targetPosition)
    {

        animator.SetTrigger("Walking");

        Vector3 currentPosition = transform.position;

        LookingAtTarget(targetPosition);
       // Debug.Log("It follow" + targetPosition.ToString());
        float distance = Vector3.Distance(currentPosition, targetPosition);

        if (distance > StopDistance )
        {
          //  Debug.Log("il suis encore");
            Vector3 directionOfTravel = (targetPosition - currentPosition).normalized;
            Vector3 newPosition = currentPosition + (directionOfTravel * NetIA.EnnemisScriptableClone.Speed * Runner.DeltaTime);

            // Utiliser MovePosition pour déplacer le Rigidbody de manière fluide
            rb.MovePosition(newPosition);
        }

    }
    private bool LimiteReach(Vector3 Spawn, float limite)
    {
     //   Debug.Log("distance du spawn = " + Vector3.Distance(transform.position,Spawn));
        if(Vector3.Distance(transform.position, Spawn) > limite)
        {
           return true;
        }
        else
        {
            return false;
        }
    }

    private void Patrol()
    {
        if(!IsPatrol)
        {
            IsPatrol = true;
        }
        else
        {
            
            if(!LimiteReach(PatrolPoint,StopDistance)) 
            {
               // Debug.Log("Point de patrouille atteins");
                IsPatrol = false;
                PatrolPoint = Utilitaire.GetRandomVectorFromOrigin(DistanceLimite / 2, 0.08f, SpawnPoint);
            }
            Follow(PatrolPoint);
        }
            
        
    }

    private void LookingAtTarget(Vector3 target)
    {
       // Debug.Log("REGARDE " + target.ToString());
        //Mesh.transform.LookAt(target);
    }

    
}
