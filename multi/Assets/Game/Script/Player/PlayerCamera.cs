using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Transform currentObstruction;

    public float Hauteur;
    Quaternion DefaultRotation;
    Vector3 DefaultAngle = new Vector3(35,0,0);
    Vector3 targetPosition;
    public Network_Player netplayer;
    Camera cam;
    private MeshRenderer currentMeshRenderer;
    private void Start()
    {
        currentObstruction = target;

        DefaultRotation.eulerAngles = DefaultAngle;
        transform.rotation = DefaultRotation;
        cam = this.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            targetPosition = target.position;
            transform.position = new Vector3(targetPosition.x, Hauteur, targetPosition.z - 5);
            ViewObstucted();
        }
    }  

    public float GetMousePos()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mousePositionRelativeToCenter = mousePosition - screenCenter;

        float Hauteur = mousePositionRelativeToCenter.y * 10000;
        float Largeur = mousePositionRelativeToCenter.x * 10000;

        float MouseAngleA = 90;
        if(Largeur < 0) 
        {
            MouseAngleA = -90;
        }
        float MouseAngleB = Mathf.Rad2Deg* Mathf.Atan(Hauteur / Largeur);
        float MouseAngleC = MouseAngleA - (MouseAngleB);

        return MouseAngleC;
    }

    void ViewObstucted()
    {
        RaycastHit hit;



        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, 4.5f))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                // Si un nouvel objet obstrue la vue
                if (currentObstruction != hit.transform)
                {
                    // Restaurer l'ancienne obstruction si elle existe
                    if (currentObstruction != null && currentMeshRenderer != null)
                    {
                        currentMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }

                    // Mettre à jour la nouvelle obstruction
                    currentObstruction = hit.transform;
                    currentMeshRenderer = currentObstruction.gameObject.GetComponent<MeshRenderer>();

                    // Si le nouvel objet obstruant a un MeshRenderer
                    if (currentMeshRenderer != null)
                    {
                        currentMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                    }
                }

                // Déplacement s'il y a une obstruction
                if (Vector3.Distance(currentObstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, target.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * 2f * Time.deltaTime);
                }
            }
            else
            {
                // Restaurer l'ancienne obstruction si elle existe
                if (currentObstruction != null && currentMeshRenderer != null)
                {
                    currentMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    currentObstruction = null;
                    currentMeshRenderer = null;
                }

                // Déplacement s'il n'y a pas d'obstruction
                if (Vector3.Distance(transform.position, target.position) < 4.5f)
                {
                    transform.Translate(Vector3.back * 2f * Time.deltaTime);
                }
            }
        }
        else
        {
            // Restaurer l'ancienne obstruction si elle existe
            if (currentObstruction != null && currentMeshRenderer != null)
            {
                currentMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                currentObstruction = null;
                currentMeshRenderer = null;
            }
        }
    }
}
    
