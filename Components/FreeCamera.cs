using BepInEx;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Components;

internal class FreeCamera : MonoBehaviour
{
    private IInputSystem InputSystem;
    private bool IsUpsideDown;
    private float
        RotationShiftX,
        RotationShiftY,
        RotationX,
        RotationY;
    private Quaternion TargetRotation;
    public float Speed = 0;
    public bool SmoothLookMode = false;
    public float SmoothLookModeInterpolation = 0;

#pragma warning disable IDE0051
    private void Awake()
    {
        enabled = false;
        InputSystem = UnityInput.Current;
    }

    private void OnEnable()
    {
        RotationX = CameraFunction._current._RotX;
        RotationY = CameraFunction._current._RotY;
    }
    private void OnDisable()
    {
        CameraFunction._current._mainCamera.transform.rotation = CameraFunction._current.transform.rotation;
        CameraFunction._current.CameraReset_Lerp();
    }

    private void Update()
    {
        HandleControl();
        HandleRotation();
        HandlePosition();
    }
#pragma warning restore IDE0051

    private void HandleControl()
    {
        if (InputSystem.mouseScrollDelta.y == 0)
            return;

        Speed += InputSystem.mouseScrollDelta.y * Managers.FreeCamera.Instance.ScrollSpeedAdjustmentStep;
        if (Speed < 0.01)
            Speed = 0.01f;
    }
    private void HandleRotation()
    {
        RotationShiftX = InputControlManager.current._altVert_input * CameraFunction._current.inputSensitivity;
        RotationShiftY = InputControlManager.current._altHoriz_input * CameraFunction._current.inputSensitivity;

        IsUpsideDown = Vector3.Dot(transform.up, Vector3.up) < 0;

        if (IsUpsideDown)
            RotationShiftY = -RotationShiftY;

        RotationX += RotationShiftX;
        RotationY += RotationShiftY;

        TargetRotation = Quaternion.Euler(RotationX, RotationY, 0);
        transform.rotation = SmoothLookMode ? Quaternion.Slerp(transform.rotation, TargetRotation, SmoothLookModeInterpolation * Time.deltaTime) : TargetRotation;
    }
    private void HandlePosition()
    {
        if (InputSystem.GetKey(Managers.FreeCamera.Instance.Left))
            transform.position -= transform.right * Speed * Time.deltaTime;
        else if (InputSystem.GetKey(Managers.FreeCamera.Instance.Right))
            transform.position += transform.right * Speed * Time.deltaTime;

        if (InputSystem.GetKey(Managers.FreeCamera.Instance.Forward))
            transform.position += transform.forward * Speed * Time.deltaTime;
        else if (InputSystem.GetKey(Managers.FreeCamera.Instance.Backward))
            transform.position -= transform.forward * Speed * Time.deltaTime;

        if (InputSystem.GetKey(Managers.FreeCamera.Instance.Up))
            transform.position += Vector3.up * Speed * Time.deltaTime;
        else if (InputSystem.GetKey(Managers.FreeCamera.Instance.Down))
            transform.position += Vector3.down * Speed * Time.deltaTime;
    }
}