using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class WaitToDespawn : NetworkBehaviour
{
    public float LifeTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(LifeTime);
        Runner.Despawn(Object);
    }
}

