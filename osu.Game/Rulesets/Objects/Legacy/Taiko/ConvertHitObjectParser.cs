﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osuTK;
using osu.Framework.Bindables;
using osu.Game.Audio;

namespace osu.Game.Rulesets.Objects.Legacy.Taiko
{
    /// <summary>
    /// A HitObjectParser to parse legacy osu!taiko Beatmaps.
    /// </summary>
    public class ConvertHitObjectParser : Legacy.ConvertHitObjectParser
    {
        public ConvertHitObjectParser(double offset, int formatVersion)
            : base(offset, formatVersion)
        {
        }

        protected override HitObject CreateHit(Vector2 position, bool newCombo, int comboOffset)
        {
            return new ConvertHit();
        }

        protected override HitObject CreateSlider(Vector2 position, bool newCombo, int comboOffset, PathControlPoint[] controlPoints, double? length, int repeatCount,
                                                  BindableList<BindableList<HitSampleInfo>> nodeSamples)
        {
            return new ConvertSlider
            {
                Path = new SliderPath(controlPoints, length),
                NodeSamples = nodeSamples,
                RepeatCount = repeatCount
            };
        }

        protected override HitObject CreateSpinner(Vector2 position, bool newCombo, int comboOffset, double duration)
        {
            return new ConvertSpinner
            {
                Duration = duration
            };
        }

        protected override HitObject CreateHold(Vector2 position, bool newCombo, int comboOffset, double duration)
        {
            return null;
        }
    }
}
