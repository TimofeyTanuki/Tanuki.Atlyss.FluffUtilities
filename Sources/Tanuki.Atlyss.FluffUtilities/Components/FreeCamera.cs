using BepInEx;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Components;

internal sealed class FreeCamera : MonoBehaviour
{
    private static FreeCamera? instance;

    private IInputSystem inputSystem = null!;
    private CameraFunction cameraFunction = null!;
    private CameraCollision cameraCollision = null!;
    private InputControlManager inputControlManager = null!;

    public KeyCode moveForward, moveRight, moveBackward, moveLeft, moveUp, moveDown;
    private float speedAdjustmentScrollMultiplier;
    private bool lockCharacterControl;

    private bool smoothLook = false;
    private float smoothLookInterpolation = 0;

    private Quaternion targetRotation;
    private Vector3 positionShift;
    private float rotationShiftX, rotationShiftY, rotationX, rotationY, speed, delta;

    public static FreeCamera? GetOrCreate()
    {
        if (instance != null)
            return instance;

        if (!CameraFunction._current)
            return null;

        Camera mainCamera = CameraFunction._current._mainCamera;

        if (!mainCamera.enabled)
            return null;

        return mainCamera.gameObject.AddComponent<FreeCamera>();
    }

    public void Reconfigure()
    {
        enabled = false;

        Types.Configuration.Sections.FreeCamera freeCameraSection = Configuration.Instance.FreeCamera;

        speed = freeCameraSection.BaseSpeed.Value;

        smoothLook = freeCameraSection.SmoothLook.Value;
        smoothLookInterpolation = freeCameraSection.SmoothLook_Interpolation.Value;

        speedAdjustmentScrollMultiplier = freeCameraSection.SpeedAdjustmentScrollMultiplier.Value;
        lockCharacterControl = freeCameraSection.LockCharacterControls.Value;

        moveForward = freeCameraSection.Move_Forward.Value;
        moveRight = freeCameraSection.Move_Right.Value;
        moveBackward = freeCameraSection.Move_Backward.Value;
        moveLeft = freeCameraSection.Move_Left.Value;
        moveUp = freeCameraSection.Move_Up.Value;
        moveDown = freeCameraSection.Move_Down.Value;
    }

    private void HandleControls()
    {
        if (inputSystem.mouseScrollDelta.y == 0)
            return;

        speed += inputSystem.mouseScrollDelta.y * speedAdjustmentScrollMultiplier;

        if (speed < 0.001)
            speed = 0.001f;
    }

    private void HandleRotation()
    {
        rotationShiftX = inputControlManager._altVert_input * cameraFunction.inputSensitivity;
        rotationShiftY = inputControlManager._altHoriz_input * cameraFunction.inputSensitivity;

        if (Vector3.Dot(transform.up, Vector3.up) < 0)
            rotationShiftY = -rotationShiftY;

        rotationX += rotationShiftX;
        rotationY += rotationShiftY;

        targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = smoothLook ? Quaternion.Slerp(transform.rotation, targetRotation, smoothLookInterpolation * Time.deltaTime) : targetRotation;
    }

    private void HandlePosition()
    {
        delta = speed * Time.deltaTime;
        positionShift = Vector3.zero;

        if (inputSystem.GetKey(moveLeft))
            positionShift += Vector3.left * delta;
        else if (inputSystem.GetKey(moveRight))
            positionShift += Vector3.right * delta;

        if (inputSystem.GetKey(moveForward))
            positionShift += Vector3.forward * delta;
        else if (inputSystem.GetKey(moveBackward))
            positionShift += Vector3.back * delta;

        if (inputSystem.GetKey(moveUp))
            positionShift += Vector3.up * delta;
        else if (inputSystem.GetKey(moveDown))
            positionShift += Vector3.down * delta;

        transform.Translate(positionShift);
    }

    private void PreventLocalPlayerControl(PlayerMove instance, ref bool runOriginal)
    {
        if (!instance.isLocalPlayer)
            return;

        runOriginal = false;
    }

    private void OnStopClient() => Destroy(this);

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void Start() => inputSystem = UnityInput.Current;

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        Reconfigure();

        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;

        cameraFunction = CameraFunction._current;
        cameraCollision = CameraCollision._current;
        inputControlManager = InputControlManager.current;
    }

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void OnEnable()
    {
        rotationX = CameraFunction._current._RotX;
        rotationY = CameraFunction._current._RotY;

        cameraFunction.enabled = false;
        cameraCollision.enabled = false;

        if (lockCharacterControl)
            Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix += PreventLocalPlayerControl;
    }

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void OnDisable()
    {
        if (lockCharacterControl)
            Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix -= PreventLocalPlayerControl;

        if (!cameraFunction)
            return;

        cameraCollision.transform.localPosition = cameraCollision.dollyDir * cameraCollision.distance;

        cameraFunction.enabled = true;
        cameraCollision.enabled = true;

        cameraFunction._mainCamera.transform.rotation = cameraFunction.transform.rotation;
        cameraFunction.CameraReset_Lerp();
    }

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void OnDestroy()
    {
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix -= OnStopClient;
        instance = null;
    }

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void Update()
    {
        HandleControls();
        HandleRotation();
        HandlePosition();
    }
}
