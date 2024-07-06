using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float Hauteur;
    Quaternion DefaultRotation;
    Vector3 DefaultAngle = new Vector3(35,0,0);
    Vector3 targetPosition;

    private void Start()
    {
        DefaultRotation.eulerAngles = DefaultAngle;
        transform.rotation = DefaultRotation;
    }

    private void LateUpdate()
    {

            targetPosition = target.position;
            transform.position = new Vector3(targetPosition.x,Hauteur,targetPosition.z-5);
    }  




}
