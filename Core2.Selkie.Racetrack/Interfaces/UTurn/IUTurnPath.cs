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
        [UsedImplicitly]
        ISettings Settings { get; set; }

        [NotNull]
        [UsedImplicitly]
        IUTurnCircle UTurnCircle { get; }

        [UsedImplicitly]
        ISettings UTrunCircleSettings { get; }
    }
}