using BepInEx;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Components;

internal sealed class NoClip : MonoBehaviour
{
    public KeyCode Forward, Right, Backward, Left, Up, Down, AlternativeSpeedKey;
    public float BaseSpeed, AlternativeBaseSpeed;
    public IInputSystem InputSystem = UnityInput.Current;

    private Vector3 positionShift;
    private float currentSpeed;

#pragma warning disable IDE0051
    private void Awake() => enabled = false;

    private void Update()
    {
        if (Player._mainPlayer._inChat)
            return;

        currentSpeed = InputSystem.GetKey(AlternativeSpeedKey) ? AlternativeBaseSpeed : BaseSpeed;
        positionShift = Vector3.zero;

        Player._mainPlayer._pMove.transform.rotation = Quaternion.Euler(0f, CameraFunction._current._RotY, 0f);

        if (InputSystem.GetKey(Left))
            positionShift += Vector3.left * currentSpeed * Time.deltaTime;
        else if (InputSystem.GetKey(Right))
            positionShift += Vector3.right * currentSpeed * Time.deltaTime;

        if (InputSystem.GetKey(Forward))
            positionShift += Vector3.forward * currentSpeed * Time.deltaTime;
        else if (InputSystem.GetKey(Backward))
            positionShift += Vector3.back * currentSpeed * Time.deltaTime;

        if (InputSystem.GetKey(Up))
            positionShift += Vector3.up * currentSpeed * Time.deltaTime;
        else if (InputSystem.GetKey(Down))
            positionShift += Vector3.down * currentSpeed * Time.deltaTime;

        Player._mainPlayer._pMove.transform.Translate(positionShift);
    }
#pragma warning restore IDE0051
}
