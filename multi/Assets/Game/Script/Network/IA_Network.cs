using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Network : NetworkBehaviour
{
    public EnnemisScriptable ScriptableRef;
    public EnnemisScriptable EnnemisScriptableClone;
    [Networked] public float NetworkedLife { get; set; }
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            ScriptableRef.Initialize();
            EnnemisScriptableClone = Instantiate(ScriptableRef);
            NetworkedLife = EnnemisScriptableClone.Life;
            StartCoroutine(WaitUntilDie());
            Debug.Log(Object + "hauteur is" + transform.position.y);
            Debug.Log(Object + "has spawned");

        }
    }

    void FixedUpdate()
    {
        //Debug.Log(NetworkedLife);
        
    }

    #region gestion de mort
    IEnumerator WaitUntilDie()
    {
        yield return new WaitUntil(() => NetworkedLife <= 0);
        Destroy();
    }

    public void Destroy()
    {
        Debug.Log("il est mort");
        Runner.Despawn(Object);
    }
    #endregion


}
