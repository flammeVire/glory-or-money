using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class OnSpawnedMesh : NetworkBehaviour, ISpawned
{
    public NetworkObject Mesh;

    public void SetParentMesh(Transform Parent)
    {

        Mesh.transform.SetParent(Parent);
        Destroy(this);

    }
    
}
