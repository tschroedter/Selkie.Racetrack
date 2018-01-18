﻿using System;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using Core2.Selkie.Racetrack.Turn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.UTurn
{
    public class DetermineCirclePairCalculator : IDetermineCirclePairCalculator
    {
        public DetermineCirclePairCalculator([NotNull] IPossibleTurnCircles possibleTurnCircles)
        {
            m_PossibleTurnCircles = possibleTurnCircles;
        }

        private readonly IPossibleTurnCircles m_PossibleTurnCircles;

        public void Calculate()
        {
            Pair = Determine(Settings,
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
                    string message = $"Calculated turn direction '{calculator.TurnDirection}' is not known!";
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

            if ( Math.Abs(relativeToStartAzimuth.Radians - BaseAngle.RadiansFor180Degrees) < 0.001 )
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

        public ISettings Settings { get; set; } = Racetrack.Settings.Unknown;

        public ITurnCirclePair Pair { get; private set; } = TurnCirclePair.Unknown;

        public ITurnCircle Zero => Pair.Zero;

        public ITurnCircle One => Pair.One;

        #endregion
    }
}