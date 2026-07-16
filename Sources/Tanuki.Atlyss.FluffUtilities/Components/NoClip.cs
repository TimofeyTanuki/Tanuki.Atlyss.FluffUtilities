using BepInEx;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Components;

internal sealed class NoClip : MonoBehaviour
{
    private static NoClip? instance;

    public KeyCode Forward, Right, Backward, Left, Up, Down, AlternativeSpeedKey;
    public float BaseSpeed, AlternativeBaseSpeed;
    private IInputSystem InputSystem = null!;

    private Vector3 positionShift;
    private float delta;

    private Player player = null!;
    private Transform playerTransform = null!;
    private CameraFunction cameraFunction = null!;

    public static NoClip? GetOrCreate()
    {
        if (instance != null)
            return instance;

        if (!Player._mainPlayer)
            return null;

        return Player._mainPlayer.gameObject.AddComponent<NoClip>();
    }

    public void Reconfigure()
    {
        Types.Configuration.Sections.NoClip noClipSection = Configuration.Instance.NoClip;

        Forward = noClipSection.Move_Forward.Value;
        Right = noClipSection.Move_Right.Value;
        Backward = noClipSection.Move_Backward.Value;
        Left = noClipSection.Move_Left.Value;
        Up = noClipSection.Move_Up.Value;
        Down = noClipSection.Move_Down.Value;
        AlternativeSpeedKey = noClipSection.AlternativeSpeedKey.Value;

        BaseSpeed = noClipSection.BaseSpeed.Value;
        AlternativeBaseSpeed = noClipSection.AlternativeBaseSpeed.Value;
    }

    private void PreventLocalPlayerControl(PlayerMove instance, ref bool runOriginal)
    {
        if (!instance.isLocalPlayer)
            return;

        runOriginal = false;
    }

    private void OnStopClient() => Destroy(this);

#pragma warning disable IDE0051
    private void Start() => InputSystem = UnityInput.Current;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        enabled = false;

        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix += OnStopClient;

        player = Player._mainPlayer;
        playerTransform = player.transform;
        cameraFunction = CameraFunction._current;

        Reconfigure();
    }


    private void OnEnable()
    {
        player._pMove._playerController.enabled = false;
        CameraCollision._current.enabled = false;

        Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix += PreventLocalPlayerControl;
    }

    private void OnDisable()
    {
        Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix -= PreventLocalPlayerControl;

        if (!player)
            return;

        player._pMove._airTime = 0;
        player._pMove._playerController.enabled = true;

        CameraCollision._current.enabled = true;
    }

    private void OnDestroy()
    {
        Game.Patches.AtlyssNetworkManager.OnStopClient.OnPrefix -= OnStopClient;
        instance = null;
    }

    private void Update()
    {
        if (player._inChat)
            return;

        delta = (InputSystem.GetKey(AlternativeSpeedKey) ? AlternativeBaseSpeed : BaseSpeed) * Time.deltaTime;
        positionShift = Vector3.zero;

        playerTransform.rotation = Quaternion.Euler(0f, cameraFunction._RotY, 0f);

        if (InputSystem.GetKey(Left))
            positionShift += Vector3.left * delta;
        else if (InputSystem.GetKey(Right))
            positionShift += Vector3.right * delta;

        if (InputSystem.GetKey(Forward))
            positionShift += Vector3.forward * delta;
        else if (InputSystem.GetKey(Backward))
            positionShift += Vector3.back * delta;

        if (InputSystem.GetKey(Up))
            positionShift += Vector3.up * delta;
        else if (InputSystem.GetKey(Down))
            positionShift += Vector3.down * delta;

        playerTransform.Translate(positionShift);
    }
#pragma warning restore IDE0051
}
