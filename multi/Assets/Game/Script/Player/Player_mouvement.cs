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
    Vector3 move;

    public bool ForceLook;
    public NetworkObject target;
   

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        NetPlayer = GetComponent<Network_Player>();

    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) {  return; }    //pour bouger seulement l'obj du joueur

        GlobalMovement();
        GlobalLook();


    }


    #region movement
    private void GlobalMovement()
    {
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Runner.DeltaTime * NetPlayer.Speed;

        controller.Move(move);
    }
    #endregion

    #region looking
    private void GlobalLook()
    {
        if (!ForceLook)
        {
            if (NetPlayer.IsLooking)
            {
                Look.eulerAngles = new Vector3(0, NetPlayer.MousePos, 0);
                transform.rotation = Look;
            }

            if (!NetPlayer.IsLooking && move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
        else
        {
            Debug.Log("ForceLook ");
            gameObject.transform.LookAt(target.transform.position);
        }
    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    public void Rpc_BeScared(NetworkObject newtarget)
    {
        ForceLook = true;
        target = newtarget;
        StartCoroutine(DelayStopScared());
    }


    IEnumerator DelayStopScared()
    {
        yield return new WaitForSeconds(3);
        ForceLook = false;
        target = null;
    }

    #endregion

}
