using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Interfaces
{
    public interface IPath
    {
        [NotNull]
        Point StartPoint { get; }

        [NotNull]
        Point EndPoint { get; }

        [NotNull]
        IPolyline Polyline { get; }

        [NotNull]
        IEnumerable <IPolylineSegment> Segments { get; }

        bool IsUnknown { get; }

        [NotNull]
        Distance Distance { get; }

        void AddSegment([NotNull] IPolylineSegment polylineSegment);

        [NotNull]
        IPath Reverse();
    }
}