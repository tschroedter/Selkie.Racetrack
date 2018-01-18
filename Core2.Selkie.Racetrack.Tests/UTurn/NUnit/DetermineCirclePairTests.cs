using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Turn;
using Core2.Selkie.Racetrack.UTurn;

namespace Core2.Selkie.Racetrack.Tests.UTurn.NUnit
{
    
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class DetermineCirclePairTests
    {
        [SetUp]
        public void Setup()
        {
            var settings = new Settings(new Point(-50.0,
                                                  0.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            m_PairsCalculator = CreateDetermineCirclePair(settings,
                                                          possibleTurnCircles);
        }

        private DetermineCirclePairCalculator m_PairsCalculator;

        [NotNull]
        private DetermineCirclePairCalculator CreateDetermineCirclePair([NotNull] Settings settings,
                                                                        [NotNull] PossibleTurnCircles
                                                                            possibleTurnCircles)
        {
            var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                      {
                                          Settings = settings
                                      };
            determineCirclePair.Calculate();

            return determineCirclePair;
        }

        [Test]
        public void DetermineCirclePairForFinishPointAboveTest()
        {
            var settings = new Settings(new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(0.0,
                                                  50.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCircleStarboard,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCircleStarboard,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForFinishPointBelowTest()
        {
            var settings = new Settings(new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(0.0,
                                                  -55.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForFinishPointToLeftTest()
        {
            var settings = new Settings(new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(90.0),
                                        new Point(-50.0,
                                                  0.0),
                                        Angle.FromDegrees(270.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCircleStarboard,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCircleStarboard,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForFinishPointToRightTest()
        {
            var settings = new Settings(new Point(-50.0,
                                                  0.0),
                                        Angle.FromDegrees(90.0),
                                        new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(270.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPoint45DegreesCaseQ1ToFinishPointTest()
        {
            var settings = new Settings(new Point(50.0,
                                                  50.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPoint45DegreesCaseQ2ToFinishPointTest()
        {
            var settings = new Settings(new Point(-50.0,
                                                  50.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPoint45DegreesCaseQ3ToFinishPointTest()
        {
            var settings = new Settings(new Point(-50.0,
                                                  -50.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCircleStarboard,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCircleStarboard,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPoint45DegreesCaseQ4ToFinishPointTest()
        {
            var settings = new Settings(new Point(50.0,
                                                  -50.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCircleStarboard,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCircleStarboard,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointEqualsFinishPointTest()
        {
            var settings = new Settings(new Point(-50.0,
                                                  0.0),
                                        Angle.FromDegrees(0.0),
                                        new Point(-50.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCircleStarboard,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCircleStarboard,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseOneQ1Test()
        {
            var settings = new Settings(new Point(50.0,
                                                  50.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(25.0,
                                                  25.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseOneQ2Test()
        {
            var settings = new Settings(new Point(-50.0,
                                                  50.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(-100.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseOneQ3Test()
        {
            var settings = new Settings(new Point(0.0,
                                                  0.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(-50.0,
                                                  -50.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseOneQ4Test()
        {
            var settings = new Settings(new Point(50.0,
                                                  0.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(0.0,
                                                  -50.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };

            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseTwoQ1Test()
        {
            var settings = new Settings(new Point(25.0,
                                                  25.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(50.0,
                                                  50.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCircleStarboard,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCircleStarboard,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseTwoQ2Test()
        {
            var settings = new Settings(new Point(-50.0,
                                                  50.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(-75.0,
                                                  25.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseTwoQ3Test()
        {
            var settings = new Settings(new Point(-50.0,
                                                  50.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(-100.0,
                                                  -100.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo180DegreesToFinishPointCaseTwoQ4Test()
        {
            var settings = new Settings(new Point(50.0,
                                                  -50.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(25.0,
                                                  -75.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo90DegreesToFinishPointCaseFourQ1Test()
        {
            var settings = new Settings(new Point(50.0,
                                                  50.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(100.0,
                                                  0.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCirclePort,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCirclePort,
                            actual.One,
                            "One");
        }

        [Test]
        public void DetermineCirclePairForStartPointTo90DegreesToFinishPointCaseThreeQ1Test()
        {
            var settings = new Settings(new Point(50.0,
                                                  50.0),
                                        Angle.FromDegrees(45.0),
                                        new Point(0.0,
                                                  100.0),
                                        Angle.FromDegrees(180.0),
                                        new Distance(100.0),
                                        new Distance(100.0),
                                        true,
                                        true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = settings
                                      };
            possibleTurnCircles.Calculate();

            ITurnCirclePair actual = m_PairsCalculator.Determine(settings,
                                                                 possibleTurnCircles);

            Assert.AreEqual(possibleTurnCircles.FinishTurnCircleStarboard,
                            actual.Zero,
                            "Zero");
            Assert.AreEqual(possibleTurnCircles.StartTurnCircleStarboard,
                            actual.One,
                            "One");
        }

        [Test]
        public void OneDefaultTest()
        {
            var calculator = new DetermineCirclePairCalculator(Substitute.For <IPossibleTurnCircles>());

            Assert.True(calculator.One.IsUnknown);
        }

        [Test]
        public void PairDefaultTest()
        {
            var calculator = new DetermineCirclePairCalculator(Substitute.For <IPossibleTurnCircles>());

            Assert.True(calculator.Pair.IsUnknown);
        }

        [Test]
        public void SettingsDefaultTest()
        {
            var calculator = new DetermineCirclePairCalculator(Substitute.For <IPossibleTurnCircles>());

            Assert.True(calculator.Settings.IsUnknown);
        }

        [Test]
        public void SettingsRoundtripTest()
        {
            var calculator = new DetermineCirclePairCalculator(Substitute.For <IPossibleTurnCircles>());

            var settings = Substitute.For <ISettings>();

            calculator.Settings = settings;

            Assert.AreEqual(settings,
                            calculator.Settings);
        }

        [Test]
        public void ZeroDefaultTest()
        {
            var calculator = new DetermineCirclePairCalculator(Substitute.For <IPossibleTurnCircles>());

            Assert.True(calculator.Zero.IsUnknown);
        }
    }
}