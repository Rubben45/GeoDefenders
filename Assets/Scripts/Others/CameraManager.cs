using Sirenix.OdinInspector;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [TabGroup("CameraMetrics", TextColor = "green", Icon = SdfIconType.BarChartFill)]
    [SerializeField] private float cameraMoveSpeed = 20f;
    [TabGroup("CameraMetrics")]
    [SerializeField] private float cameraRotationSpeed = 20f;
    [TabGroup("CameraMetrics")]
    [SerializeField] private float cameraZoomSpeed = 10f;
    [TabGroup("CameraMetrics")]
    [SerializeField] private float maxFieldOfView = 50;
    [TabGroup("CameraMetrics")]
    [SerializeField] private float minFieldOfView = 10;
    [TabGroup("CameraMetrics")]
    [SerializeField] private float followOffsetMaxY = 50f;
    [TabGroup("CameraMetrics")]
    [SerializeField] private float followOffsetMinY = 10f;
    [TabGroup("CameraMetrics")]
    [SerializeField] private float zoomStepSize = 5;

    [TabGroup("CameraKeyBinds", TextColor = "red", Icon = SdfIconType.ArrowsMove)]
    [SerializeField] private KeyCode moveForwardKey = KeyCode.W;
    [TabGroup("CameraKeyBinds")]
    [SerializeField] private KeyCode moveBackwardKey = KeyCode.S;
    [TabGroup("CameraKeyBinds")]
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [TabGroup("CameraKeyBinds")]
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;

    [TabGroup("CameraKeyBinds")]
    [SerializeField] private KeyCode rotateRightKey = KeyCode.Q;
    [TabGroup("CameraKeyBinds")]
    [SerializeField] private KeyCode rotateLeftKey = KeyCode.E;

    private Vector3 movementInput;
    private Vector3 followOffset;
    private float rotationInput;
    private float mouseScrollInput;
    private float currentFieldOfView = 50;


    private void Awake()
    {
        followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    void Update()
    {
        HandlePlayerInput();
        HandleCameraMovement();
        HandleCameraRotation();
        HandleCameraZoon_LowerY();
        HandleCameraZoom();
    }

    private void HandlePlayerInput()
    {
        movementInput = Vector3.zero;
        rotationInput = 0f;

        if (Input.GetKey(moveForwardKey)) movementInput.z = 1f;
        if (Input.GetKey(moveBackwardKey)) movementInput.z = -1f;
        if (Input.GetKey(moveRightKey)) movementInput.x = 1f;
        if (Input.GetKey(moveLeftKey)) movementInput.x = -1f;

        if (Input.GetKey(rotateRightKey)) rotationInput = 1f;
        if (Input.GetKey(rotateLeftKey)) rotationInput = -1f;

        mouseScrollInput = Input.mouseScrollDelta.y;
    }

    private void HandleCameraMovement()
    {
        Vector3 moveDir = transform.forward * movementInput.z + transform.right * movementInput.x;
        transform.position += cameraMoveSpeed * Time.deltaTime * moveDir;
    }

    private void HandleCameraRotation()
    {
        Vector3 rotationDir = new(0f, rotationInput * cameraRotationSpeed * Time.deltaTime, 0f);
        transform.eulerAngles += rotationDir;
    }

    private void HandleCameraZoom()
    {
        currentFieldOfView += (mouseScrollInput > 0 ? -zoomStepSize : mouseScrollInput < 0 ? zoomStepSize : 0);
        currentFieldOfView = Mathf.Clamp(currentFieldOfView, minFieldOfView, maxFieldOfView);

        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, currentFieldOfView, cameraZoomSpeed * Time.deltaTime);
    }

    private void HandleCameraZoon_LowerY()
    {
        followOffset.y += (mouseScrollInput > 0 ? -zoomStepSize : mouseScrollInput < 0 ? zoomStepSize : 0);

        followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);

        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * cameraZoomSpeed);
    }
}
