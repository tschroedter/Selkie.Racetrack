using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Racetrack
{
    [ProjectComponent(Lifestyle.Transient)]
    public class Path : IPath
    {
        private const int DoNotCareId = -1;
        public static readonly IPath Unknown = new Path(Point.Unknown);

        private readonly IPolyline m_Polyline = new Polyline(DoNotCareId,
                                                             Constants.LineDirection.Forward);

        private readonly Point m_StartPoint;
        private Point m_EndPoint = Point.Unknown;

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
            m_StartPoint = startArcSegment.StartPoint;

            AddSegment(startArcSegment);
            AddSegment(line);
            AddSegment(endArcSegment);
        }

        public override string ToString()
        {
            return "Length: {0:F2}".Inject(Distance.Length);
        }

        #region IPath Members

        public Point StartPoint
        {
            get
            {
                return m_StartPoint;
            }
        }

        public Point EndPoint
        {
            get
            {
                return m_EndPoint;
            }
            private set
            {
                m_EndPoint = value;
            }
        }

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