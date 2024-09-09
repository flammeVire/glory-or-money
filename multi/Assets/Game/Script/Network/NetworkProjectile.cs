/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkProjectile : NetworkBehaviour, ISpawned
{
    public float Speed;
    public float Life;
    public Network_Player NetPlayer;
    [SerializeField] Rigidbody rb;
    public override void Spawned()
    {
        StartCoroutine(LifeTime());
    }

    public override void FixedUpdateNetwork()
    {
        Moving();
    }


    void Moving()
    {
        rb.velocity = transform.forward * Speed; 
    }



    IEnumerator LifeTime()
    {
        yield return new WaitForSecondsRealtime(Life);
        EndOfLife();
    }

    void EndOfLife()
    {
        Runner.Despawn(Object);
    }


    private void OnTriggerEnter(Collider other)
    {


            if (other.gameObject.tag == "Monster" )
            {
                other.GetComponent<IA_Network>().NetworkedLife -= NetPlayer.PlayerScriptableClone.Degat;
                EndOfLife();
            }        
    }
}
*/
using System.Collections;
using UnityEngine;
using Fusion;

public class NetworkProjectile : NetworkBehaviour, ISpawned
{
    public float Speed;
    public float Life;
    public Network_Player NetPlayer;
    //[SerializeField] private Rigidbody rb;

    public override void Spawned()
    {
        StartCoroutine(LifeTime());
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            Moving();
        }
    }

    private void Moving()
    {
        transform.position += transform.forward * Speed * Runner.DeltaTime;
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSecondsRealtime(Life);
        EndOfLife();
    }

    private void EndOfLife()
    {
        if (Object.HasStateAuthority) // Ensure only the authoritative instance despawns
        {
            Runner.Despawn(Object);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Monster"))
        {
            other.GetComponent<IA_Network>().NetworkedLife -= NetPlayer.PlayerScriptableClone.Degat;

        }
    }
}
