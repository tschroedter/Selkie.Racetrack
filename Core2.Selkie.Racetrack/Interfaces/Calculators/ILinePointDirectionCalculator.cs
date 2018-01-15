using Core2.Selkie.Geometry;

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface ILinePointDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}