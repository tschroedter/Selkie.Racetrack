using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.UTurn;

namespace Core2.Selkie.Racetrack.Tests.Calculators.NUnit
{
    [TestFixture]
    internal sealed class PathSelectorTests
    {
        [SetUp]
        public void Setup()
        {
            m_Paths = new[]
                      {
                          Substitute.For <IPath>()
                      };
            m_PathsUTurn = Substitute.For <IPath>();

            m_Settings = Substitute.For <ISettings>();
            m_Converter = Substitute.For <ITurnCirclePairsToPathsConverter>();
            m_UTurnPath = Substitute.For <IUTurnPath>();

            m_Converter.Paths.Returns(m_Paths);
            m_UTurnPath.Path.Returns(m_PathsUTurn);

            m_Calculator = new PathSelectorCalculator(m_Converter,
                                                      m_UTurnPath)
                           {
                               Settings = m_Settings
                           };
        }

        private PathSelectorCalculator m_Calculator;
        private ITurnCirclePairsToPathsConverter m_Converter;
        private IPath[] m_Paths;
        private IPath m_PathsUTurn;
        private ISettings m_Settings;
        private IUTurnPath m_UTurnPath;

        [Test]
        public void CalculateCallsConvertOfConverterTest()
        {
            m_Calculator.Calculate();

            m_Converter.Received().Convert();
        }

        [Test]
        public void CalculateCallsConvertOfUTurnPathTest()
        {
            m_Calculator.Calculate();

            m_UTurnPath.Received().Calculate();
        }

        [Test]
        public void CalculateSetsPathsCountTest()
        {
            m_Calculator.Calculate();

            IEnumerable <IPath> actual = m_Calculator.Paths;

            Assert.AreEqual(1,
                            actual.Count());
        }

        [Test]
        public void CalculateSetsPathsTest()
        {
            m_Calculator.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Calculator.Paths);
        }

        [Test]
        public void CalculateSetsSettingForConverterTest()
        {
            m_Calculator.Calculate();

            Assert.AreEqual(m_Settings,
                            m_Converter.Settings);
        }

        [Test]
        public void CalculateSetsSettingForUTurnPathTest()
        {
            m_Calculator.Calculate();

            Assert.AreEqual(m_Settings,
                            m_UTurnPath.Settings);
        }

        [Test]
        public void PathsDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.Paths.Count());
        }

        [Test]
        public void SelectPathsReturnsPathsForUTurnPathIsRequiredIsFalseTest()
        {
            m_UTurnPath.IsRequired.Returns(false);

            IEnumerable <IPath> actual = m_Calculator.SelectPaths(m_UTurnPath,
                                                                  m_Converter);

            Assert.AreEqual(m_Paths,
                            actual);
        }

        [Test]
        public void SelectPathsReturnsPathsForUTurnPathIsRequiredIsTrueAndUTurnAllowedIsFalseTest()
        {
            m_Settings.IsPortTurnAllowed.Returns(false);
            m_Settings.IsStarboardTurnAllowed.Returns(false);

            m_UTurnPath.IsRequired.Returns(true);

            IEnumerable <IPath> actual = m_Calculator.SelectPaths(m_UTurnPath,
                                                                  m_Converter);

            Assert.AreEqual(m_Paths,
                            actual);
        }

        [Test]
        public void SelectPathsReturnsPathsForUTurnPathIsRequiredIsTrueAndUTurnAllowedIsTrueTest()
        {
            m_Settings.IsPortTurnAllowed.Returns(true);
            m_Settings.IsStarboardTurnAllowed.Returns(true);

            m_UTurnPath.IsRequired.Returns(true);

            IEnumerable <IPath> selectPaths = m_Calculator.SelectPaths(m_UTurnPath,
                                                                       m_Converter);
            IPath actual = selectPaths.First();

            Assert.AreEqual(m_PathsUTurn,
                            actual);
        }

        [Test]
        public void SettingsDefaultTest()
        {
            var calculator = new PathSelectorCalculator(m_Converter,
                                                        m_UTurnPath);

            Assert.True(calculator.Settings.IsUnknown);
        }

        [Test]
        public void SettingsRoundtripTest()
        {
            var calculator = new PathSelectorCalculator(m_Converter,
                                                        m_UTurnPath);

            var settings = Substitute.For <ISettings>();

            calculator.Settings = settings;

            Assert.AreEqual(settings,
                            calculator.Settings);
        }

        [Test]
        public void UTurnAllowedReturnsFalseForIsPortTurnAllowedIsFalseTest()
        {
            m_Settings.IsPortTurnAllowed.Returns(false);
            m_Settings.IsStarboardTurnAllowed.Returns(true);

            Assert.False(m_Calculator.IsUTurnAllowed);
        }

        [Test]
        public void UTurnAllowedReturnsFalseForIsStarboardTurnAllowedIsFalseTest()
        {
            m_Settings.IsPortTurnAllowed.Returns(true);
            m_Settings.IsStarboardTurnAllowed.Returns(false);

            Assert.False(m_Calculator.IsUTurnAllowed);
        }

        [Test]
        public void UTurnAllowedReturnsTrueForBothAreTrueTest()
        {
            m_Settings.IsPortTurnAllowed.Returns(true);
            m_Settings.IsStarboardTurnAllowed.Returns(true);

            Assert.True(m_Calculator.IsUTurnAllowed);
        }
    }
}