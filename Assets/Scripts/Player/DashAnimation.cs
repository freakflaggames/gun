using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAnimation : MonoBehaviour
{
    [SerializeField]
    ParticleSystem DashParticles;
    private void OnEnable()
    {
        FPSController.onDashed += Dash;
    }
    public void Dash(Vector3 direction)
    {
        direction = direction.normalized;

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
        Vector3 eulerAngles = Quaternion.AngleAxis(-angle, Vector3.up).eulerAngles;

        var shape = DashParticles.shape;

        Vector3 position = Vector3.zero;
        position.x = Mathf.Round(direction.x);
        position.y = 1;

        shape.position = position;
        shape.rotation = eulerAngles;

        DashParticles.Play();
    }
    private void OnDisable()
    {
        FPSController.onDashed -= Dash;
    }
}
