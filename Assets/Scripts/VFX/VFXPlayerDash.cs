using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles the particle animation for when the player dashes
//It should be housed in the "Dash" child object of the "Particles" child object of the player prefab object
//It communicates with the Player controller through listening to its events

[RequireComponent(typeof(ParticleSystem))]
public class VFXPlayerDash : MonoBehaviour
{
    public ParticleSystem DashParticles;
    private void Awake()
    {
        DashParticles = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        //Listen to playercontroller events to see if dash has been performed
        PlayerController.onDashed += Dash;
    }
    public void Dash(Vector3 direction)
    {
        //Normalize direction so orthographic and diagonal directions return same magnitude
        direction = direction.normalized;

        //Find angle of direction using Atan2 equation
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
        //make forward rotation of dash particles equal to the angle of the direction 
        Vector3 eulerAngles = Quaternion.AngleAxis(-angle, Vector3.up).eulerAngles;

        //Get dash particle system shape 
        var shape = DashParticles.shape;

        //Move particle shape position based on direction
        Vector3 position = Vector3.zero;
        position.x = Mathf.Round(direction.x);
        position.y = 1;

        //Rotate and move particle shape to match direction
        shape.position = position;
        shape.rotation = eulerAngles;

        //Trigger particle animation
        DashParticles.Play();
    }
    private void OnDisable()
    {
        PlayerController.onDashed -= Dash;
    }
}
