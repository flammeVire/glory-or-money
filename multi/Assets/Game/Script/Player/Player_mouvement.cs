using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player_mouvement : NetworkBehaviour
{
    private CharacterController controller;
    public GameObject CamPrefab;

    public float PlayerSpeed = 2f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void Spawned()
    {
        
        if (HasStateAuthority)
        {
            GameObject CamPrefabClone = Instantiate(CamPrefab);
            CamPrefabClone.GetComponent<PlayerCamera>().target = this.transform;
        }
    }



    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) {  return; }    //pour bouger seulement l'obj du joueur

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Runner.DeltaTime * PlayerSpeed;


        controller.Move(move);


        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
}
