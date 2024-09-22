using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class MonsterSpawner : NetworkBehaviour
{
    [SerializeField] NetworkObject IANetwork_Prefab;

    private void Start()
    {
        transform.SetParent(null);
        if(this.Object.GetComponent<NetworkTransform>() == null)
        {
            this.Object.AddComponent<NetworkTransform>();
        }


        
        Rpc_SpawningMonster();
    }

    //[Rpc(RpcSources.All, RpcTargets.All)]
    void Rpc_SpawningMonster()
    {
        if (Object.HasStateAuthority)
        {
            Vector3 SpawnPoint = transform.position;
            Debug.Log(SpawnPoint);

            Runner.Spawn(IANetwork_Prefab, SpawnPoint, Quaternion.identity);
            // Runner.Despawn(Object);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("SpawnMonster");
            Rpc_SpawningMonster();
        }
    }



    /*
        Si object a item MonsterSpawner
        Spawn un monstre
        Detruit l'object
     */

}
