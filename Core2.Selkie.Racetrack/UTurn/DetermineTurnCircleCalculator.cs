using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.UTurn
{
    [UsedImplicitly]
    public class DetermineTurnCircleCalculator : IDetermineTurnCircleCalculator
    {
        public void Calculate()
        {
            StartTurnCircle = DetermineStartTurnCircle(Settings,
                                                       UTurnCircle);
            FinishTurnCircle = DetermineFinishTurnCircle(Settings,
                                                         UTurnCircle);
        }

        public ISettings Settings { get; set; } = Racetrack.Settings.Unknown;

        public IUTurnCircle UTurnCircle { get; set; } = UTurn.UTurnCircle.Unknown;

        public ITurnCircle StartTurnCircle { get; private set; } = TurnCircle.Unknown;

        public ITurnCircle FinishTurnCircle { get; private set; } = TurnCircle.Unknown;

        [NotNull]
        [UsedImplicitly]
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

        [NotNull]
        [UsedImplicitly]
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
    }
}