using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.UTurn
{
    public class DetermineTurnCircleCalculator : IDetermineTurnCircleCalculator
    {
        private ITurnCircle m_FinishTurnCircle = TurnCircle.Unknown;
        private ISettings m_Settings = Racetrack.Settings.Unknown;
        private ITurnCircle m_StartTurnCircle = TurnCircle.Unknown;
        private IUTurnCircle m_UTurnCircle = UTurn.UTurnCircle.Unknown;

        public void Calculate()
        {
            m_StartTurnCircle = DetermineStartTurnCircle(m_Settings,
                                                         m_UTurnCircle);
            m_FinishTurnCircle = DetermineFinishTurnCircle(m_Settings,
                                                           m_UTurnCircle);
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
            set
            {
                m_UTurnCircle = value;
            }
        }

        public ITurnCircle StartTurnCircle
        {
            get
            {
                return m_StartTurnCircle;
            }
        }

        public ITurnCircle FinishTurnCircle
        {
            get
            {
                return m_FinishTurnCircle;
            }
        }

        [NotNull]
        internal ITurnCircle DetermineStartTurnCircle([NotNull] ISettings settings,
                                                      [NotNull] IUTurnCircle uTurnCircle)
        {
            ITurnCircle turnCircle = uTurnCircle.Zero;

            if ( !turnCircle.IsPointOnCircle(settings.StartPoint) )
            {
                turnCircle = uTurnCircle.One;
            }

            return turnCircle;
        }

        [NotNull]
        internal ITurnCircle DetermineFinishTurnCircle([NotNull] ISettings settings,
                                                       [NotNull] IUTurnCircle uTurnCircle)
        {
            ITurnCircle turnCircle = uTurnCircle.Zero;

            if ( !turnCircle.IsPointOnCircle(settings.FinishPoint) )
            {
                turnCircle = uTurnCircle.One;
            }

            return turnCircle;
        }
    }
}