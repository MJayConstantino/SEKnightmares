using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera bossCamera;

    [Header("Camera Settings")]
    [SerializeField] private float bossViewSize = 20f;

    private void Start()
    {
        if (playerCamera) playerCamera.Priority = 10;
        if (bossCamera)
        {
            bossCamera.Priority = 0;
            bossCamera.m_Lens.OrthographicSize = bossViewSize;
        }
    }

    public void SwitchToBossCamera()
    {
        if (playerCamera) playerCamera.Priority = 0;
        if (bossCamera) bossCamera.Priority = 10;
    }

    public void SwitchToPlayerCamera()
    {
        if (playerCamera) playerCamera.Priority = 10;
        if (bossCamera) bossCamera.Priority = 0;
    }
}