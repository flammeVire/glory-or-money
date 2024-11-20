using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BearTrap : NetworkBehaviour
{
    public float LifeTime;
    public float StuckTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && !other.gameObject.GetComponent<Network_Player>().IsAGhost)
        {
            Debug.Log("Touched a player");
            other.GetComponentInParent<Network_Player>().Rpc_SpeedModifier(0,StuckTime);
            StartCoroutine(Dying());
        }
        if(other.gameObject.layer == 8)
        {
            Debug.Log("Touched a Monster");
            other.GetComponent<IA_Network>().Rpc_SpeedModifier(0,StuckTime);
            StartCoroutine(Dying());
        }
    }

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(LifeTime);
        Runner.Despawn(Object);
    }

}
