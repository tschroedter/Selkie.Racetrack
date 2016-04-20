using Selkie.Geometry;

namespace Selkie.Racetrack.Interfaces.Calculators
{
    public interface IRacetrackLineDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}