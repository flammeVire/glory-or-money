using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class IA_Mouvement_WalkToward : NetworkBehaviour
{
    #region Variable
    [SerializeField] IA_Detection Detection;
    [SerializeField] IA_Network NetIA;

    [SerializeField] Rigidbody rb;

    public float StopDistance = 0.2f;
    public float DistanceLimite;


    public NetworkObject Mesh;
    public Vector3 Target;

    public Animator animator;
    [Networked] public Vector3 SpawnPoint {  get; set; }
    [Networked] public Vector3 PatrolPoint { get; set; }

    public State ActualState;
    #endregion


    public enum State
    {
        Wait,
        Chase,
        Return,
        Patrol,
        None
    }

    #region Unity Function
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnPoint = transform.position;
        PatrolPoint = Utilitaire.GetRandomVectorFromOrigin(DistanceLimite / 2, 0.08f, SpawnPoint);
    }
    
    private void FixedUpdate()
    {

        Rpc_SwitchState(ActualState);
        
    }

    #endregion

    #region Move Management

    State StateManagment()
    {
       // Debug.Log("State Management Previous state =" + ActualState);
        /*
        if (LimiteReach(SpawnPoint, DistanceLimite))
        {
            return State.Return;
        }           //si sort de sa limite
        else if (ActualState == State.Return)
        {
            if (LimiteReach(SpawnPoint, 0.5f))
            {
                Debug.Log("Toujours pas au spawn");
                return State.Return;
            }
            else
            {
                Debug.Log("au spawn");
                return State.Wait;
            }
        }                  // si reviens au spawn

        else if (Detection.CurrentTarget != null && ActualState != State.Return)        //si a une target
        {
            Target = Detection.CurrentTarget.transform.position;
            Debug.Log("State Management Move state =" + State.Move);

            return State.Move;
        }

        else if (LimiteReach(SpawnPoint, StopDistance) && ActualState != State.Patrol)     // si au spawn
        {
            Debug.Log("AU SPAWN");
            return State.Wait;
        }       

        else if (ActualState == State.Patrol)
        {
            if (LimiteReach(PatrolPoint, StopDistance))
            {
                Debug.Log("Point atteins");
                return State.Wait;
            }
            else
            {
                Debug.Log("Continue a Patroll");
                return State.Patrol;
            }
        }               // si patrouille

        else if (ActualState == State.Wait || ActualState != State.Patrol)
        {
            PatrolPoint = Utilitaire.GetRandomVectorFromOrigin(DistanceLimite / 2, 0.08f, SpawnPoint);
            return State.Patrol;
        }   //si attends ou ne patrouille pas

        else if (Detection.CurrentTarget == null)                // si n'a pas de target
        {
            Target = SpawnPoint;
            return State.Move;
        }
        else                            // si aucun autre cas
        {
            return State.None;
        } //si aucun cas

        */

        if (LimiteReach(SpawnPoint, DistanceLimite))
        {
            return State.Return;
        } // si sort de la limite reviens

        else if (ActualState == State.Return)
        {
            if (!LimiteReach(SpawnPoint, StopDistance))
            {
                return State.Wait;
            }   // si au spawn attends
            else { return State.Return; }           // sinon reviens
        }       // si reviens  -> reviens jusqu'au spawn

        else if (Detection.CurrentTarget != null)
        {
            Target = Detection.CurrentTarget.transform.position;
            return State.Chase;
        }   // si joueur dans rayon -> chasse

       

        else if (ActualState == State.Wait)
        {
            PatrolPoint = Utilitaire.GetRandomVectorFromOrigin(DistanceLimite / 2, transform.position.y, SpawnPoint);
            return State.Patrol;
        }      // si attends -> patrouille

        else if(ActualState == State.Patrol)
        {
            if (!LimiteReach(PatrolPoint,StopDistance)) 
            {
                return State.Wait;
            }
            else
            {
                return State.Patrol;
            }
        }   // Si patrouille, va jusqu'au point de patrouille

        else if (!LimiteReach(SpawnPoint, StopDistance))
        {
            return State.Wait;
        }       // si au spawn -> attends

        else if (Detection.CurrentTarget == null)
        {
            Target = SpawnPoint;
            return State.Chase;
        }   // si aucun joueur dans rayon -> va au spawn

        else
        {
            return State.None;
        }       // si aucun cas -> aucun state
    }

    private bool LimiteReach(Vector3 Spawn, float limite)
    {
        if (Vector3.Distance(transform.position, Spawn) > limite)
        {
            return true;
        }
        else
        {
            return false;
        }
        
        


    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    void Rpc_SwitchState(State NewState)
    {
        ActualState = StateManagment();
        //Debug.Log("State =" + ActualState);
        switch (NewState)
        {
            case State.Wait:
                break;

            case State.Chase:
                MoveToTarget(Target);
                LookAt(Target);
                break;

            case State.Return:
                MoveToTarget(SpawnPoint);
                LookAt(SpawnPoint);
                break;

            case State.Patrol:
                MoveToTarget(PatrolPoint);
                LookAt(PatrolPoint);
                break;


            default:
                Debug.Log("Incorrect State");
                ActualState = State.Return;
                break;
        }
    }

        


    public void MoveToTarget(Vector3 TargetPosition)
    {
        animator.SetTrigger("Walking");
        Vector3 currentPosition = transform.position;

        float distance = Vector3.Distance(currentPosition, TargetPosition);

        if (distance > StopDistance)
        {
            Vector3 directionOfTravel = (TargetPosition - currentPosition).normalized;
            Vector3 newPosition = currentPosition + (directionOfTravel * NetIA.Speed * Runner.DeltaTime);

            rb.MovePosition(newPosition);
        }
    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    public void Rpc_ForceReturn()
    {
        ActualState = State.Return;
    }

    #endregion

    void LookAt(Vector3 TargetPosition) 
    {
        transform.LookAt(TargetPosition);
    }

}
