using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanuki.Atlyss.FluffUtilities.Managers;

internal class Hotkey : MonoBehaviour
{
    private class HotkeyActionGroup(KeyCode KeyCode) : IComparable<HotkeyActionGroup>
    {
        public KeyCode KeyCode = KeyCode;
        public HashSet<Action> Actions = [];

        public int CompareTo(HotkeyActionGroup HotkeyActionGroup) =>
            KeyCode.CompareTo(HotkeyActionGroup.KeyCode);
    }

    public static Hotkey Instance;
    private readonly SortedSet<HotkeyActionGroup> HotkeyActionGroups;
    private readonly IInputSystem InputSystem;

    private Hotkey()
    {
        HotkeyActionGroups = [];
        InputSystem = UnityInput.Current;
    }

    public static void Initialize()
    {
        if (Instance is not null)
            return;

        Instance = Main.Instance.gameObject.AddComponent<Hotkey>();
        DontDestroyOnLoad(Instance);
    }
    public void BindAction(KeyCode KeyCode, Action Action)
    {
        if (KeyCode is KeyCode.None)
            return;

        HotkeyActionGroup HotkeyActionGroup = HotkeyActionGroups.Where(x => x.KeyCode == KeyCode).SingleOrDefault();
        if (HotkeyActionGroup is null)
        {
            HotkeyActionGroup = new(KeyCode);
            HotkeyActionGroups.Add(HotkeyActionGroup);
        }

        HotkeyActionGroup.Actions.Add(Action);
        enabled = true;
    }
    public void UnbindAction(Action Action)
    {
        List<HotkeyActionGroup> HotkeyActionGroupsToRemove = [];
        foreach (HotkeyActionGroup HotkeyActionGroup in HotkeyActionGroups)
        {
            HotkeyActionGroup.Actions.Remove(Action);

            if (HotkeyActionGroup.Actions.Count > 0)
                continue;

            HotkeyActionGroupsToRemove.Add(HotkeyActionGroup);
        }
        HotkeyActionGroupsToRemove.ForEach(x => HotkeyActionGroups.Remove(x));
    }
    public void UnbindAction(KeyCode KeyCode, Action Action)
    {
        HotkeyActionGroup HotkeyActionGroup = HotkeyActionGroups.Where(x => x.KeyCode == KeyCode).SingleOrDefault();
        if (HotkeyActionGroup is null)
            return;

        HotkeyActionGroup.Actions.Remove(Action);

        if (HotkeyActionGroup.Actions.Count == 0)
            HotkeyActionGroups.Remove(HotkeyActionGroup);
    }
    public void Reset()
    {
        enabled = false;
        HotkeyActionGroups.Clear();
    }

#pragma warning disable IDE0051
    private void Awake() => enabled = false;
    private void Update()
    {
        foreach (HotkeyActionGroup HotkeyActionGroup in HotkeyActionGroups)
        {
            if (!InputSystem.GetKeyDown(HotkeyActionGroup.KeyCode))
                continue;

            foreach (Action Action in HotkeyActionGroup.Actions)
                Action.Invoke();
        }
    }
#pragma warning restore IDE0051
}