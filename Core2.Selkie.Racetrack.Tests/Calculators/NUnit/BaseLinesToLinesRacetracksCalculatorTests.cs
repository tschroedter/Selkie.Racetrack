using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Tests.Calculators.NUnit
{
    // ReSharper disable ClassTooBig
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class BaseLinesToLinesRacetracksCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_FromLine = Substitute.For <ILine>();
            m_ToLine = Substitute.For <ILine>();
            m_ToLines = new[]
                        {
                            m_FromLine,
                            m_ToLine
                        };
            m_Radius = new Distance(100.0);
            m_PathOne = Substitute.For <IPath>();
            m_PathTwo = Substitute.For <IPath>();
            m_PathsOneTwo = new[]
                            {
                                m_PathOne,
                                m_PathTwo
                            };

            m_RacetrackCalculator = Substitute.For <IBaseRacetrackCalculator>();
            m_RacetrackCalculator.Paths.Returns(m_PathsOneTwo);

            m_Calculator = new TestBaseLinesToLinesRacetracksCalculator(m_RacetrackCalculator);
        }

        private BaseLinesToLinesRacetracksCalculator m_Calculator;
        private ILine m_FromLine;
        private IPath m_PathOne;
        private IPath[] m_PathsOneTwo;
        private IPath m_PathTwo;
        private IBaseRacetrackCalculator m_RacetrackCalculator;
        private Distance m_Radius;
        private ILine m_ToLine;
        private ILine[] m_ToLines;

        private class TestBaseLinesToLinesRacetracksCalculator : BaseLinesToLinesRacetracksCalculator
        {
            public TestBaseLinesToLinesRacetracksCalculator([NotNull] IBaseRacetrackCalculator calculator)
                : base(calculator)
            {
            }

            protected override ILine GetFromLine(ILine toLine)
            {
                return toLine;
            }
        }

        [Test]
        public void CalculateSetsPathsCountTest()
        {
            m_Calculator.Lines = m_ToLines;

            m_Calculator.Calculate();

            IPath[][] actual = m_Calculator.Paths;

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateSetsPathsForFirstTest()
        {
            m_Calculator.Lines = m_ToLines;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 0 ];

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateSetsPathsForFirstValueTest()
        {
            m_Calculator.Lines = m_ToLines;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 0 ];

            Assert.AreEqual(m_PathOne,
                            actual [ 0 ],
                            "[0]");
            Assert.AreEqual(m_PathTwo,
                            actual [ 1 ],
                            "[1]");
        }

        [Test]
        public void CalculateSetsPathsForSecondTest()
        {
            m_Calculator.Lines = m_ToLines;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 1 ];

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateSetsPathsForSecondValueTest()
        {
            m_Calculator.Lines = m_ToLines;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 1 ];

            Assert.AreEqual(m_PathOne,
                            actual [ 0 ],
                            "[0]");
            Assert.AreEqual(m_PathTwo,
                            actual [ 1 ],
                            "[1]");
        }

        [Test]
        public void CallCalculatorCallsCalculateTest()
        {
            m_Calculator.CallCalculator(m_FromLine,
                                        m_ToLines);

            m_RacetrackCalculator.Received().Calculate();
        }

        [Test]
        public void CallCalculatorReturnsPathsTest()
        {
            IPath[] actual = m_Calculator.CallCalculator(m_FromLine,
                                                         m_ToLines);

            Assert.AreEqual(m_PathsOneTwo,
                            actual);
        }

        [Test]
        public void CallCalculatorSetsFromLineTest()
        {
            m_Calculator.CallCalculator(m_FromLine,
                                        m_ToLines);

            Assert.AreEqual(m_FromLine,
                            m_RacetrackCalculator.FromLine);
        }

        [Test]
        public void CallCalculatorSetsIsPortTurnAllowedTest()
        {
            m_Calculator.IsPortTurnAllowed = true;

            m_Calculator.CallCalculator(m_FromLine,
                                        m_ToLines);

            Assert.True(m_RacetrackCalculator.IsPortTurnAllowed);
        }

        [Test]
        public void CallCalculatorSetsIsStarboardTurnAllowedTest()
        {
            m_Calculator.IsStarboardTurnAllowed = true;

            m_Calculator.CallCalculator(m_FromLine,
                                        m_ToLines);

            Assert.True(m_RacetrackCalculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void CallCalculatorSetsToLinesTest()
        {
            m_Calculator.CallCalculator(m_FromLine,
                                        m_ToLines);

            Assert.AreEqual(m_ToLines,
                            m_RacetrackCalculator.ToLines);
        }

        [Test]
        public void CallCalculatorSetsTurnRadiusForPortTest()
        {
            m_Calculator.TurnRadiusForPort = m_Radius;

            m_Calculator.CallCalculator(m_FromLine,
                                        m_ToLines);

            Assert.AreEqual(m_Radius,
                            m_RacetrackCalculator.TurnRadiusForPort);
        }

        [Test]
        public void CallCalculatorSetsTurnRadiusForStarboardTest()
        {
            m_Calculator.TurnRadiusForStarboard = m_Radius;

            m_Calculator.CallCalculator(m_FromLine,
                                        m_ToLines);

            Assert.AreEqual(m_Radius,
                            m_RacetrackCalculator.TurnRadiusForStarboard);
        }

        [Test]
        public void IsPortTurnAllowedDefaultTest()
        {
            Assert.True(m_Calculator.IsPortTurnAllowed);
        }

        [Test]
        public void IsPortTurnAllowedRoundtripTest()
        {
            m_Calculator.IsPortTurnAllowed = false;

            Assert.False(m_Calculator.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedDefaultTest()
        {
            Assert.True(m_Calculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedRoundtripTest()
        {
            m_Calculator.IsStarboardTurnAllowed = false;

            Assert.False(m_Calculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void LinesDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.Lines.Count());
        }

        [Test]
        public void LinesRoundtripTest()
        {
            var line = Substitute.For <ILine>();
            ILine[] lines =
            {
                line
            };

            m_Calculator.Lines = lines;

            Assert.AreEqual(lines,
                            m_Calculator.Lines);
        }

        [Test]
        public void PathsDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.Paths.GetLength(0));
        }

        [Test]
        public void RadiusRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForStarboard = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForStarboard);
        }

        [Test]
        public void TurnRadiusForPortTurnInMetresDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForPort.IsUnknown);
        }

        [Test]
        public void TurnRadiusForPortTurnInMetresRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForPort = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForPort);
        }

        [Test]
        public void TurnRadiusForStarboardTurnInMetresDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForStarboard.IsUnknown);
        }
    }
}