using Core2.Selkie.Geometry;

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface IRacetrackLineDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}