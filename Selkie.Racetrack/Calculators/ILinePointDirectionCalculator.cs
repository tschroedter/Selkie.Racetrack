using Selkie.Geometry;

namespace Selkie.Racetrack.Calculators
{
    public interface ILinePointDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}