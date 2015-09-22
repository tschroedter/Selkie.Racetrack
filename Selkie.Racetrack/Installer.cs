using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Converter;
using Selkie.Windsor;

namespace Selkie.Racetrack
{
    public class Installer : BaseInstaller <Installer>
    {
        public override string GetPrefixOfDllsToInstall()
        {
            return "Selkie.";
        }

        protected override void InstallComponents(IWindsorContainer container,
                                                  IConfigurationStore store)
        {
            container.Register(Component.For <ICalculatorFactory>().AsFactory());

            // ReSharper disable MaximumChainedReferences
            container.Register(
                               Classes.FromThisAssembly()
                                      .BasedOn <ICalculator>()
                                      .WithServiceFromInterface(typeof ( ICalculator ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));
            // ReSharper restore MaximumChainedReferences

            // ReSharper disable MaximumChainedReferences
            container.Register(
                               Classes.FromThisAssembly()
                                      .BasedOn <IConverter>()
                                      .WithServiceFromInterface(typeof ( IConverter ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));
            // ReSharper restore MaximumChainedReferences        
        }
    }
}