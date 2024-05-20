using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPlayer : MonoBehaviour {
    [SerializeField] float step;
    [SerializeField] GameObject VRCamera;

    public void CameraUp() {
        MoveCameraY(step);
    }
    public void CameraDown() {
        MoveCameraY(-step);
    }

    private void MoveCameraY(float stepY) {
        if (VRCamera != null) {
            Vector3 newPosition = VRCamera.transform.position;
            newPosition.y += stepY;
            VRCamera.transform.position = newPosition;
        }
        else {
            Debug.Log("Missing Camera assigned to UpDownPlayer");
        }
    }
}
