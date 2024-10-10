// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Screens.Edit.Commands.Proxies;
using osuTK;

namespace osu.Game.Rulesets.Osu.Edit.Commands
{
    public static class SliderCommandProxyExtensions
    {
        public static void Reverse(this CommandProxy<SliderPath> pathProxy, out Vector2 positionalOffset)
        {
            var sliderPath = pathProxy.Target;

            var controlPoints = pathProxy.ControlPoints();

            var inheritedLinearPoints = controlPoints.Where(p => sliderPath.PointsInSegment(p.Target)[0].Type == PathType.LINEAR && p.Type() == null).ToList();

            // Inherited points after a linear point, as well as the first control point if it inherited,
            // should be treated as linear points, so their types are temporarily changed to linear.
            inheritedLinearPoints.ForEach(p => p.SetType(PathType.LINEAR));

            double[] segmentEnds = sliderPath.GetSegmentEnds().ToArray();

            // Remove segments after the end of the slider.
            for (int numSegmentsToRemove = segmentEnds.Count(se => se >= 1) - 1; numSegmentsToRemove > 0 && controlPoints.Count > 0;)
            {
                if (controlPoints.Last().Type() is not null)
                {
                    numSegmentsToRemove--;
                    segmentEnds = segmentEnds[..^1];
                }

                controlPoints.RemoveAt(controlPoints.Count - 1);
            }

            // Restore original control point types.
            inheritedLinearPoints.ForEach(p => p.SetType(null));

            // Recalculate middle perfect curve control points at the end of the slider path.
            if (controlPoints.Count >= 3 && controlPoints[^3].Type() == PathType.PERFECT_CURVE && controlPoints[^2].Type() == null && segmentEnds.Any())
            {
                double lastSegmentStart = segmentEnds.Length > 1 ? segmentEnds[^2] : 0;
                double lastSegmentEnd = segmentEnds[^1];

                var circleArcPath = new List<Vector2>();
                sliderPath.GetPathToProgress(circleArcPath, lastSegmentStart / lastSegmentEnd, 1);

                controlPoints[^2].SetPosition(circleArcPath[circleArcPath.Count / 2]);
            }

            pathProxy.reverseControlPoints(out positionalOffset);
        }

        /// <summary>
        /// Reverses the order of the provided <see cref="SliderPath"/>'s <see cref="PathControlPoint"/>s.
        /// </summary>
        /// <param name="sliderPath">The <see cref="SliderPath"/>.</param>
        /// <param name="positionalOffset">The positional offset of the resulting path. It should be added to the start position of this path.</param>
        private static void reverseControlPoints(this CommandProxy<SliderPath> sliderPath, out Vector2 positionalOffset)
        {
            var points = sliderPath.ControlPoints().ToArray();
            positionalOffset = sliderPath.Target.PositionAt(1);

            sliderPath.ControlPoints().Clear();

            PathType? lastType = null;

            for (int i = 0; i < points.Length; i++)
            {
                var p = points[i];
                p.SetPosition(p.Position() - positionalOffset);

                // propagate types forwards to last null type
                if (i == points.Length - 1)
                {
                    p.SetType(lastType);
                    p.SetPosition(Vector2.Zero);
                }
                else if (p.Type() != null)
                {
                    var type = p.Type();
                    (type, lastType) = (lastType, type);
                    p.SetType(type);
                }

                sliderPath.ControlPoints().Insert(0, p);
            }
        }
    }
}
