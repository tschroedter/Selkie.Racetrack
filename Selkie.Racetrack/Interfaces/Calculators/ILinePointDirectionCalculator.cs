using Selkie.Geometry;

namespace Selkie.Racetrack.Interfaces.Calculators
{
    public interface ILinePointDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}