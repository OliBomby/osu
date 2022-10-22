// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Screens.Play;

namespace osu.Game.Screens.Edit.GameplayTest
{
    public class EditSelectPlayer : Player
    {
        protected override bool CheckModsAllowFailure() => false; // never fail.
    }
}
