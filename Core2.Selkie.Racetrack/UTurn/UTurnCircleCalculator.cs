using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.UTurn
{
    [UsedImplicitly]
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

        public ITurnCircle Zero => m_DetermineCirclePairCalculator.Zero;

        public ITurnCircle One => m_DetermineCirclePairCalculator.One;

        public ITurnCirclePair Pair => m_DetermineCirclePairCalculator.Pair;

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