using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _3DCamera;
    [SerializeField] private CinemachineCamera _2DCamera;
    [HideInInspector] public bool is2DCamera = false;
    public void Switch()
    {
        is2DCamera = !is2DCamera;
        if (is2DCamera)
        {
            _2DCamera.Priority = 1;
            _3DCamera.Priority = 0;
        }
        else
        {
            _2DCamera.Priority = 0;
            _3DCamera.Priority = 1;

        }
    }
}
