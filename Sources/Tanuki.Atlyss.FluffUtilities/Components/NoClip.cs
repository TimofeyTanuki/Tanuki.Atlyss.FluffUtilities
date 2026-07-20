using BepInEx;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Components;

internal sealed class NoClip : MonoBehaviour
{
    private static NoClip? instance;

    public KeyCode moveForward, moveRight, moveBackward, moveLeft, moveUp, moveDown, alternativeSpeedKey;
    public float speed, alternativeSpeed;
    private IInputSystem inputSystem = null!;

    private Vector3 positionShift;
    private float delta;

    private Player player = null!;
    private Transform playerTransform = null!;
    private CameraFunction cameraFunction = null!;
    private CameraCollision cameraCollision = null!;

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
        enabled = false;

        Types.Configuration.Sections.NoClip noClipSection = Configuration.Instance.NoClip;

        moveForward = noClipSection.Move_Forward.Value;
        moveRight = noClipSection.Move_Right.Value;
        moveBackward = noClipSection.Move_Backward.Value;
        moveLeft = noClipSection.Move_Left.Value;
        moveUp = noClipSection.Move_Up.Value;
        moveDown = noClipSection.Move_Down.Value;
        alternativeSpeedKey = noClipSection.AlternativeSpeedKey.Value;

        speed = noClipSection.Speed.Value;
        alternativeSpeed = noClipSection.AlternativeSpeed.Value;
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

        player = Player._mainPlayer;
        playerTransform = player.transform;
        cameraFunction = CameraFunction._current;
        cameraCollision = CameraCollision._current;
    }

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void OnEnable()
    {
        player._pMove._playerController.enabled = false;
        cameraCollision.enabled = false;

        Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix += PreventLocalPlayerControl;
    }

    [SuppressMessage("CodeQuality", "IDE0051")]
    private void OnDisable()
    {
        Game.Patches.PlayerMove.Client_LocalPlayerControl.OnPrefix -= PreventLocalPlayerControl;

        if (!player)
            return;

        player._pMove._airTime = 0;
        player._pMove._playerController.enabled = true;

        cameraCollision.enabled = true;
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
        if (player._inChat)
            return;

        delta = (inputSystem.GetKey(alternativeSpeedKey) ? alternativeSpeed : speed) * Time.deltaTime;
        positionShift = Vector3.zero;

        playerTransform.rotation = Quaternion.Euler(0f, cameraFunction._RotY, 0f);

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

        playerTransform.Translate(positionShift);
    }
}
