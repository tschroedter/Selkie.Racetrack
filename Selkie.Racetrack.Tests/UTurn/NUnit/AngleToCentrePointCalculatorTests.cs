using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;
using Selkie.Racetrack.UTurn;

namespace Selkie.Racetrack.Tests.UTurn.NUnit
{
    // ReSharper disable ClassTooBig
    [ExcludeFromCodeCoverage]
    internal sealed class AngleToCentrePointCalculatorTests
    {
        #region Nested type: AngleToCentrePointCalculatorCaseOneTests

        [TestFixture]
        internal sealed class AngleToCentrePointCalculatorCaseOneTests
        {
            [SetUp]
            public void Setup()
            {
                var settings = Substitute.For <ISettings>();

                var zeroCentrePoint = new Point(200.0,
                                                -30.0);
                var oneCentrePoint = new Point(200.0,
                                               60.0);

                m_Zero = new TurnCircle(new Circle(zeroCentrePoint,
                                                   30.0),
                                        Constants.CircleSide.Starboard,
                                        Constants.CircleOrigin.Start,
                                        Constants.TurnDirection.Clockwise);

                m_One = new TurnCircle(new Circle(oneCentrePoint,
                                                  30.0),
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Finish,
                                       Constants.TurnDirection.Clockwise);

                var pair = new TurnCirclePair(settings,
                                              m_Zero,
                                              m_One);

                m_Calculator = new AngleToCentrePointCalculator(pair);
            }

            private AngleToCentrePointCalculator m_Calculator;
            private TurnCircle m_One;
            private TurnCircle m_Zero;

            [Test]
            public void CentrePointTest()
            {
                var expected = new Point(239.69,
                                         15.0);
                Point actual = m_Calculator.CentrePoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IntersectionPointForTurnCircleOneTest()
            {
                var expected = new Point(219.84,
                                         37.5);
                Point actual = m_Calculator.IntersectionPointForTurnCircle(m_One);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IntersectionPointForTurnCircleUnknownTest()
            {
                var two = new TurnCircle(new Circle(-33.0,
                                                    33.0,
                                                    3.3),
                                         Constants.CircleSide.Port,
                                         Constants.CircleOrigin.Start,
                                         Constants.TurnDirection.Counterclockwise);

                Point expected = Point.Unknown;
                Point actual = m_Calculator.IntersectionPointForTurnCircle(two);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IntersectionPointForTurnCircleZeroTest()
            {
                var expected = new Point(219.84,
                                         -7.5);
                Point actual = m_Calculator.IntersectionPointForTurnCircle(m_Zero);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void LeftIntersectionPointTest()
            {
                var expected = new Point(219.84,
                                         37.5);
                Point actual = m_Calculator.LeftIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void LeftTurnCircleTest()
            {
                Assert.AreEqual(m_One,
                                m_Calculator.LeftTurnCircle);
            }

            [Test]
            public void RadiansForLeftTurnCircleTest()
            {
                Assert.AreEqual(Angle.FromRadians(5.4351232281981057),
                                m_Calculator.AngleForLeftTurnCircle);
            }

            [Test]
            public void RadiansForRightTurnCircleTest()
            {
                Assert.AreEqual(Angle.FromRadians(0.84806207898148089),
                                m_Calculator.AngleForRightTurnCircle);
            }

            [Test]
            public void RightIntersectionPointTest()
            {
                var expected = new Point(219.84,
                                         -7.5);
                Point actual = m_Calculator.RightIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void RightTurnCircleTest()
            {
                Assert.AreEqual(m_Zero,
                                m_Calculator.RightTurnCircle);
            }
        }

        #endregion

        #region Nested type: AngleToCentrePointCalculatorGeneralTests

        [TestFixture]
        internal sealed class AngleToCentrePointCalculatorGeneralTests
        {
            [SetUp]
            public void Setup()
            {
                m_Settings = Substitute.For <ISettings>();

                m_Zero = new TurnCircle(new Circle(-3.0,
                                                   0.0,
                                                   2.5),
                                        Constants.CircleSide.Port,
                                        Constants.CircleOrigin.Start,
                                        Constants.TurnDirection.Counterclockwise);

                m_One = new TurnCircle(new Circle(3.0,
                                                  0.0,
                                                  2.5),
                                       Constants.CircleSide.Port,
                                       Constants.CircleOrigin.Finish,
                                       Constants.TurnDirection.Counterclockwise);

                var pair = new TurnCirclePair(m_Settings,
                                              m_Zero,
                                              m_One);

                m_Calculator = new AngleToCentrePointCalculator(pair);
            }

            private readonly Angle m_FixedAngle = Angle.FromRadians(0.9272952180016123d);
            private AngleToCentrePointCalculator m_Calculator;
            private TurnCircle m_One;
            private ISettings m_Settings;
            private TurnCircle m_Zero;

            private void AssertCalculateRadiansRelativeToXAxisLeftTurnCircle([NotNull] Angle angle,
                                                                             [NotNull] Angle expected)
            {
                const double radius = 5.0;

                var circle = new Circle(0.0,
                                        0.0,
                                        3.0);
                Point centrePoint = circle.PointOnCircle(angle);
                Point point = circle.PointOnCircle(angle + Angle.For180Degrees);

                Angle actual = m_Calculator.CalculateRadiansRelativeToXAxisForLeftTurnCircle(centrePoint,
                                                                                             point,
                                                                                             radius);

                Assert.AreEqual(expected,
                                actual);
            }

            private void AssertCalculateRadiansRelativeToXAxisRightTurnCircle([NotNull] Angle angle,
                                                                              [NotNull] Angle expected)
            {
                const double radius = 5.0;

                var circle = new Circle(0.0,
                                        0.0,
                                        3.0);
                Point centrePoint = circle.PointOnCircle(angle);
                Point point = circle.PointOnCircle(angle + Angle.For180Degrees);

                Angle actual = m_Calculator.CalculateRadiansRelativeToXAxisForRightTurnCircle(centrePoint,
                                                                                              point,
                                                                                              radius);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void CalculateAngleRelativeToCentreLineTest()
            {
                var zeroCentrePoint = new Point(-3.0,
                                                0.0);
                var oneCentrePoint = new Point(3.0,
                                               0.0);
                const double radius = 5.0;

                Angle actual = m_Calculator.CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                               oneCentrePoint,
                                                                               radius);

                Assert.AreEqual(m_FixedAngle,
                                actual);
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor135DegreesLeftTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For135Degrees,
                                                                    Angle.FromRadians(0.14189705460416402));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor135DegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.For135Degrees,
                                                                     Angle.FromRadians(4.5704919257805257));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor180DegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.For180Degrees,
                                                                     Angle.FromRadians(5.3558900891779739));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor225DegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.For225Degrees,
                                                                     Angle.FromRadians(6.1412882525754222));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor270DegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.For270Degrees,
                                                                     Angle.FromRadians(0.64350110879328426));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor315DegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.For315Degrees,
                                                                     Angle.FromRadians(1.4288992721907321));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor45DegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.For45Degrees,
                                                                     Angle.FromRadians(2.9996955989856291));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisFor90DegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.For90Degrees,
                                                                     Angle.FromRadians(3.7850937623830774));
            }

            [Test]
            public void CalculateAngleRelativeToXAxisForZeroDegreesRightTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisRightTurnCircle(Angle.ForZeroDegrees,
                                                                     Angle.FromRadians(2.2142974355881808));
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor180DegreesCaseOneLeftTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For180Degrees,
                                                                    m_FixedAngle);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor180DegreesCaseTwoLeftTurnCircleTest()
            {
                Angle expected = m_FixedAngle + Angle.For180Degrees;
                const double radius = 5.0;

                Angle zero = Angle.For180Degrees;
                Angle one = zero + Angle.For180Degrees;

                var circle = new Circle(0.0,
                                        0.0,
                                        3.0);
                Point zeroCentrePoint = circle.PointOnCircle(zero);
                Point oneCentrePoint = circle.PointOnCircle(one);

                Angle actual = m_Calculator.CalculateRadiansRelativeToXAxisForLeftTurnCircle(oneCentrePoint,
                                                                                             zeroCentrePoint,
                                                                                             radius);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor225DegreesLeftTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For225Degrees,
                                                                    Angle.FromRadians(1.7126933813990606));
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor270DegreesLeftTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For270Degrees,
                                                                    Angle.FromRadians(2.4980915447965089));
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor315DegreesLeftTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For315Degrees,
                                                                    Angle.FromRadians(3.2834897081939567));
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor360DegreesCaseOneLeftTurnCircleTest()
            {
                Angle expected = m_FixedAngle + Angle.For180Degrees;

                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For360Degrees,
                                                                    expected);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor360DegreesCaseTwoLeftTurnCircleTest()
            {
                const double radius = 5.0;

                Angle zero = Angle.For360Degrees;
                Angle one = zero + Angle.For180Degrees;

                var circle = new Circle(0.0,
                                        0.0,
                                        3.0);
                Point zeroCentrePoint = circle.PointOnCircle(zero);
                Point oneCentrePoint = circle.PointOnCircle(one);

                Angle actual = m_Calculator.CalculateRadiansRelativeToXAxisForLeftTurnCircle(oneCentrePoint,
                                                                                             zeroCentrePoint,
                                                                                             radius);

                Assert.AreEqual(m_FixedAngle,
                                actual);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor45DegreesLeftTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For45Degrees,
                                                                    Angle.FromRadians(4.8542860349888537));
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisFor90DegreesLeftTurnCircleTest()
            {
                AssertCalculateRadiansRelativeToXAxisLeftTurnCircle(Angle.For90Degrees,
                                                                    Angle.FromRadians(5.639684198386302));
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisForRightTurnCircleRetrurns135DegreesTest()
            {
                var zeroCentrePoint = new Point(200.0,
                                                0.0);
                var oneCentrePoint = new Point(200.0,
                                               -60.0);

                Angle expected = Angle.FromRadians(0.84106867056793033d);
                Angle actual = m_Calculator.CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                               oneCentrePoint,
                                                                               45.0);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisForRightTurnCircleRetrurns180DegreesTest()
            {
                var zeroCentrePoint = new Point(200.0,
                                                0.0);
                var oneCentrePoint = new Point(200.0,
                                               -60.0);

                Angle expected = Angle.FromRadians(0.84106867056793033d);
                Angle actual = m_Calculator.CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                               oneCentrePoint,
                                                                               45.0);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisForRightTurnCircleRetrurns45DegreesTest()
            {
                var zeroCentrePoint = new Point(200.0,
                                                00.0);
                var oneCentrePoint = new Point(260.0,
                                               60.0);

                Angle expected = Angle.FromRadians(0.339836909454122d);
                Angle actual = m_Calculator.CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                               oneCentrePoint,
                                                                               45.0);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisForRightTurnCircleRetrurns90DegreesTest()
            {
                var zeroCentrePoint = new Point(200.0,
                                                -30.0);
                var oneCentrePoint = new Point(200.0,
                                               30.0);

                Angle expected = Angle.FromRadians(0.84106867056793033d);
                Angle actual = m_Calculator.CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                               oneCentrePoint,
                                                                               45.0);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisForRightTurnCircleRetrurnsZeroDegreesTest()
            {
                var zeroCentrePoint = new Point(200.0,
                                                0.0);
                var oneCentrePoint = new Point(200.0,
                                               60.0);

                Angle expected = Angle.FromRadians(0.84106867056793033d);
                Angle actual = m_Calculator.CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                               oneCentrePoint,
                                                                               45.0);

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void CalculateRadiansRelativeToXAxisForZeroDegreesTest()
            {
                const double radius = 5.0;

                Angle radiansZero = Angle.ForZeroDegrees;
                Angle radiansOne = radiansZero + Angle.For180Degrees;

                var circle = new Circle(0.0,
                                        0.0,
                                        3.0);
                Point zeroCentrePoint = circle.PointOnCircle(radiansZero);
                Point oneCentrePoint = circle.PointOnCircle(radiansOne);

                Angle actual = m_Calculator.CalculateRadiansRelativeToXAxisForLeftTurnCircle(oneCentrePoint,
                                                                                             zeroCentrePoint,
                                                                                             radius);

                Assert.AreEqual(m_FixedAngle,
                                actual);
            }

            [Test]
            public void DetermineLeftTurnCircleReturnsOneForOneIsLeftTest()
            {
                var zero = Substitute.For <ITurnCircle>();
                zero.Side.Returns(Constants.CircleSide.Port);
                zero.Origin.Returns(Constants.CircleOrigin.Finish);

                var one = Substitute.For <ITurnCircle>();

                var pair = Substitute.For <ITurnCirclePair>();
                pair.Zero.Returns(zero);
                pair.One.Returns(one);

                Assert.AreEqual(one,
                                m_Calculator.DetermineLeftTurnCircle(pair));
            }

            [Test]
            public void DetermineLeftTurnCircleReturnsOneForOneIsRightTest()
            {
                var zero = Substitute.For <ITurnCircle>();
                zero.Side.Returns(Constants.CircleSide.Port);
                zero.Origin.Returns(Constants.CircleOrigin.Finish);

                var one = Substitute.For <ITurnCircle>();

                var pair = Substitute.For <ITurnCirclePair>();
                pair.Zero.Returns(zero);
                pair.One.Returns(one);

                Assert.AreEqual(one,
                                m_Calculator.DetermineLeftTurnCircle(pair));
            }

            [Test]
            public void DetermineLeftTurnCircleReturnsOneForZeroIsLeftAndStartTest()
            {
                var zero = Substitute.For <ITurnCircle>();
                zero.Side.Returns(Constants.CircleSide.Starboard);
                zero.Origin.Returns(Constants.CircleOrigin.Start);

                var one = Substitute.For <ITurnCircle>();

                var pair = Substitute.For <ITurnCirclePair>();
                pair.Zero.Returns(zero);
                pair.One.Returns(one);

                Assert.AreEqual(one,
                                m_Calculator.DetermineLeftTurnCircle(pair));
            }

            [Test]
            public void DetermineLeftTurnCircleReturnsZeroForZeroIsLeftTest()
            {
                var zero = Substitute.For <ITurnCircle>();
                zero.Side.Returns(Constants.CircleSide.Port);
                zero.Origin.Returns(Constants.CircleOrigin.Start);

                var pair = Substitute.For <ITurnCirclePair>();
                pair.Zero.Returns(zero);

                Assert.AreEqual(zero,
                                m_Calculator.DetermineLeftTurnCircle(pair));
            }

            [Test]
            public void DetermineRightTurnCircleReturnsZeroForZeroIsRightTest()
            {
                var zero = Substitute.For <ITurnCircle>();
                zero.Side.Returns(Constants.CircleSide.Port);
                zero.Origin.Returns(Constants.CircleOrigin.Start);

                var pair = Substitute.For <ITurnCirclePair>();
                pair.Zero.Returns(zero);

                Assert.AreEqual(zero,
                                m_Calculator.DetermineLeftTurnCircle(pair));
            }

            [Test]
            public void IsValidReturnsFalseForDifferentSidesTest()
            {
                var pair = Substitute.For <ITurnCirclePair>();
                ITurnCircle zero = pair.Zero;
                ITurnCircle one = pair.One;
                zero.Side.Returns(Constants.CircleSide.Port);
                one.Side.Returns(Constants.CircleSide.Starboard);

                Assert.False(m_Calculator.IsValid(pair));
            }

            [Test]
            public void IsValidReturnsTrueForSameSidesTest()
            {
                var pair = Substitute.For <ITurnCirclePair>();
                ITurnCircle zero = pair.Zero;
                ITurnCircle one = pair.One;
                zero.Side.Returns(Constants.CircleSide.Port);
                one.Side.Returns(Constants.CircleSide.Port);

                Assert.True(m_Calculator.IsValid(pair));
            }

            [Test]
            public void RadiansForLeftTurnCircleTest()
            {
                Angle actual = m_Calculator.AngleForLeftTurnCircle;

                Assert.AreEqual(m_FixedAngle,
                                actual);
            }

            [Test]
            public void RadiansForRightTurnCircleTest()
            {
                Angle expected = Angle.FromRadians(2.2142974355881808);
                Angle actual = m_Calculator.AngleForRightTurnCircle;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void ThrowsForInvalidTurnCirclsTest()
            {
                m_Settings.StartAzimuth.Returns(Angle.For90Degrees);

                var zero = new TurnCircle(new Circle(3.0,
                                                     0.0,
                                                     2.5),
                                          Constants.CircleSide.Port,
                                          Constants.CircleOrigin.Finish,
                                          Constants.TurnDirection.Counterclockwise);

                var one = new TurnCircle(new Circle(3.0,
                                                    0.0,
                                                    2.5),
                                         Constants.CircleSide.Port,
                                         Constants.CircleOrigin.Finish,
                                         Constants.TurnDirection.Counterclockwise);

                var pair = new TurnCirclePair(m_Settings,
                                              zero,
                                              one);

                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws <ArgumentException>(() => new AngleToCentrePointCalculator(pair));
            }
        }

        #endregion
    }
}