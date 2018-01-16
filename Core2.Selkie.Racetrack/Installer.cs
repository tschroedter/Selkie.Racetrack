using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core2.Selkie.Common;
using Core2.Selkie.Geometry.ThreeD.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using ICalculator = Core2.Selkie.Geometry.Calculators.ICalculator;

[assembly: InternalsVisibleTo("Core2.Selkie.Racetrack.Tests")]

namespace Core2.Selkie.Racetrack
{
    [ExcludeFromCodeCoverage]
    public class Installer : SelkieInstaller <Installer>
    {
        protected override void InstallComponents(IWindsorContainer container,
                                                  IConfigurationStore store)
        {
            container.Register(Component.For <ICalculatorFactory>().AsFactory());

            Assembly assembly = Assembly.GetAssembly(typeof(Installer));

            container.Register(
                               Classes.FromAssembly(assembly)
                                      .BasedOn <ICalculator>()
                                      .WithServiceFromInterface(typeof( ICalculator ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));
            container.Register(
                               Classes.FromAssembly(assembly)
                                      .BasedOn <IConverter>()
                                      .WithServiceFromInterface(typeof( IConverter ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));
        }
    }
}