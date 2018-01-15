using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
{
    public interface ICalculatorFactory
    {
        T Create <T>() where T : ICalculator;
        void Release([NotNull] ICalculator calculator);
    }
}