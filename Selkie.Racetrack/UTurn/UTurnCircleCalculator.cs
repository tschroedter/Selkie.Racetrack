using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.UTurn
{
    public class UTurnCircleCalculator : IUTurnCircleCalculator
    {
        private readonly IDetermineCirclePairCalculator m_DetermineCirclePairCalculator;
        private ICircle m_Circle = Geometry.Shapes.Circle.Unknown;
        private IPossibleTurnCircles m_PossibleTurnCircles = PossibleTurnCircles.Unknown;
        private ISettings m_Settings = Racetrack.Settings.Unknown;
        private Constants.TurnDirection m_TurnDirection = Constants.TurnDirection.Unknown;
        private Point m_UTurnOneIntersectionPoint = Point.Unknown;
        private Point m_UTurnZeroIntersectionPoint = Point.Unknown;

        public UTurnCircleCalculator([NotNull] IDetermineCirclePairCalculator determineCirclePairCalculator)
        {
            m_DetermineCirclePairCalculator = determineCirclePairCalculator;
        }

        public ISettings Settings
        {
            get
            {
                return m_Settings;
            }
            set
            {
                m_Settings = value;
            }
        }

        public ICircle Circle
        {
            get
            {
                return m_Circle;
            }
        }

        public Constants.TurnDirection TurnDirection
        {
            get
            {
                return m_TurnDirection;
            }
        }

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

        public Point UTurnZeroIntersectionPoint
        {
            get
            {
                return m_UTurnZeroIntersectionPoint;
            }
            private set
            {
                m_UTurnZeroIntersectionPoint = value;
            }
        }

        public Point UTurnOneIntersectionPoint
        {
            get
            {
                return m_UTurnOneIntersectionPoint;
            }
            private set
            {
                m_UTurnOneIntersectionPoint = value;
            }
        }

        public void Calculate()
        {
            m_PossibleTurnCircles = new PossibleTurnCircles
                                    {
                                        Settings = m_Settings
                                    };
            m_PossibleTurnCircles.Calculate();

            m_DetermineCirclePairCalculator.Settings = m_Settings;
            m_DetermineCirclePairCalculator.Calculate();

            ITurnCirclePair pair = m_DetermineCirclePairCalculator.Pair;
            var calculator = new AngleToCentrePointCalculator(pair);

            UTurnZeroIntersectionPoint = calculator.IntersectionPointForTurnCircle(pair.Zero);
            UTurnOneIntersectionPoint = calculator.IntersectionPointForTurnCircle(pair.One);

            m_TurnDirection = pair.Zero.TurnDirection == Constants.TurnDirection.Clockwise
                                  ? Constants.TurnDirection.Counterclockwise
                                  : Constants.TurnDirection.Clockwise;

            m_Circle = new Circle(calculator.CentrePoint,
                                  m_Settings.Radius.Length);
        }
    }
}