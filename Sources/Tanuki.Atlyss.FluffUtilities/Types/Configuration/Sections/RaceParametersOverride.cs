using BepInEx.Configuration;
using System.Collections.Generic;

namespace Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

internal sealed class RaceParametersOverride
{
    private const string SECTION_NAME = "RaceParametersOverride";
    private const string ITEM_SECTION_NAME_BASE = "RaceParametersOverrideItem";
    private const string ITEM_SECTION_NAME_TEMPLATE = ITEM_SECTION_NAME_BASE + "_{0}";

    private readonly ConfigFile configFile;

    private IReadOnlyDictionary<string, ScriptablePlayerRace> CachedRaces => Game.Accessors.GameManager._cachedScriptableRaces(GameManager._current);

    public ConfigEntry<bool> SeparateOverridesForEachRace;

    public RaceParametersOverrideItem BaseOverrideItem;

    public Dictionary<string, RaceParametersOverrideItem> OverrideItems = [];

    public RaceParametersOverride(ConfigFile configFile)
    {
        this.configFile = configFile;

        SeparateOverridesForEachRace = configFile.Bind(SECTION_NAME, nameof(SeparateOverridesForEachRace), false);
        BaseOverrideItem = new(ITEM_SECTION_NAME_BASE, configFile);

        if (SeparateOverridesForEachRace.Value)
            Game.Patches.GameManager.Cache_ScriptableAssets.OnPostfix += InitializeOverridesForEachRace;
    }

    private void InitializeOverridesForEachRace()
    {
        Game.Patches.GameManager.Cache_ScriptableAssets.OnPostfix -= InitializeOverridesForEachRace;

        foreach (string raceName in CachedRaces.Keys)
            GetOrCreateItem(raceName);
    }

    public RaceParametersOverrideItem GetOrCreateItem(string raceName)
    {
        if (OverrideItems.TryGetValue(raceName, out RaceParametersOverrideItem item))
            return item;

        item = new(string.Format(ITEM_SECTION_NAME_TEMPLATE, raceName), configFile);
        OverrideItems.Add(raceName, item);

        return item;
    }
}
