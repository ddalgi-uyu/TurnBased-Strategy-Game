using UnityEngine;
using Cinemachine;

public class SceneShake : MonoBehaviour
{
    public static SceneShake Instance {  get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.LogError("There is more than one SceneShake! " + transform + "-" + instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }

}
