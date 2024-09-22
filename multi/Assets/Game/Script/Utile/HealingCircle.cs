using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class HealingCircle : NetworkBehaviour
{
    [SerializeField] Collider HealCollider;
    [SerializeField] float PV_Restored;
    [SerializeField] float lifetime;
    private void Start()
    {
        StartCoroutine(EndOfLife());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && other.gameObject.GetComponent<Network_Player>().CurrentClass != PlayerScriptable.PossibleClass.None)
        {
            Debug.Log("HAS ENTER");
            StartCoroutine(Healing(other.gameObject.GetComponent<Network_Player>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7 && other.gameObject.GetComponent<Network_Player>().CurrentClass != PlayerScriptable.PossibleClass.None)
        {

            Debug.Log("HAS LEave");

        }
    }

    IEnumerator EndOfLife()
    {
        yield return new WaitForSeconds(lifetime);
        Runner.Despawn(Object);
    }


    IEnumerator Healing(Network_Player player)
    {
        player.healing(PV_Restored);
        yield return new WaitForSeconds(1);
        StartCoroutine(Healing(player));
    }
}
