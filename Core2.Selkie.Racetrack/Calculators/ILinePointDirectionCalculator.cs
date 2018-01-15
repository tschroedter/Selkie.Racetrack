using Core2.Selkie.Geometry;

namespace Core2.Selkie.Racetrack.Calculators
{
    public interface ILinePointDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}