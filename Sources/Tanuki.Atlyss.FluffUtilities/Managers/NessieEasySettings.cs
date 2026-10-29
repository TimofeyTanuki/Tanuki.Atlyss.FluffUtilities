using Nessie.ATLYSS.EasySettings;
using Nessie.ATLYSS.EasySettings.UIElements;
using System;
using System.Collections.Generic;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

/// <summary>
/// Provides access to the Nessie Easy Settings integration (<see cref="BepInEx.BepInDependency.DependencyFlags.SoftDependency"/>).
/// </summary>
/// <remarks>
/// Must be initialized by calling <see cref="GetInstance"/> from <see cref="Main.Awake"/>.
/// </remarks>
internal sealed class NessieEasySettings
{
    public const string GUID = "EasySettings";
    private static NessieEasySettings? instance;

    public readonly bool IsAvailable = false;

    private readonly HashSet<Action> pendingActions = [];

    public event Action<SettingsTab>? OnModSettingsTabInitialized;

    private NessieEasySettings()
    {
        if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID))
            return;

        IsAvailable = true;
        Settings.OnInitialized.AddListener(OnInitialized);
        Settings.OnApplySettings.AddListener(OnApplySettings);
    }

    public static NessieEasySettings GetInstance()
    {
        instance ??= new();
        return instance;
    }


    /// <summary>
    /// Implements <see cref="AddPendingUpdate(ESettingsUpdate)"/> for classes:
    /// <list type="bullet">
    /// <item><see cref="AtlyssAdvancedSlider"/></item>
    /// <item><see cref="AtlyssToggle"/></item>
    /// <item><see cref="AtlyssKeyButton"/></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <see cref="Action"/> is always called only once, even if it's associated with multiple elements.
    /// </remarks>
    internal void AddPendingUpdateListener(BaseAtlyssElement element, Action action)
    {
        switch (element)
        {
            case AtlyssAdvancedSlider advancedSlider:
                advancedSlider.OnValueChanged.AddListener(_ => pendingActions.Add(action));
                break;
            case AtlyssToggle toggle:
                toggle.OnValueChanged.AddListener(_ => pendingActions.Add(action));
                break;
            case AtlyssKeyButton keyButton:
                keyButton.OnValueChanged.AddListener(_ => pendingActions.Add(action));
                break;
        }
    }

    private void OnInitialized()
    {
        SettingsTab settingsTab = Settings.GetOrAddCustomTab(PluginInfo.NAME);

        OnModSettingsTabInitialized?.Invoke(settingsTab);
    }

    private void OnApplySettings()
    {

        if (pendingActions.Count == 0)
            return;

        Action[] actions = [.. pendingActions];
        pendingActions.Clear();

        foreach (Action action in actions)
            action();
    }
}
