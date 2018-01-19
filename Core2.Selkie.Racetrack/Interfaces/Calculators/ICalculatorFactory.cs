using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface ICalculatorFactory
    {
        T Create <T>()
            where T : ICalculator;

        void Release([NotNull] ICalculator calculator);
    }
}