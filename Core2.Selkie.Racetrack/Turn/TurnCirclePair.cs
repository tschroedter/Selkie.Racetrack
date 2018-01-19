using System.Collections.Generic;
using System.Linq;
using Core2.Selkie.Geometry.Calculators;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class TurnCirclePair : ITurnCirclePair
    {
        private TurnCirclePair(bool isUnknown)
        {
            IsUnknown = isUnknown;
            m_Calculator = new CirclePairTangentLinesCalculator(CirclePair);
        }

        public TurnCirclePair([NotNull] ISettings settings,
                              [NotNull] ITurnCircle one,
                              [NotNull] ITurnCircle two)
        {
            CirclePair = new CirclePair(one.Circle,
                                        two.Circle);
            m_Calculator = new CirclePairTangentLinesCalculator(CirclePair);

            if ( CirclePair.Zero.CentrePoint == one.CentrePoint )
            {
                Zero = one;
                One = two;
            }
            else
            {
                Zero = two;
                One = one;
            }

            // Note: special case half-circle required
            if ( Zero.Circle.Equals(One.Circle) )
            {
                ValidTangents = CreateDummyTangentsForSameCircles(settings,
                                                                  Zero);
                IsValid = true;
            }
            else
            {
                m_Calculator.CirclePair = CirclePair;
                m_Calculator.Calculate();

                ValidTangents = SelectTangents(m_Calculator);
                IsValid = DetermineIsValid(one,
                                           two) && ValidTangents.Any();
            }
        }

        public static readonly ITurnCirclePair Unknown = new TurnCirclePair(true);
        private readonly ICirclePairTangentLinesCalculator m_Calculator;

        public bool IsUnknown { get; }

        [NotNull]
        [UsedImplicitly]
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

        [UsedImplicitly]
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
        [UsedImplicitly]
        internal IEnumerable <ILine> SelectTangents([NotNull] ICirclePairTangentLinesCalculator calculator)
        {
            var lines = new List <ILine>();

            lines.AddRange(calculator.OuterTangents);
            lines.AddRange(calculator.InnerTangents);

            return lines;
        }

        #region ITurnCirclePair Members

        public bool IsValid { get; }

        public IEnumerable <ILine> OuterTangents => m_Calculator.OuterTangents;

        public IEnumerable <ILine> InnerTangents => m_Calculator.InnerTangents;

        public IEnumerable <ILine> Tangents => m_Calculator.Tangents;

        public IEnumerable <ILine> ValidTangents { get; } = new ILine[0];

        public int NumberOfTangents => CirclePair.NumberOfTangents;

        public ITurnCircle Zero { get; } = TurnCircle.Unknown;

        public ITurnCircle One { get; } = TurnCircle.Unknown;

        public ICirclePair CirclePair { get; } = Geometry.Shapes.CirclePair.Unknown;

        #endregion
    }
}