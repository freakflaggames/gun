using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles bullet impact position and rotation.
//It should be housed on the Bullet prefab object

public class VFXBulletImpact : MonoBehaviour
{
    //Rotate and move the impact to the hit position provided by Bullet
    public void Initialize(RaycastHit hit)
    {
        //put the impact vfx on the hit point of the cast, then move it away from the face its on
        //using the mesh's normal direction (direction outward of face)
        Vector3 impactPos = hit.point + (hit.normal * 0.1f);

        //rotate the impact vfx towards the normal direction, using the hit object's relative upward position
        Quaternion rotation = Quaternion.LookRotation(hit.normal, hit.transform.up);

        //after parenting to object, then set position and rotation
        transform.position = impactPos;
        transform.rotation = rotation;
    }
}
