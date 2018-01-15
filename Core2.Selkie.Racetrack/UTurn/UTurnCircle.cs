using System;
using JetBrains.Annotations;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Racetrack.UTurn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class UTurnCircle : IUTurnCircle
    {
        private UTurnCircle(bool isUnknown = false)
        {
            Settings = Racetrack.Settings.Unknown;
            IsUnknown = isUnknown;
        }

        public UTurnCircle([NotNull] IPossibleTurnCircles possibleTurnCircles,
                           [NotNull] IUTurnCircleCalculator uTurnCircleCalculator)
        {
            Settings = Racetrack.Settings.Unknown;
            m_PossibleTurnCircles = possibleTurnCircles;
            m_UTurnCircleCalculator = uTurnCircleCalculator;
        }

        public static readonly IUTurnCircle Unknown = new UTurnCircle(true);
        private readonly IPossibleTurnCircles m_PossibleTurnCircles;
        private readonly IUTurnCircleCalculator m_UTurnCircleCalculator;

        public bool IsUnknown { get; private set; }

        public ISettings Settings { get; set; }

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

        internal bool DetermineIsUTurnRequired([NotNull] IPossibleTurnCircles possibleTurnCircles)
        {
            ICircle startPointPort = possibleTurnCircles.StartTurnCirclePort.Circle;
            ICircle startPointStarboard = possibleTurnCircles.StartTurnCircleStarboard.Circle;
            ICircle finishPointPort = possibleTurnCircles.FinishTurnCirclePort.Circle;
            ICircle finishPointStarboard = possibleTurnCircles.FinishTurnCircleStarboard.Circle;

            return startPointPort.Intersects(finishPointStarboard) && startPointStarboard.Intersects(finishPointPort);
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