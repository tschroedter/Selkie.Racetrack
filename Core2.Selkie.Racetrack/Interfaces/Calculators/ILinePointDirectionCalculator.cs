using Core2.Selkie.Geometry;

// ReSharper disable UnusedMemberInSuper.Global

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface ILinePointDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}