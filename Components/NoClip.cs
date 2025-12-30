using BepInEx;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Components
{
    internal class NoClip : MonoBehaviour
    {
        public float Speed;
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
            if (Player._mainPlayer._inChat)
                return;

            Speed = InputSystem.GetKey(Managers.NoClip.Instance.AlternativeSpeedKey) ? Managers.NoClip.Instance.AlternativeSpeed : Managers.NoClip.Instance.Speed;

            PositionShift = Vector3.zero;

            Player._mainPlayer._pMove.transform.rotation = Quaternion.Euler(0f, CameraFunction._current._RotY, 0f);

            if (InputSystem.GetKey(Managers.NoClip.Instance.Left))
                PositionShift -= Vector3.right * Speed * Time.deltaTime;
            else if (InputSystem.GetKey(Managers.NoClip.Instance.Right))
                PositionShift += Vector3.right * Speed * Time.deltaTime;

            if (InputSystem.GetKey(Managers.NoClip.Instance.Forward))
                PositionShift += Vector3.forward * Speed * Time.deltaTime;
            else if (InputSystem.GetKey(Managers.NoClip.Instance.Backward))
                PositionShift -= Vector3.forward * Speed * Time.deltaTime;

            if (InputSystem.GetKey(Managers.NoClip.Instance.Up))
                PositionShift += Vector3.up * Speed * Time.deltaTime;
            else if (InputSystem.GetKey(Managers.NoClip.Instance.Down))
                PositionShift += Vector3.down * Speed * Time.deltaTime;

            Player._mainPlayer._pMove.transform.Translate(PositionShift);
        }
#pragma warning restore IDE0051
    }
}