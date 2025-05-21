using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public Transform mainCamera;

    private void LateUpdate()
    {
        if (mainCamera != null)
            return;

        Vector3 position = mainCamera.localPosition;

        Vector3 localEuler = mainCamera.localEulerAngles;
        localEuler.x = 0f;
        localEuler.z = 0f;

        mainCamera.localPosition = position;
    }
}
