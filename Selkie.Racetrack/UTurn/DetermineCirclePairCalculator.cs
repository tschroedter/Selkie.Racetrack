﻿using System;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Turn;
using Selkie.Racetrack.Interfaces.UTurn;
using Selkie.Racetrack.Turn;
using Selkie.Windsor.Extensions;

namespace Selkie.Racetrack.UTurn
{
    public class DetermineCirclePairCalculator : IDetermineCirclePairCalculator
    {
        public DetermineCirclePairCalculator([NotNull] IPossibleTurnCircles possibleTurnCircles)
        {
            m_PossibleTurnCircles = possibleTurnCircles;
        }

        private readonly IPossibleTurnCircles m_PossibleTurnCircles;
        private ITurnCirclePair m_Pair = TurnCirclePair.Unknown;
        private ISettings m_Settings = Racetrack.Settings.Unknown;

        public void Calculate()
        {
            m_Pair = Determine(m_Settings,
                               m_PossibleTurnCircles);
        }

        [NotNull]
        internal ITurnCirclePair Determine([NotNull] ISettings settings,
                                           [NotNull] IPossibleTurnCircles possibleTurnCircles)
        {
            possibleTurnCircles.Settings = settings;
            possibleTurnCircles.Calculate();

            var startCircle = new Circle(settings.StartPoint,
                                         1.0);
            Point startExtended = startCircle.PointOnCircle(settings.StartAzimuth);
            var startLine = new Line(settings.StartPoint,
                                     startExtended);

            var calculator = new RacetrackLineDirectionCalculator(startLine,
                                                                  settings.FinishPoint,
                                                                  settings.DefaultTurnDirection);

            switch ( calculator.TurnDirection )
            {
                case Constants.TurnDirection.Counterclockwise:
                    return new TurnCirclePair(settings,
                                              possibleTurnCircles.StartTurnCircleStarboard,
                                              possibleTurnCircles.FinishTurnCircleStarboard);

                case Constants.TurnDirection.Clockwise:
                    return new TurnCirclePair(settings,
                                              possibleTurnCircles.StartTurnCirclePort,
                                              possibleTurnCircles.FinishTurnCirclePort);

                case Constants.TurnDirection.Unknown:
                    return CalculatePair(settings,
                                         possibleTurnCircles);

                default:
                    string message = "Calculated turn direction '{0}' is not known!".Inject(calculator.TurnDirection);
                    throw new ArgumentException(message);
            }
        }

        [NotNull]
        private ITurnCirclePair CalculatePair([NotNull] ISettings settings,
                                              [NotNull] IPossibleTurnCircles possibleTurnCircles)
        {
            var line = new Line(settings.StartPoint,
                                settings.FinishPoint);
            var circle = new Circle(settings.StartPoint,
                                    line.Length);
            Angle angle = circle.RadiansRelativeToXAxisCounterClockwise(settings.FinishPoint);

            Angle relativeToStartAzimuth = angle - settings.StartAzimuth;

            if ( Math.Abs(relativeToStartAzimuth.Radians - Angle.RadiansFor180Degrees) < 0.001 )
            {
                return new TurnCirclePair(settings,
                                          possibleTurnCircles.StartTurnCirclePort,
                                          possibleTurnCircles.FinishTurnCirclePort);
            }
            return new TurnCirclePair(settings,
                                      possibleTurnCircles.StartTurnCircleStarboard,
                                      possibleTurnCircles.FinishTurnCircleStarboard);
        }

        #region IDetermineCirclePairCalculator Members

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

        public ITurnCirclePair Pair
        {
            get
            {
                return m_Pair;
            }
        }

        public ITurnCircle Zero
        {
            get
            {
                return m_Pair.Zero;
            }
        }

        public ITurnCircle One
        {
            get
            {
                return m_Pair.One;
            }
        }

        #endregion
    }
}