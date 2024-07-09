using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{

    public bool canGrapple;


    public float GrabDelay;

    float lastGrabTime;


    [SerializeField]
    LayerMask TargetMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        FPSController.onGrapple += startGrapple;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startGrapple()
    {
        bool canGrapple = lastGrabTime + GrabDelay < Time.time;

        if (canGrapple)
        {
            Transform camera = Camera.main.transform;

            Vector3 direction = camera.forward;
            float maxDistance = float.MaxValue;

            bool hitTarget = Physics.Raycast(camera.position, direction, out RaycastHit hit, maxDistance, TargetMask);

            if (hitTarget)
            {
                
            }
        }
    }


    IEnumerator GrabEnemy()
    {
        yield return new WaitForSeconds(1);
    }
}
