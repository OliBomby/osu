﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Osu.Objects
{
    public class StreamHitCircle : HitCircle
    {
        public readonly Bindable<bool> NewComboBindable = new BindableBool();

        public override bool NewCombo
        {
            get => NewComboBindable.Value;
            set => NewComboBindable.Value = value;
        }
    }
}