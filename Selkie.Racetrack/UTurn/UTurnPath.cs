using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;

namespace Selkie.Racetrack.UTurn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class UTurnPath : IUTurnPath
    {
        private readonly IDetermineTurnCircleCalculator m_Calculator;
        private readonly IUTurnCircle m_UTurnCircle;
        private IPath m_Path = Racetrack.Path.Unknown;
        private ISettings m_Settings = Racetrack.Settings.Unknown;

        public UTurnPath([NotNull] IUTurnCircle uTurnCircle,
                         [NotNull] IDetermineTurnCircleCalculator calculator)
        {
            m_UTurnCircle = uTurnCircle;
            m_Calculator = calculator;
        }

        public void Calculate()
        {
            m_UTurnCircle.Settings = m_Settings;
            m_UTurnCircle.Calculate();

            m_Path = m_UTurnCircle.IsRequired
                         ? CalculatePath(m_Settings,
                                         m_UTurnCircle)
                         : Racetrack.Path.Unknown;
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

        public IUTurnCircle UTurnCircle
        {
            get
            {
                return m_UTurnCircle;
            }
        }

        [NotNull]
        private IPath CalculatePath([NotNull] ISettings settings,
                                    [NotNull] IUTurnCircle uTurnCircle)
        {
            DetermineTurnCircles();

            IPath path = CreatePath(settings,
                                    uTurnCircle);

            return path;
        }

        [NotNull]
        private IPath CreatePath([NotNull] ISettings settings,
                                 [NotNull] IUTurnCircle uTurnCircle)
        {
            ITurnCircleArcSegment startArcSegment = CreateStartTurnCircleArcSegment(settings,
                                                                                    uTurnCircle,
                                                                                    m_Calculator);

            ITurnCircleArcSegment finishArcSegment = CreateFinishTurnCircleArcSegment(settings,
                                                                                      uTurnCircle,
                                                                                      m_Calculator);

            ITurnCircleArcSegment uTurnArcSegment = CreateUTurnArcSegment(uTurnCircle,
                                                                          Constants.CircleOrigin.Unknown,
                                                                          startArcSegment.EndPoint,
                                                                          finishArcSegment.StartPoint);

            IPath path = new Path(startArcSegment,
                                  uTurnArcSegment,
                                  finishArcSegment);
            return path;
        }

        internal void DetermineTurnCircles()
        {
            m_Calculator.Settings = m_Settings;
            m_Calculator.UTurnCircle = m_UTurnCircle;
            m_Calculator.Calculate();
        }

        [NotNull]
        internal ITurnCircleArcSegment CreateFinishTurnCircleArcSegment([NotNull] ISettings settings,
                                                                        [NotNull] IUTurnCircle uTurnCircle,
                                                                        [NotNull] IDetermineTurnCircleCalculator
                                                                            calculator)
        {
            Point intersectionPoint = DeterminArcSegmentIntersectionPoint(uTurnCircle,
                                                                          calculator.FinishTurnCircle);

            ITurnCircleArcSegment finishArcSegment = CreateFinishArcSegment(settings,
                                                                            intersectionPoint,
                                                                            calculator.FinishTurnCircle);
            return finishArcSegment;
        }

        [NotNull]
        internal ITurnCircleArcSegment CreateStartTurnCircleArcSegment([NotNull] ISettings settings,
                                                                       [NotNull] IUTurnCircle uTurnCircle,
                                                                       [NotNull] IDetermineTurnCircleCalculator
                                                                           calculator)
        {
            Point intersectionPoint = DeterminArcSegmentIntersectionPoint(uTurnCircle,
                                                                          calculator.StartTurnCircle);

            ITurnCircleArcSegment startArcSegment = CreateStartArcSegment(settings,
                                                                          intersectionPoint,
                                                                          calculator.StartTurnCircle);
            return startArcSegment;
        }

        [NotNull]
        // ReSharper disable once TooManyArguments
        internal ITurnCircleArcSegment CreateUTurnArcSegment([NotNull] IUTurnCircle uTurnCircle,
                                                             Constants.CircleOrigin origin,
                                                             [NotNull] Point startPoint,
                                                             [NotNull] Point endPoint)
        {
            ITurnCircleArcSegment turnCircle = new TurnCircleArcSegment(uTurnCircle.Circle,
                                                                        uTurnCircle.TurnDirection,
                                                                        origin,
                                                                        startPoint,
                                                                        endPoint);

            return turnCircle;
        }

        [NotNull]
        internal ITurnCircleArcSegment CreateStartArcSegment([NotNull] ISettings settings,
                                                             [NotNull] Point intersectionPoint,
                                                             [NotNull] ITurnCircle startTurnCircle)
        {
            ITurnCircleArcSegment turnCircle = new TurnCircleArcSegment(startTurnCircle.Circle,
                                                                        startTurnCircle.TurnDirection,
                                                                        Constants.CircleOrigin.Start,
                                                                        settings.StartPoint,
                                                                        intersectionPoint);

            return turnCircle;
        }

        [NotNull]
        internal Point DeterminArcSegmentIntersectionPoint([NotNull] IUTurnCircle uTurnCircle,
                                                           [NotNull] ITurnCircle circle)
        {
            return circle.IsPointOnCircle(uTurnCircle.UTurnZeroIntersectionPoint)
                       ? uTurnCircle.UTurnZeroIntersectionPoint
                       : uTurnCircle.UTurnOneIntersectionPoint;
        }

        [NotNull]
        internal ITurnCircleArcSegment CreateFinishArcSegment([NotNull] ISettings settings,
                                                              [NotNull] Point intersectionPoint,
                                                              [NotNull] ITurnCircle finishTurnCircle)
        {
            ITurnCircleArcSegment turnCircle = new TurnCircleArcSegment(finishTurnCircle.Circle,
                                                                        finishTurnCircle.TurnDirection,
                                                                        Constants.CircleOrigin.Finish,
                                                                        intersectionPoint,
                                                                        settings.FinishPoint);

            return turnCircle;
        }

        #region IUTurnPath Members

        public IPath Path
        {
            get
            {
                return m_Path;
            }
        }

        public bool IsRequired
        {
            get
            {
                return m_UTurnCircle.IsRequired;
            }
        }

        #endregion
    }
}