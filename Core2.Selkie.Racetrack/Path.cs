using System.Collections.Generic;
using System.Linq;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack
{
    [UsedImplicitly]
    [ProjectComponent(Lifestyle.Transient)]
    public class Path : IPath
    {
        public Path([NotNull] Point startPoint)
        {
            StartPoint = startPoint;
            EndPoint = startPoint;
        }

        public Path([NotNull] IPolyline polyline)
        {
            StartPoint = polyline.StartPoint;
            EndPoint = polyline.EndPoint;
            Polyline = polyline;
        }

        public Path([NotNull] IPolylineSegment startArcSegment,
                    [NotNull] IPolylineSegment line,
                    [NotNull] IPolylineSegment endArcSegment)
        {
            EndPoint = Point.Unknown;
            StartPoint = startArcSegment.StartPoint;

            AddSegment(startArcSegment);
            AddSegment(line);
            AddSegment(endArcSegment);
        }

        private const int DoNotCareId = -1;
        public static readonly IPath Unknown = new Path(Point.Unknown);

        public override string ToString()
        {
            return $"Length: {Distance.Length:F2}";
        }

        #region IPath Members

        public Point StartPoint { get; }

        public Point EndPoint { get; private set; }

        public void AddSegment(IPolylineSegment polylineSegment)
        {
            Polyline.AddSegment(polylineSegment);

            EndPoint = polylineSegment.EndPoint;
        }

        public IPath Reverse()
        {
            if ( !Polyline.Segments.Any() )
            {
                return new Path(StartPoint);
            }

            IPolyline reversePolyline = Polyline.Reverse();

            var reversePath = new Path(reversePolyline);

            return reversePath;
        }

        public IPolyline Polyline { get; } = new Polyline(DoNotCareId,
                                                          Constants.LineDirection.Forward);

        public IEnumerable <IPolylineSegment> Segments => Polyline.Segments;

        public bool IsUnknown => !Polyline.Segments.Any();

        public Distance Distance => new Distance(Polyline.Length);

        #endregion
    }
}