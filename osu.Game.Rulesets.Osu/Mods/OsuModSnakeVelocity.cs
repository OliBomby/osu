// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects.Drawables;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModSnakeVelocity : Mod, IApplicableToDrawableHitObject
    {
        public override string Name => "Snake Velocity";

        public override string Acronym => "SV";

        public override LocalisableString Description => "Sliders snake in at the same speed they are consumed.";

        public override double ScoreMultiplier => 1;

        public override ModType Type => ModType.Fun;

        public void ApplyToDrawableHitObject(DrawableHitObject drawableObject)
        {
            switch (drawableObject)
            {
                case DrawableSlider slider:
                    slider.SnakeInAtSliderVelocity = true;
                    break;

                case DrawableSliderTick tick:
                    tick.RevealWithSliderBody = true;
                    break;
            }
        }
    }
}
