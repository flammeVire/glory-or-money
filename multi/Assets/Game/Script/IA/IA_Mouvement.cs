using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using TMPro;
public class IA_Mouvement : NetworkBehaviour
{
    public float Speed = 2f;
    Vector3 SpawnPoint;
    public IA_Detection Detection;
    Rigidbody rb;
    public float StopDistance = 0.1f;
    bool Retour = false;
    public float DistanceLimite;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnPoint = transform.position;
        if (this.GetComponent<NetworkTransform>() == null)
        {
            this.AddComponent<NetworkTransform>();
        }
    }

    void FixedUpdate()
    {
        bool InChase = Detection.InChase;
        GameObject Target = Detection.CurrentTarget;


        if (!Limite(SpawnPoint, DistanceLimite))
        {
            if(Target != null && InChase)
            {
                Follow(InChase, Target.transform);
            }
            else if(Target == null) 
            {
                RetourSpawn(Target);
            }

        }

        else { Retour = true; }
        
        if(Retour)
        {
            RetourSpawn(Target);
        }

    }


    private void Follow(bool Chase,Transform target)
    {
       Vector3 targetPosition = target.position;
       Vector3 currentPosition = transform.position;

        float distance = Vector3.Distance(currentPosition, targetPosition);

        if (distance > StopDistance )
        {
            Vector3 directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();
            rb.MovePosition(currentPosition + (directionOfTravel * Speed * Time.deltaTime));
        }
    }


    private bool Limite(Vector3 Spawn, float limite)
    {
        bool IsLimiteReach;
        if(Vector3.Distance(transform.position, Spawn) > limite)
        {
           return IsLimiteReach = true;
        }
        else
        {
            return IsLimiteReach = false;
        }
    }

    private void RetourSpawn(GameObject Target)
    {
            Vector3 currentPosition = transform.position;
            Vector3 directionOfTravel = SpawnPoint - currentPosition;
            directionOfTravel.Normalize();
            rb.MovePosition(currentPosition + (directionOfTravel * Speed * Time.deltaTime));
          
            if (Vector3.Distance(transform.position,SpawnPoint) <= 0.2f)
        {
            Retour = false;
        }
    }


    // comportement :
    // suis un joueur si il peut chasser
    // si un joueur il ne suis plus aucun joueur reviens
    //si il reviens ET que quelqu'un rentre en detection, reviens
    //si il atteins sa limite reviens
    //si le joueurs reviens car obj == null -> si atk, joueur qui attaque = current target 

}
