using JetBrains.Annotations;

namespace Selkie.Racetrack.Calculators
{
    public interface ICalculatorFactory
    {
        T Create <T>() where T : ICalculator;
        void Release([NotNull] ICalculator calculator);
    }
}