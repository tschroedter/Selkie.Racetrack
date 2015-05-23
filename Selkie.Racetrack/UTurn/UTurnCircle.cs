using System;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;
using Selkie.Windsor;

namespace Selkie.Racetrack.UTurn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class UTurnCircle : IUTurnCircle
    {
        public static IUTurnCircle Unknown = new UTurnCircle(true);
        private readonly bool m_IsUnknown;
        private readonly IPossibleTurnCircles m_PossibleTurnCircles;
        private readonly IUTurnCircleCalculator m_UTurnCircleCalculator;
        private ISettings m_Settings = Racetrack.Settings.Unknown;

        private UTurnCircle(bool isUnknown = false)
        {
            m_IsUnknown = isUnknown;
        }

        public UTurnCircle([NotNull] IPossibleTurnCircles possibleTurnCircles,
                           [NotNull] IUTurnCircleCalculator uTurnCircleCalculator)
        {
            m_PossibleTurnCircles = possibleTurnCircles;
            m_UTurnCircleCalculator = uTurnCircleCalculator;
        }

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
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

        public void Calculate()
        {
            if ( IsSameStartFinishAzimuth() )
            {
                IsPossible = false;
                IsRequired = false;

                return;
            }

            CalculatePossibleUTurnCircle();
        }

        private void CalculatePossibleUTurnCircle()
        {
            m_PossibleTurnCircles.Settings = Settings;
            m_PossibleTurnCircles.Calculate();

            IsRequired = DetermineIsUTurnRequired(m_PossibleTurnCircles);

            if ( !IsRequired )
            {
                IsPossible = false;
                return;
            }

            IsPossible = true;

            m_UTurnCircleCalculator.Settings = Settings;
            m_UTurnCircleCalculator.Calculate();
        }

        private bool IsSameStartFinishAzimuth()
        {
            return Math.Abs(Settings.StartAzimuth.Radians - Settings.FinishAzimuth.Radians) < Constants.EpsilonDistance;
        }

        internal bool DetermineIsUTurnRequired([NotNull] IPossibleTurnCircles possibleTurnCircles)
        {
            ICircle startPointPort = possibleTurnCircles.StartTurnCirclePort.Circle;
            ICircle startPointStarboard = possibleTurnCircles.StartTurnCircleStarboard.Circle;
            ICircle finishPointPort = possibleTurnCircles.FinishTurnCirclePort.Circle;
            ICircle finishPointStarboard = possibleTurnCircles.FinishTurnCircleStarboard.Circle;

            return startPointPort.Intersects(finishPointStarboard) && startPointStarboard.Intersects(finishPointPort);
        }

        #region IUTurnCircle Members

        public Point UTurnZeroIntersectionPoint
        {
            get
            {
                return m_UTurnCircleCalculator.UTurnZeroIntersectionPoint;
            }
        }

        public Point UTurnOneIntersectionPoint
        {
            get
            {
                return m_UTurnCircleCalculator.UTurnOneIntersectionPoint;
            }
        }

        public Point CentrePoint
        {
            get
            {
                return m_UTurnCircleCalculator.Circle.CentrePoint;
            }
        }

        public ICircle Circle
        {
            get
            {
                return m_UTurnCircleCalculator.Circle;
            }
        }

        public Constants.TurnDirection TurnDirection
        {
            get
            {
                return m_UTurnCircleCalculator.TurnDirection;
            }
        }

        public bool IsPossible { get; private set; }

        public bool IsRequired { get; private set; }

        public ITurnCirclePair TurnCirclePair
        {
            get
            {
                return m_UTurnCircleCalculator.Pair;
            }
        }

        public ITurnCircle Zero
        {
            get
            {
                return m_UTurnCircleCalculator.Zero;
            }
        }

        public ITurnCircle One
        {
            get
            {
                return m_UTurnCircleCalculator.One;
            }
        }

        #endregion
    }
}