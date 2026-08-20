using System;
using System.Collections.Generic;
using Tanuki.Atlyss.FluffUtilities.Types.Configuration.Sections;

namespace Tanuki.Atlyss.FluffUtilities.Managers.Game.Controllers;

public sealed class Race
{
    private static GameManager GameManager => GameManager._current;
    private IReadOnlyDictionary<string, ScriptablePlayerRace> CachedRaces => Atlyss.Game.Accessors.GameManager._cachedScriptableRaces(GameManager);
    private Configuration configuration = Configuration.Instance;

    private RaceDisplayParametersOverrideItem? baseDisplayParametersOverride = null;

    internal Race()
    {
        Atlyss.Game.Patches.GameManager.Cache_ScriptableAssets.OnPostfix += OnScriptableAssetsCached;
        configuration.RaceDisplayParametersOverride.BaseOverrideItem.Enabled.SettingChanged += OnBaseDisplayParametersStateChanged;

        RefreshBaseDisplayParametersOverride();
    }


    private void RefreshBaseDisplayParametersOverride() =>
        baseDisplayParametersOverride = configuration.RaceDisplayParametersOverride.BaseOverrideItem.Enabled.Value ? configuration.RaceDisplayParametersOverride.BaseOverrideItem : null;

    private void OnBaseDisplayParametersStateChanged(object sender, EventArgs e) => RefreshBaseDisplayParametersOverride();

    private void OnScriptableAssetsCached()
    {

    }

    private RaceDisplayParametersOverrideItem? GetDisplayParametersOverride(string? raceName)
    {
        RaceDisplayParametersOverrideItem? overrideItem = baseDisplayParametersOverride;

        if (!string.IsNullOrEmpty(raceName))
            Configuration.Instance.RaceDisplayParametersOverride.OverrideItems.TryGetValue(raceName, out overrideItem);

        return overrideItem;
    }

    public void Override()
    {
        foreach (ScriptablePlayerRace scriptablePlayerRace in CachedRaces.Values)
        {
            CharacterParamsGroup characterParamsGroup = scriptablePlayerRace._raceDisplayParams;
            RaceDisplayParametersOverrideItem? overrideItem = GetDisplayParametersOverride(scriptablePlayerRace._raceName);

            if (overrideItem is null)
                continue;

            characterParamsGroup._headWidthRange.Set(overrideItem.HeadWidth.Minimum.Value, overrideItem.HeadWidth.Maximum.Value);
            characterParamsGroup._headModRange.Set(overrideItem.MuzzleLength.Minimum.Value, overrideItem.MuzzleLength.Maximum.Value);
            characterParamsGroup._heightRange.Set(overrideItem.Height.Minimum.Value, overrideItem.Height.Maximum.Value);
            characterParamsGroup._widthRange.Set(overrideItem.Width.Minimum.Value, overrideItem.Width.Maximum.Value);
            characterParamsGroup._torsoRange.Set(overrideItem.TorsoSize.Minimum.Value, overrideItem.TorsoSize.Maximum.Value);
            characterParamsGroup._boobRange.Set(overrideItem.BreastSize.Minimum.Value, overrideItem.BreastSize.Maximum.Value);
            characterParamsGroup._armRange.Set(overrideItem.ArmsSize.Minimum.Value, overrideItem.ArmsSize.Maximum.Value);
            characterParamsGroup._bellyRange.Set(overrideItem.BellySize.Minimum.Value, overrideItem.BellySize.Maximum.Value);
            characterParamsGroup._bottomRange.Set(overrideItem.BottomSize.Minimum.Value, overrideItem.BottomSize.Maximum.Value);
            characterParamsGroup._pitchRange.Set(overrideItem.VoicePitch.Minimum.Value, overrideItem.VoicePitch.Maximum.Value);
        }
    }
}
