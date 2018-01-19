using System.Collections.Generic;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using JetBrains.Annotations;
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Core2.Selkie.Racetrack.Interfaces
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