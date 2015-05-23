using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;
using Selkie.Racetrack.UTurn;

namespace Selkie.Racetrack.Tests.UTurn.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class DetermineTurnCircleCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_StartPoint = new Point(-50.0,
                                     0.0);
            m_StartAzimuth = Angle.FromDegrees(90.0);
            m_FinishPoint = new Point(0.0,
                                      0.0);
            m_FinishAzimuth = Angle.FromDegrees(270.0);
            m_Radius = new Distance(100.0);

            m_Settings = new Settings(m_StartPoint,
                                      m_StartAzimuth,
                                      m_FinishPoint,
                                      m_FinishAzimuth,
                                      m_Radius,
                                      true,
                                      true);

            var possibleTurnCircles = new PossibleTurnCircles
                                      {
                                          Settings = m_Settings
                                      };

            var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                      {
                                          Settings = m_Settings
                                      };
            determineCirclePair.Calculate();

            var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair);

            m_UTurnCircle = new UTurnCircle(possibleTurnCircles,
                                            uTurnCircleCalculator)
                            {
                                Settings = m_Settings
                            };
            m_UTurnCircle.Calculate();

            m_Calculator = new DetermineTurnCircleCalculator
                           {
                               Settings = m_Settings,
                               UTurnCircle = m_UTurnCircle
                           };

            m_Calculator.Calculate();
        }

        private DetermineTurnCircleCalculator m_Calculator;
        private Angle m_FinishAzimuth;
        private Point m_FinishPoint;
        private Distance m_Radius;
        private ISettings m_Settings;
        private Angle m_StartAzimuth;
        private Point m_StartPoint;
        private IUTurnCircle m_UTurnCircle;

        [Test]
        public void DetermineFinishTurnCircleReturnsOneForFinishPointIsOnOneCircleTest()
        {
            var zeroTurnCircle = Substitute.For <ITurnCircle>();
            // ReSharper disable once MaximumChainedReferences
            zeroTurnCircle.IsPointOnCircle(Arg.Any <Point>()).ReturnsForAnyArgs(false);

            var oneTurnCircle = Substitute.For <ITurnCircle>();

            var uTurnCircle = Substitute.For <IUTurnCircle>();
            uTurnCircle.Zero.Returns(zeroTurnCircle);
            uTurnCircle.One.Returns(oneTurnCircle);

            ITurnCircle actual = m_Calculator.DetermineFinishTurnCircle(m_Settings,
                                                                        uTurnCircle);

            Assert.AreEqual(oneTurnCircle,
                            actual);
        }

        [Test]
        public void DetermineFinishTurnCircleReturnsZeroForFinishPointIsOnZeroCircleTest()
        {
            var zeroTurnCircle = Substitute.For <ITurnCircle>();
            // ReSharper disable once MaximumChainedReferences
            zeroTurnCircle.IsPointOnCircle(Arg.Any <Point>()).ReturnsForAnyArgs(true);

            var uTurnCircle = Substitute.For <IUTurnCircle>();
            uTurnCircle.Zero.Returns(zeroTurnCircle);

            ITurnCircle actual = m_Calculator.DetermineFinishTurnCircle(m_Settings,
                                                                        uTurnCircle);

            Assert.AreEqual(zeroTurnCircle,
                            actual);
        }

        [Test]
        public void DetermineStartTurnCircleReturnsOneForStartPointIsOnOneCircleTest()
        {
            var zeroTurnCircle = Substitute.For <ITurnCircle>();
            // ReSharper disable once MaximumChainedReferences
            zeroTurnCircle.IsPointOnCircle(Arg.Any <Point>()).ReturnsForAnyArgs(false);

            var oneTurnCircle = Substitute.For <ITurnCircle>();

            var uTurnCircle = Substitute.For <IUTurnCircle>();
            uTurnCircle.Zero.Returns(zeroTurnCircle);
            uTurnCircle.One.Returns(oneTurnCircle);

            ITurnCircle actual = m_Calculator.DetermineStartTurnCircle(m_Settings,
                                                                       uTurnCircle);

            Assert.AreEqual(oneTurnCircle,
                            actual);
        }

        [Test]
        public void DetermineStartTurnCircleReturnsZeroForFinishPointIsOnZeroCircleTest()
        {
            var zeroTurnCircle = Substitute.For <ITurnCircle>();
            // ReSharper disable once MaximumChainedReferences
            zeroTurnCircle.IsPointOnCircle(Arg.Any <Point>()).ReturnsForAnyArgs(true);

            var uTurnCircle = Substitute.For <IUTurnCircle>();
            uTurnCircle.Zero.Returns(zeroTurnCircle);

            ITurnCircle actual = m_Calculator.DetermineStartTurnCircle(m_Settings,
                                                                       uTurnCircle);

            Assert.AreEqual(zeroTurnCircle,
                            actual);
        }

        [Test]
        public void FinishTurnCircleDefaultTest()
        {
            var calculator = new DetermineTurnCircleCalculator();

            Assert.True(calculator.FinishTurnCircle.IsUnknown);
        }

        [Test]
        public void FinishTurnCircleTest()
        {
            Assert.AreEqual(new Point(100.0,
                                      0.0),
                            m_Calculator.FinishTurnCircle.CentrePoint);
            Assert.AreEqual(Constants.CircleOrigin.Finish,
                            m_Calculator.FinishTurnCircle.Origin,
                            "Origin");
            Assert.AreEqual(Constants.CircleSide.Port,
                            m_Calculator.FinishTurnCircle.Side,
                            "Side");
        }

        [Test]
        public void SettingsDefaultTest()
        {
            var calculator = new DetermineTurnCircleCalculator();

            Assert.True(calculator.Settings.IsUnknown);
        }

        [Test]
        public void SettingsRoundtripTest()
        {
            var settings = Substitute.For <ISettings>();

            m_Calculator.Settings = settings;

            Assert.AreEqual(settings,
                            m_Calculator.Settings);
        }

        [Test]
        public void StartTurnCircleDefaultTest()
        {
            var calculator = new DetermineTurnCircleCalculator();

            Assert.True(calculator.StartTurnCircle.IsUnknown);
        }

        [Test]
        public void StartTurnCircleTest()
        {
            Assert.AreEqual(new Point(-150.0,
                                      0.0),
                            m_Calculator.StartTurnCircle.CentrePoint,
                            "CentrePoint");
            Assert.AreEqual(Constants.CircleOrigin.Start,
                            m_Calculator.StartTurnCircle.Origin,
                            "Origin");
            Assert.AreEqual(Constants.CircleSide.Port,
                            m_Calculator.StartTurnCircle.Side,
                            "Side");
        }

        [Test]
        public void UTurnCircleDefaultTest()
        {
            var calculator = new DetermineTurnCircleCalculator();

            Assert.True(calculator.UTurnCircle.IsUnknown);
        }

        [Test]
        public void UTurnCircleRoundtripTest()
        {
            var uTurnCircle = Substitute.For <IUTurnCircle>();

            m_Calculator.UTurnCircle = uTurnCircle;

            Assert.AreEqual(uTurnCircle,
                            m_Calculator.UTurnCircle);
        }
    }
}