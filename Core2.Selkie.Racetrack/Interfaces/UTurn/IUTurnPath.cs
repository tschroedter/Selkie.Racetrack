using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.UTurn
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