using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class IA_Mouvement : NetworkBehaviour
{
    public float raduis = 2f;
    public float Speed = 2f;
    [SerializeField] Vector3 SpawnPoint;
    public IA_Detection Detection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnPoint = transform.position;
    }

    private void FixedUpdate()
    {
        bool IsChase = Detection.InChase;
        GameObject Target = Detection.target;

        Follow(IsChase, Target);
        RetourSpawn();
    }


    private void Follow(bool Chase,GameObject Target)
    {
        if(Target != null && Chase)
        {
            // ajoute vitesse en X/Y/Z (mais bouge que en x) donc marche aps
          //  rb.velocity = new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z)*Speed * Time.deltaTime;
            Debug.Log("CHASSSEEE");
        }
    }


    private void RetourSpawn()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = SpawnPoint;
        }
    }


 

}
