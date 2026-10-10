using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
[RequireComponent(typeof(CinemachineInputAxisController))]
public class CinemachineSettingsBinder : MonoBehaviour
{
    [SerializeField] private GameSettings settings;

    private CinemachineCamera vcam;
    private CinemachineInputAxisController axisController;
    private CinemachineOrbitalFollow orbitalFollow;
    private CinemachineRotationComposer rotationComposer;

    private void Awake()
    {
        CacheComponents();
        ApplySettings();
    }

    private void OnEnable()
    {
        if (settings != null)
        {
            settings.OnSettingsUpdated += ApplySettings;
        }
    }

    private void OnDisable()
    {
        if (settings != null)
        {
            settings.OnSettingsUpdated -= ApplySettings;
        }
    }

    private void CacheComponents()
    {
        if (vcam == null) vcam = GetComponent<CinemachineCamera>();
        if (axisController == null) axisController = GetComponent<CinemachineInputAxisController>();
        if (orbitalFollow == null) orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        if (rotationComposer == null) rotationComposer = GetComponent<CinemachineRotationComposer>();
    }

    public void ApplySettings()
    {
        if (settings == null) return;
        CacheComponents();

        // 1. Sensibilidad
        if (axisController != null)
        {
            if (axisController.Controllers.Count > 0)
            {
                var c0 = axisController.Controllers[0];
                c0.Input.Gain = settings.EffectiveGainX;
                axisController.Controllers[0] = c0;
            }

            if (axisController.Controllers.Count > 1)
            {
                var c1 = axisController.Controllers[1];
                c1.Input.Gain = settings.EffectiveGainY;
                axisController.Controllers[1] = c1;
            }
        }

        // 2. FOV
        if (vcam != null)
        {
            vcam.Lens.FieldOfView = settings.fieldOfView;
        }

        // 3. Distancia (Radius)
        if (orbitalFollow != null)
        {
            orbitalFollow.Radius = settings.cameraDistance;
        }

        // 4. Encuadre (Screen Position)
        if (rotationComposer != null)
        {
            rotationComposer.Composition.ScreenPosition = settings.screenPosition;
        }
    }
}