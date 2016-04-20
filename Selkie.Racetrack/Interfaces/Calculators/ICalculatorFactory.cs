using JetBrains.Annotations;

namespace Selkie.Racetrack.Interfaces.Calculators
{
    public interface ICalculatorFactory
    {
        T Create <T>() where T : ICalculator;
        void Release([NotNull] ICalculator calculator);
    }
}