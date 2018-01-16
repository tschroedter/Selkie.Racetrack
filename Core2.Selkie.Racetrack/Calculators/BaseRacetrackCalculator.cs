using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
{
    public abstract class BaseRacetrackCalculator : IBaseRacetrackCalculator
    {
        protected BaseRacetrackCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
        {
            IsPortTurnAllowed = true;
            IsStarboardTurnAllowed = true;
            Paths = new IPath[0];
            FromLine = Line.Unknown;
            ToLines = new ILine[0];
            TurnRadiusForPort = Distance.Unknown;
            TurnRadiusForStarboard = Distance.Unknown;
            Calculator = calculator;
        }

        public ILinePairToRacetrackCalculator Calculator { get; }

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
                    ILinePairToRacetrackCalculator calculator = GetCalculator(FromLine,
                                                                              toLine,
                                                                              TurnRadiusForPort,
                                                                              TurnRadiusForStarboard);

                    racetrack = calculator.Racetrack;
                }

                racetracks [ i ] = racetrack;
            }

            return racetracks;
        }
    }
}