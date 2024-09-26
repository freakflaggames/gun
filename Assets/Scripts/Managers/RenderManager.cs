using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.ShaderData;

public class RenderManager : MonoBehaviour
{
    [SerializeField] FullScreenPassRendererFeature fullscreenRend;
    [SerializeField] Volume globalVolume;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (fullscreenRend.isActive && globalVolume.weight > 0f)
            {
                fullscreenRend.SetActive(false);
                globalVolume.weight = 0f;
            }   
            else
            {
                fullscreenRend.SetActive(true);
                globalVolume.weight = 1f;
            }
        }
    }
}
