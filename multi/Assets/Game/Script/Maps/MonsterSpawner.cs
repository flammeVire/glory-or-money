using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MonsterSpawner : NetworkBehaviour
{
    [SerializeField] NetworkObject IANetwork_Prefab;

    private void Start()
    {
        Runner.Spawn(IANetwork_Prefab,this.transform.position,Quaternion.identity);
        Runner.Despawn(Object);
    }


    /*
        Si object a item MonsterSpawner
        Spawn un monstre
        Detruit l'object
     */

}
