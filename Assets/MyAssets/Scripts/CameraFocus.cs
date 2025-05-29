using Unity.Cinemachine;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public CinemachineBrain Brain;
    public ICinemachineCamera CamA;
    public ICinemachineCamera CamB;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CamA = GetComponent<CinemachineCamera>();
        CamB = GetComponent<CinemachineCamera>();

        int layer = 1;
        int priority = 1;
        float weight = 1f;
        float blendTime = 0f;
        Brain.SetCameraOverride(layer,priority, CamA, CamB, weight, blendTime);
    }
}
