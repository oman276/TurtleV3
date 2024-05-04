using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private float shakeDuration = 0f;
    private float shakeAmplitude = 1.2f; // Amplitude of the shake. Higher values mean more shake.
    private float shakeFrequency = 1.5f; // Frequency of the shake. Higher values mean faster shake.

    public CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private CinemachineBrain brain;
    public NoiseSettings noiseProfile;

    void Start()
    {
        brain = GetComponent<CinemachineBrain>();
        /*
        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        */
    }

    void Update()
    {
        if (virtualCameraNoise)
        {
            if (shakeDuration > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;

                shakeDuration -= Time.deltaTime;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = 0f;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        virtualCamera = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        virtualCameraNoise = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        if (!virtualCameraNoise) {
            virtualCameraNoise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //var noiseProfile = Resources.Load<NoiseSettings>("6D Shake");
            //if (!noiseProfile) Debug.LogError("Didn't Work");
            virtualCameraNoise.m_NoiseProfile = noiseProfile;
        }
        shakeAmplitude = intensity;
        shakeDuration = time;
    }
}
