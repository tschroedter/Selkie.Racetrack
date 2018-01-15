using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Core2.Selkie.Geometry.Calculators;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class TurnCirclePair : ITurnCirclePair
    {
        private TurnCirclePair(bool isUnknown)
        {
            m_IsUnknown = isUnknown;
            m_Calculator = new CirclePairTangentLinesCalculator(m_CirclePair);
        }

        public TurnCirclePair([NotNull] ISettings settings,
                              [NotNull] ITurnCircle one,
                              [NotNull] ITurnCircle two)
        {
            m_CirclePair = new CirclePair(one.Circle,
                                          two.Circle);
            m_Calculator = new CirclePairTangentLinesCalculator(m_CirclePair);

            if ( m_CirclePair.Zero.CentrePoint == one.CentrePoint )
            {
                m_Zero = one;
                m_One = two;
            }
            else
            {
                m_Zero = two;
                m_One = one;
            }

            // Note: special case half-circle required
            if ( m_Zero.Circle.Equals(m_One.Circle) )
            {
                m_ValidTangents = CreateDummyTangentsForSameCircles(settings,
                                                                    m_Zero);
                m_IsValid = true;
            }
            else
            {
                m_Calculator.CirclePair = m_CirclePair;
                m_Calculator.Calculate();

                m_ValidTangents = SelectTangents(m_Calculator);
                m_IsValid = DetermineIsValid(one,
                                             two) && m_ValidTangents.Any();
            }
        }

        public static readonly ITurnCirclePair Unknown = new TurnCirclePair(true);
        private readonly ICirclePairTangentLinesCalculator m_Calculator;
        private readonly ICirclePair m_CirclePair = Geometry.Shapes.CirclePair.Unknown;
        private readonly bool m_IsUnknown;
        private readonly bool m_IsValid;
        private readonly ITurnCircle m_One = TurnCircle.Unknown;
        private readonly IEnumerable <ILine> m_ValidTangents = new ILine[0];
        private readonly ITurnCircle m_Zero = TurnCircle.Unknown;

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        [NotNull]
        internal IEnumerable <ILine> CreateDummyTangentsForSameCircles([NotNull] ISettings settings,
                                                                       [NotNull] ITurnCircle turnCircle)
        {
            Point point = turnCircle.Circle.PointOnCircle(settings.StartAzimuth);
            var line = new Line(point,
                                point);

            var lines = new List <ILine>
                        {
                            line
                        };

            return lines;
        }

        internal bool DetermineIsValid([NotNull] ITurnCircle one,
                                       [NotNull] ITurnCircle two)
        {
            if ( one.Circle.Equals(two.Circle) )
            {
                return true;
            }

            double radius = ( one.Radius.Length + two.Radius.Length ) / 2.0;
            double distance = one.Circle.Distance(two.Circle);

            return distance >= radius;
        }

        [NotNull]
        internal IEnumerable <ILine> SelectTangents([NotNull] ICirclePairTangentLinesCalculator calculator)
        {
            var lines = new List <ILine>();

            lines.AddRange(calculator.OuterTangents);
            lines.AddRange(calculator.InnerTangents);

            return lines;
        }

        #region ITurnCirclePair Members

        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
        }

        public IEnumerable <ILine> OuterTangents
        {
            get
            {
                return m_Calculator.OuterTangents;
            }
        }

        public IEnumerable <ILine> InnerTangents
        {
            get
            {
                return m_Calculator.InnerTangents;
            }
        }

        public IEnumerable <ILine> Tangents
        {
            get
            {
                return m_Calculator.Tangents;
            }
        }

        public IEnumerable <ILine> ValidTangents
        {
            get
            {
                return m_ValidTangents;
            }
        }

        public int NumberOfTangents
        {
            get
            {
                return m_CirclePair.NumberOfTangents;
            }
        }

        public ITurnCircle Zero
        {
            get
            {
                return m_Zero;
            }
        }

        public ITurnCircle One
        {
            get
            {
                return m_One;
            }
        }

        public ICirclePair CirclePair
        {
            get
            {
                return m_CirclePair;
            }
        }

        #endregion
    }
}