using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public abstract class BaseRacetrackCalculator : IBaseRacetrackCalculator
    {
        protected BaseRacetrackCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
        {
            IsPortTurnAllowed = true;
            IsStarboardTurnAllowed = true;
            Paths = new IPath[]
                    {
                    };
            FromLine = Line.Unknown;
            ToLines = new ILine[]
                      {
                      };
            TurnRadiusForPort = Distance.Unknown;
            TurnRadiusForStarboard = Distance.Unknown;
            Calculator = calculator;
        }

        public ILinePairToRacetrackCalculator Calculator { get; private set; }

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public IPath[] Paths { get; private set; }

        public ILine FromLine { get; set; }

        public ILine[] ToLines { get; set; }

        public Distance TurnRadiusForPort { get; set; }

        public Distance TurnRadiusForStarboard { get; set; }

        public void Calculate()
        {
            Paths = CalculateRacetracks();
        }

        [NotNull]
        internal abstract ILinePairToRacetrackCalculator GetCalculator([NotNull] ILine fromLine,
                                                                       [NotNull] ILine toLine,
                                                                       [NotNull] Distance radiusForPortTurn,
                                                                       [NotNull] Distance radiusForStarboardTurn);

        [NotNull]
        private IPath[] CalculateRacetracks()
        {
            var racetracks = new IPath[ToLines.Length];

            for ( var i = 0 ; i < ToLines.Length ; i++ )
            {
                ILine toLine = ToLines [ i ];
                IPath racetrack;

                if ( FromLine.Equals(toLine) )
                {
                    racetrack = Path.Unknown;
                }
                else
                {
                    ILinePairToRacetrackCalculator linePairToRacetrackCalculator = GetCalculator(FromLine,
                                                                                                 toLine,
                                                                                                 TurnRadiusForPort,
                                                                                                 TurnRadiusForStarboard);

                    racetrack = linePairToRacetrackCalculator.Racetrack;
                }

                racetracks [ i ] = racetrack;
            }

            return racetracks;
        }
    }
}