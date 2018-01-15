using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.UTurn
{
    public interface IUTurnPath : ICalculator
    {
        [NotNull]
        IPath Path { get; }

        bool IsRequired { get; }

        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        IUTurnCircle UTurnCircle { get; }

        ISettings UTrunCircleSettings { get; }
    }
}