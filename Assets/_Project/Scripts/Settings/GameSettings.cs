using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    public event Action OnSettingsUpdated;

    [Header("Player Movement")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Camera & Controls")]
    [Range(1f, 500f)] public float mouseSensitivityX = 150f;
    [Range(1f, 500f)] public float mouseSensitivityY = 150f;
    public bool invertY = false;

    [Header("Camera Framing & Optics")]
    [Range(30f, 100f)] public float fieldOfView = 60f;
    [Range(1f, 20f)] public float cameraDistance = 2.5f;
    public Vector2 screenPosition = new Vector2(-0.15f, 0f);

    [Header("Camera Pivot (Shoulder Target)")]
    public Vector3 pivotOffset = new Vector3(0.6f, 1.5f, 0f);

    public float EffectiveGainX => mouseSensitivityX;
    public float EffectiveGainY => invertY ? -mouseSensitivityY : mouseSensitivityY;

    private void OnValidate()
    {
        OnSettingsUpdated?.Invoke();
    }

    [ContextMenu("Reset To Defaults")]
    public void ResetToDefaults()
    {
        // 1. Valores predefinidos de movimiento
        moveSpeed = 6f;
        rotationSpeed = 12f;
        gravity = -9.81f;
        jumpHeight = 1.5f;

        // 2. Valores predefinidos de controles
        mouseSensitivityX = 20f;
        mouseSensitivityY = 20f;
        invertY = true;

        // 3. Valores predefinidos de cámara
        fieldOfView = 60f;
        cameraDistance = 2.5f;
        screenPosition = new Vector2(-0.15f, 0f);
        pivotOffset = new Vector3(0.6f, 1.5f, 0f);

        // Dispara la actualización visual inmediata si estás en Play
        OnSettingsUpdated?.Invoke();
    }
}