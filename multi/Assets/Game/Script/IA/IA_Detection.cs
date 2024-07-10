using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IA_Detection : MonoBehaviour
{
    public SphereCollider Spherecollider;

    public List<GameObject> TargetList;

    public float Radius = 4;
    public GameObject NewTarget = null;
    public GameObject CurrentTarget = null;
    public bool InChase;

    private void Start()
    {
        Spherecollider = GetComponent<SphereCollider>();
        Spherecollider.radius = Radius;
        Spherecollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NewTarget = other.gameObject;
            ListAdd(NewTarget);
            InChase = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            NewTarget = other.gameObject;
            ListRemover(NewTarget);
            
        }
    }

    void ListAdd(GameObject Target)
    {
        TargetList.Add(Target);
        CurrentTarget = TargetList[0];
    }
    void ListRemover(GameObject Target)
    {
        TargetList.Remove(Target);
        if(TargetList.Count >= 0 )
        {   
            CurrentTarget = ListCloser(TargetList);
        }
        
        
    }

    private GameObject ListCloser(List<GameObject> List)
    {
        float ClosestDistance = float.PositiveInfinity;
        GameObject ClosestObj = null;

        foreach (GameObject Target in List)
        {
            float CurrentDistance = Vector3.Distance(transform.position,Target.transform.position);
            
            if(CurrentDistance < ClosestDistance) 
            { 
                ClosestDistance = CurrentDistance;
                ClosestObj = Target;
            }

        } 

        if( ClosestObj != null )
        {
            return ClosestObj;
        }
        else
        {
            InChase = false;
            return null;
        }
       
    }
}
