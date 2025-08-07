using BepInEx;
using System;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Components;

internal class FreeCamera : MonoBehaviour
{
    public float Speed;
    public Vector3 Rotation;
    private Vector3 PositionShift;
    private IInputSystem InputSystem;

#pragma warning disable IDE0051
    private void Awake()
    {
        enabled = false;
        InputSystem = UnityInput.Current;
    }
    private void Update()
    {
        Rotation.x += InputControlManager.current._altVert_input * CameraFunction._current.inputSensitivity;
        Rotation.y += InputControlManager.current._altHoriz_input * CameraFunction._current.inputSensitivity;
        Rotation.x = Mathf.Clamp(Rotation.x, -89.99f, 89.99f);
        transform.rotation = Quaternion.Euler(Rotation);

        Speed += InputSystem.mouseScrollDelta.y * Managers.FreeCamera.Instance.ScrollSpeedStep;
        if (Speed < 0)
            Speed = 0;

        PositionShift = Vector3.zero;

        if (UnityInput.Current.GetKey(Managers.FreeCamera.Instance.Left))
            PositionShift -= transform.right * Speed * Time.deltaTime;
        else if (UnityInput.Current.GetKey(Managers.FreeCamera.Instance.Right))
            PositionShift += transform.right * Speed * Time.deltaTime;

        if (UnityInput.Current.GetKey(Managers.FreeCamera.Instance.Forward))
            PositionShift += transform.forward * Speed * Time.deltaTime;
        else if (UnityInput.Current.GetKey(Managers.FreeCamera.Instance.Backward))
            PositionShift -= transform.forward * Speed * Time.deltaTime;

        if (UnityInput.Current.GetKey(Managers.FreeCamera.Instance.Up))
            PositionShift += Vector3.up * Speed * Time.deltaTime;
        else if (UnityInput.Current.GetKey(Managers.FreeCamera.Instance.Down))
            PositionShift += Vector3.down * Speed * Time.deltaTime;

        transform.position += PositionShift;
    }
#pragma warning restore IDE0051
}