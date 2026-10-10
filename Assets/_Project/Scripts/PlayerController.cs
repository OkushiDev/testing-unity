using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameSettings settings;

    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 velocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (settings == null) return;

        // 1. Reset de gravedad en suelo
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Entrada de teclado
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
        }

        Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * inputDirection.z + camRight * inputDirection.x).normalized;

            controller.Move(moveDirection * settings.moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, settings.rotationSpeed * Time.deltaTime);
        }

        // 3. Salto
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(settings.jumpHeight * -2f * settings.gravity);
        }

        // 4. Gravedad
        velocity.y += settings.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        // 1. Mirada del personaje (Línea azul)
        Gizmos.color = Color.blue;
        Vector3 playerEyePos = transform.position + Vector3.up * 1f;
        Gizmos.DrawRay(playerEyePos, transform.forward * 3f);

        // 2. Mirada de la cámara (Línea roja y punto de impacto)
        if (Camera.main != null)
        {
            Transform cam = Camera.main.transform;
            Gizmos.color = Color.red;

            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 100f))
            {
                Gizmos.DrawLine(cam.position, hit.point);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hit.point, 0.15f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(playerEyePos, hit.point);
            }
            else
            {
                Gizmos.DrawRay(cam.position, cam.forward * 50f);
            }
        }
    }
}