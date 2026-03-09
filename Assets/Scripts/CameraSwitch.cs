using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _3DCamera;
    [SerializeField] private CinemachineCamera _2DCamera;
    public void Switch()
    {
        if (_3DCamera.Priority > _2DCamera.Priority)
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
