using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Detection : MonoBehaviour
{
    public SphereCollider collider;
    public float Radius = 4;
    public GameObject target;
    public bool InChase;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = Radius;
        collider.isTrigger = true;
    }

    private void Update()
    {
        Debug.Log("Inchase = " + InChase);
        Debug.Log("target = " + target);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
            InChase = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
        InChase = false;
    }

}
