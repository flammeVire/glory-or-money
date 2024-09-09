using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using UnityEngine.Animations;

public class Player_mouvement : NetworkBehaviour
{
    private CharacterController controller;
    Network_Player NetPlayer;
    Quaternion Look;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        NetPlayer = GetComponent<Network_Player>();

    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) {  return; }    //pour bouger seulement l'obj du joueur

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Runner.DeltaTime * NetPlayer.PlayerScriptableClone.Speed ;

        controller.Move(move);

        if (NetPlayer.IsLooking)
        {
            Look.eulerAngles = new Vector3(0, NetPlayer.MousePos, 0);
            transform.rotation = Look;
        }
        


        if(!NetPlayer.IsLooking && move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }


    
}
