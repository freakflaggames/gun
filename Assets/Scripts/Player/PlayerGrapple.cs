using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerGrapple : MonoBehaviour
{
    PlayerShooting shooting;

    public LayerMask grappleTarget;

    public LineRenderer grappleLine;

    public float reelSpeed;

    public GameObject grappledObject;

    public GameObject lassoObject;

    bool hasStartedGrapple;

    private void Awake()
    {
        shooting = GetComponent<PlayerShooting>();
    }
    private void Update()
    {

        if (grappledObject)
        {
            if (Vector3.Distance(grappledObject.transform.position, transform.position) > 2)
            {
                Vector3 diff = (transform.position - grappledObject.transform.position).normalized;
                diff.y = 0;
                print(diff);
                grappledObject.GetComponent<Rigidbody>().velocity = diff * reelSpeed;
                grappleLine.SetPosition(0, grappleLine.transform.position);
                grappleLine.SetPosition(1, grappledObject.transform.position);
            }
            else
            {
                grappledObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                grappledObject = null;
                ResetGrapple();
            }
        }
        if (!grappledObject && hasStartedGrapple)
        {
            ResetGrapple();
        }

    }
    public void ResetGrapple()
    {
        grappleLine.SetPosition(0, grappleLine.transform.position);
        grappleLine.SetPosition(1, grappleLine.transform.position);
        hasStartedGrapple = false;
        shooting.gunObject.SetActive(true);
        shooting.enabled = true;
    }
    public void StartGrapple(InputAction.CallbackContext context)
    {
        shooting.gunObject.SetActive(false);
        shooting.enabled = false;
        lassoObject.SetActive(true);
    }
    public void EndGrapple(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, float.MaxValue, grappleTarget))
        {
            grappledObject = hit.collider.gameObject;

            grappleLine.SetPosition(0, grappleLine.transform.position);
            grappleLine.SetPosition(1, grappledObject.transform.position);

            hasStartedGrapple = true;
        }
        lassoObject.SetActive(false);
    }
}
