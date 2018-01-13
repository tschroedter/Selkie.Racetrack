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
    [ProjectComponent(Lifestyle.Transient)]
    public class Path : IPath
    {
        public Path([NotNull] Point startPoint)
        {
            m_StartPoint = startPoint;
            EndPoint = startPoint;
        }

        public Path([NotNull] IPolyline polyline)
        {
            m_StartPoint = polyline.StartPoint;
            EndPoint = polyline.EndPoint;
            m_Polyline = polyline;
        }

        public Path([NotNull] IPolylineSegment startArcSegment,
                    [NotNull] IPolylineSegment line,
                    [NotNull] IPolylineSegment endArcSegment)
        {
            EndPoint = Point.Unknown;
            m_StartPoint = startArcSegment.StartPoint;

            AddSegment(startArcSegment);
            AddSegment(line);
            AddSegment(endArcSegment);
        }

        private const int DoNotCareId = -1;
        public static readonly IPath Unknown = new Path(Point.Unknown);

        private readonly IPolyline m_Polyline = new Polyline(DoNotCareId,
                                                             Constants.LineDirection.Forward);  // TODO Remove Selkie.Geometry LineDirection and use Selkie.Common one

        private readonly Point m_StartPoint;

        public override string ToString()
        {
            return $"Length: {Distance.Length:F2}";
        }

        #region IPath Members

        public Point StartPoint
        {
            get
            {
                return m_StartPoint;
            }
        }

        public Point EndPoint { get; private set; }

        public void AddSegment(IPolylineSegment polylineSegment)
        {
            m_Polyline.AddSegment(polylineSegment);

            EndPoint = polylineSegment.EndPoint;
        }

        public IPath Reverse()
        {
            if ( !m_Polyline.Segments.Any() )
            {
                return new Path(m_StartPoint);
            }

            IPolyline reversePolyline = m_Polyline.Reverse();

            var reversePath = new Path(reversePolyline);

            return reversePath;
        }

        public IPolyline Polyline
        {
            get
            {
                return m_Polyline;
            }
        }

        public IEnumerable <IPolylineSegment> Segments
        {
            get
            {
                return m_Polyline.Segments;
            }
        }

        public bool IsUnknown
        {
            get
            {
                return !m_Polyline.Segments.Any();
            }
        }

        public Distance Distance
        {
            get
            {
                return new Distance(m_Polyline.Length);
            }
        }

        #endregion
    }
}