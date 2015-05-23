using JetBrains.Annotations;
using Selkie.Racetrack.Calculators;

namespace Selkie.Racetrack.UTurn
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
    }
}