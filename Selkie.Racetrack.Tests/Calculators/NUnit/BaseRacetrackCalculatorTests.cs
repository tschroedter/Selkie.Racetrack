﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Tests.Calculators.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class BaseRacetrackCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_LineOne = Substitute.For <ILine>();
            m_LineTwo = Substitute.For <ILine>();
            // ReSharper disable MaximumChainedReferences
            m_LineOne.Equals(m_LineOne).Returns(true);
            m_LineOne.Equals(m_LineTwo).Returns(false);
            // ReSharper enable MaximumChainedReferences

            m_Racetrack = Substitute.For <IPath>();

            m_LinePairToRacetrackCalculator = Substitute.For <ILinePairToRacetrackCalculator>();
            m_LinePairToRacetrackCalculator.Racetrack.Returns(m_Racetrack);

            m_Calculator = new TestBaseRacetrackCalculator(m_LinePairToRacetrackCalculator);
        }

        private class TestBaseRacetrackCalculator : BaseRacetrackCalculator
        {
            public TestBaseRacetrackCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
                : base(calculator)
            {
            }

            internal override ILinePairToRacetrackCalculator GetCalculator(ILine fromLine,
                                                                           ILine toLine,
                                                                           Distance radiusForPortTurn,
                                                                           Distance radiusForStarboardTurn)
            {
                return Calculator;
            }
        }

        private TestBaseRacetrackCalculator m_Calculator;
        private ILinePairToRacetrackCalculator m_LinePairToRacetrackCalculator;
        private ILine m_LineOne;
        private ILine m_LineTwo;
        private IPath m_Racetrack;

        [Test]
        public void CalculateForLinesEmptyTest()
        {
            ILine[] lines =
            {
            };

            m_Calculator.ToLines = lines;
            m_Calculator.FromLine = m_LineOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            IPath[] actual = m_Calculator.Paths;

            Assert.AreEqual(0,
                            actual.Length);
        }

        [Test]
        public void CalculateForOneLineOnlyCountTest()
        {
            ILine[] lines =
            {
                m_LineOne
            };

            m_Calculator.ToLines = lines;
            m_Calculator.FromLine = m_LineOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths;

            Assert.AreEqual(1,
                            actual.Length);
        }

        [Test]
        public void CalculateForOneLineOnlyUnknownTest()
        {
            ILine[] lines =
            {
                m_LineOne
            };

            m_Calculator.ToLines = lines;
            m_Calculator.FromLine = m_LineOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath actual = m_Calculator.Paths.First();

            Assert.True(actual.IsUnknown);
        }

        [Test]
        public void CalculateForTwoLinesCountTest()
        {
            ILine[] lines =
            {
                m_LineOne,
                m_LineTwo
            };

            m_Calculator.ToLines = lines;
            m_Calculator.FromLine = m_LineOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths;

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateForTwoLinesPathOneTest()
        {
            ILine[] lines =
            {
                m_LineOne,
                m_LineTwo
            };

            m_Calculator.ToLines = lines;
            m_Calculator.FromLine = m_LineOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath actual = m_Calculator.Paths.First();

            Assert.True(actual.IsUnknown);
        }

        [Test]
        public void CalculateForTwoLinesPathTwoTest()
        {
            ILine[] lines =
            {
                m_LineOne,
                m_LineTwo
            };

            m_Calculator.ToLines = lines;
            m_Calculator.FromLine = m_LineOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath actual = m_Calculator.Paths.Last();

            Assert.AreEqual(m_Racetrack,
                            actual);
        }

        [Test]
        public void CalculatorDefaultTest()
        {
            Assert.AreEqual(m_LinePairToRacetrackCalculator,
                            m_Calculator.Calculator);
        }

        [Test]
        public void FromLineDefaultTest()
        {
            Assert.True(m_Calculator.FromLine.IsUnknown);
        }

        [Test]
        public void FromLineRoundtripTest()
        {
            var line = Substitute.For <ILine>();

            m_Calculator.FromLine = line;

            Assert.AreEqual(line,
                            m_Calculator.FromLine);
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
        public void TurnRadiusForPortDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForPort.IsUnknown);
        }

        [Test]
        public void TurnRadiusForStarboardDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForStarboard.IsUnknown);
        }

        [Test]
        public void TurnRadiusForPortRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForPort = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForPort);
        }

        [Test]
        public void TurnRadiusForStarboardRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForStarboard = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForStarboard);
        }

        [Test]
        public void ToLineRoundtripTest()
        {
            var line = Substitute.For <ILine>();
            ILine[] lines =
            {
                line
            };

            m_Calculator.ToLines = lines;

            Assert.AreEqual(lines,
                            m_Calculator.ToLines);
        }

        [Test]
        public void ToLinesDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.ToLines.Length);
        }
    }
}