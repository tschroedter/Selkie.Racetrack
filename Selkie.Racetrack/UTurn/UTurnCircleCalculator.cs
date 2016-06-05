using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Turn;
using Selkie.Racetrack.Interfaces.UTurn;

namespace Selkie.Racetrack.UTurn
{
    public class UTurnCircleCalculator : IUTurnCircleCalculator
    {
        public UTurnCircleCalculator([NotNull] IDetermineCirclePairCalculator determineCirclePairCalculator,
                                     [NotNull] IPossibleTurnCircles possibleTurnCircles,
                                     [NotNull] IAngleToCentrePointCalculator angleToCentrePointCalculator)
        {
            UTurnOneIntersectionPoint = Point.Unknown;
            UTurnZeroIntersectionPoint = Point.Unknown;
            TurnDirection = Constants.TurnDirection.Unknown;
            Circle = Geometry.Shapes.Circle.Unknown;
            Settings = Racetrack.Settings.Unknown;
            m_DetermineCirclePairCalculator = determineCirclePairCalculator;
            m_PossibleTurnCircles = possibleTurnCircles;
            m_AngleToCentrePointCalculator = angleToCentrePointCalculator;
        }

        private readonly IAngleToCentrePointCalculator m_AngleToCentrePointCalculator;
        private readonly IDetermineCirclePairCalculator m_DetermineCirclePairCalculator;
        private readonly IPossibleTurnCircles m_PossibleTurnCircles;

        public ISettings Settings { get; set; }

        public ICircle Circle { get; private set; }

        public Constants.TurnDirection TurnDirection { get; private set; }

        public ITurnCircle Zero
        {
            get
            {
                return m_DetermineCirclePairCalculator.Zero;
            }
        }

        public ITurnCircle One
        {
            get
            {
                return m_DetermineCirclePairCalculator.One;
            }
        }

        public ITurnCirclePair Pair
        {
            get
            {
                return m_DetermineCirclePairCalculator.Pair;
            }
        }

        public Point UTurnZeroIntersectionPoint { get; private set; }

        public Point UTurnOneIntersectionPoint { get; private set; }

        public void Calculate()
        {
            m_PossibleTurnCircles.Settings = Settings;
            m_PossibleTurnCircles.Calculate();

            m_DetermineCirclePairCalculator.Settings = Settings;
            m_DetermineCirclePairCalculator.Calculate();

            ITurnCirclePair pair = m_DetermineCirclePairCalculator.Pair;
            m_AngleToCentrePointCalculator.Pair = pair;
            m_AngleToCentrePointCalculator.Calculate();

            UTurnZeroIntersectionPoint = m_AngleToCentrePointCalculator.IntersectionPointForTurnCircle(pair.Zero);
            UTurnOneIntersectionPoint = m_AngleToCentrePointCalculator.IntersectionPointForTurnCircle(pair.One);

            ITurnCircle zero = pair.Zero;

            TurnDirection = zero.TurnDirection == Constants.TurnDirection.Clockwise
                                ? Constants.TurnDirection.Counterclockwise
                                : Constants.TurnDirection.Clockwise;

            Circle = new Circle(m_AngleToCentrePointCalculator.CentrePoint,
                                Settings.LargestRadiusForTurn.Length);
        }
    }
}