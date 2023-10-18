using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCemera;


    [SerializeField] [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField] [Range(0.1f, 1f)] float duration = 0.5f;

    public void Shake()
    {
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = mainCemera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCemera.transform.position = cameraPos;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");

    }
}

