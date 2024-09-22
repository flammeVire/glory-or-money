
using System.Collections;
using UnityEngine;
using Fusion;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class NetworkProjectile : NetworkBehaviour, ISpawned
{
    public float Speed;
    public float Life;
    public Network_Player NetPlayer;
    

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
        if (Object.HasStateAuthority) 
        {
            Runner.Despawn(Object);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Monster"))
        {
            if (NetPlayer.AttaqueScript != null)
            {
                Debug.Log("Projectil dmg = " + NetPlayer.AttaqueScript.Degat);
                other.GetComponent<IA_Network>().Rpc_AddPlayer(NetPlayer);
                other.GetComponent<IA_Network>().Rpc_Damage(NetPlayer.AttaqueScript.Degat);
            }
            
        }
    }
}
