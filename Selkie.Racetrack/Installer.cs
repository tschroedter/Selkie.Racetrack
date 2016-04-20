using System.Diagnostics.CodeAnalysis;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Selkie.Common;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Racetrack.Interfaces.Converters;

namespace Selkie.Racetrack
{
    [ExcludeFromCodeCoverage]
    public class Installer : SelkieInstaller <Installer>
    {
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