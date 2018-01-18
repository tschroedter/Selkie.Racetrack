using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Calculators;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Turn;
using TurnCircle = Core2.Selkie.Racetrack.Turn.TurnCircle;

namespace Core2.Selkie.Racetrack.Tests.Turn
{
    
    [ExcludeFromCodeCoverage]
    internal sealed class TurnCirclePairTests
    {
        #region Nested type: TurnCirclePairCaseHalfCircleTests

        [TestFixture]
        internal sealed class TurnCirclePairCaseHalfCircleTests
        {
            [SetUp]
            public void Setup()
            {
                m_Settings = Substitute.For <ISettings>();
                m_Settings.StartAzimuth.Returns(Angle.For270Degrees);

                m_One = new TurnCircle(new Circle(16.0,
                                                  2.5,
                                                  6.0),
                                       Constants.CircleSide.Port,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Counterclockwise);

                m_Two = new TurnCircle(new Circle(16.0,
                                                  2.5,
                                                  6.0),
                                       Constants.CircleSide.Port,
                                       Constants.CircleOrigin.Finish,
                                       Constants.TurnDirection.Counterclockwise);

                m_Pair = new TurnCirclePair(m_Settings,
                                            m_One,
                                            m_Two);
            }

            private ITurnCircle m_One;
            private TurnCirclePair m_Pair;
            private ISettings m_Settings;
            private ITurnCircle m_Two;

            [Test]
            public void CirclePairOneTest()
            {
                ICircle actual = m_Pair.CirclePair.Zero;

                Assert.AreEqual(m_Two.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Two.Radius.Length,
                                actual.Radius,
                                "Radius");
            }

            [Test]
            public void CirclePairTwoTest()
            {
                ICircle actual = m_Pair.CirclePair.One;

                Assert.AreEqual(m_One.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_One.Radius.Length,
                                actual.Radius,
                                "Radius");
            }

            [Test]
            public void InnerTangentsCountTest()
            {
                ILine[] actual = m_Pair.InnerTangents.ToArray();

                Assert.AreEqual(0,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void IsUnknownDefaultTest()
            {
                Assert.False(m_Pair.IsUnknown);
            }

            [Test]
            public void IsValidForTrueTest()
            {
                Assert.True(m_Pair.IsValid);
            }

            [Test]
            public void NumberOfTangentsTest()
            {
                Assert.AreEqual(1,
                                m_Pair.NumberOfTangents);
            }

            [Test]
            public void OneTest()
            {
                Assert.AreEqual(m_One,
                                m_Pair.Zero);
            }

            [Test]
            public void OuterTangentsCountTest()
            {
                ILine[] actual = m_Pair.OuterTangents.ToArray();

                Assert.AreEqual(0,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void TangentsCountTest()
            {
                ILine[] actual = m_Pair.Tangents.ToArray();

                Assert.AreEqual(0,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void ValidTangentsCountTest()
            {
                ILine[] actual = m_Pair.ValidTangents.ToArray();

                Assert.AreEqual(1,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void ValidTangentsFirstTest()
            {
                ILine[] tangents = m_Pair.ValidTangents.ToArray();
                ILine actual = tangents [ 0 ];

                Assert.AreEqual(new Point(16.0,
                                          -3.5),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(16.0,
                                          -3.5),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void ZeroTest()
            {
                Assert.AreEqual(m_Two,
                                m_Pair.One);
            }
        }

        #endregion

        #region Nested type: TurnCirclePairCaseOneTests

        [TestFixture]
        internal sealed class TurnCirclePairCaseOneTests
        {
            [SetUp]
            public void Setup()
            {
                m_Settings = Substitute.For <ISettings>();

                m_One = new TurnCircle(new Circle(1.0,
                                                  2.0,
                                                  6.0),
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Counterclockwise);

                m_Two = new TurnCircle(new Circle(4.0,
                                                  5.0,
                                                  3.0),
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Finish,
                                       Constants.TurnDirection.Counterclockwise);

                m_LineInner = Substitute.For <ILine>();
                m_LineInner.Length.Returns(200.0);

                m_Pair = new TurnCirclePair(m_Settings,
                                            m_One,
                                            m_Two);
            }

            private ILine m_LineInner;
            private ITurnCircle m_One;
            private TurnCirclePair m_Pair;
            private ISettings m_Settings;
            private ITurnCircle m_Two;

            [Test]
            public void CirclePairOneTest()
            {
                ICircle actual = m_Pair.CirclePair.Zero;

                Assert.AreEqual(m_One.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_One.Radius.Length,
                                actual.Radius,
                                "Radius");
            }

            [Test]
            public void CirclePairTwoTest()
            {
                ICircle actual = m_Pair.CirclePair.One;

                Assert.AreEqual(m_Two.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Two.Radius.Length,
                                actual.Radius,
                                "Radius");
            }

            [Test]
            public void CreateDummyTangentsForSameCirclesCountTest()
            {
                m_Settings.StartAzimuth.Returns(Angle.For90Degrees);

                var circle = new Circle(new Point(4.0,
                                                  3.0),
                                        3.0);

                var turnCircle = Substitute.For <ITurnCircle>();
                turnCircle.Circle.Returns(circle);

                IEnumerable <ILine> tangents = m_Pair.CreateDummyTangentsForSameCircles(m_Settings,
                                                                                        turnCircle);

                Assert.AreEqual(1,
                                tangents.Count(),
                                "Count");
            }

            [Test]
            public void CreateDummyTangentsForSameCirclesLineTest()
            {
                m_Settings.StartAzimuth.Returns(Angle.For90Degrees);

                var circle = new Circle(new Point(4.0,
                                                  3.0),
                                        3.0);

                var turnCircle = Substitute.For <ITurnCircle>();
                turnCircle.Circle.Returns(circle);

                IEnumerable <ILine> tangents = m_Pair.CreateDummyTangentsForSameCircles(m_Settings,
                                                                                        turnCircle);

                ILine actual = tangents.First();

                Assert.AreEqual(new Point(4.0,
                                          6.0),
                                actual.StartPoint);
                Assert.AreEqual(new Point(4.0,
                                          6.0),
                                actual.EndPoint);
            }

            [Test]
            public void DetermineIsValidForForDistanceLessRadiusTest()
            {
                var oneCircle = Substitute.For <ICircle>();
                
                oneCircle.Distance(Arg.Any <ICircle>()).Returns(5.0);

                var one = Substitute.For <ITurnCircle>();
                one.Radius.Returns(new Distance(6.0));
                one.Circle.Returns(oneCircle);

                var two = Substitute.For <ITurnCircle>();
                two.Radius.Returns(new Distance(6.0));

                Assert.False(m_Pair.DetermineIsValid(one,
                                                     two));
            }

            [Test]
            public void DetermineIsValidForTrueCaseEqualsTest()
            {
                var circle = new Circle(new Point(-10.0,
                                                  -10.0),
                                        5.0);

                var one = Substitute.For <ITurnCircle>();
                one.Circle.Returns(circle);

                var two = Substitute.For <ITurnCircle>();
                two.Circle.Returns(circle);

                Assert.True(m_Pair.DetermineIsValid(one,
                                                    two));
            }

            [Test]
            public void InnerTangentsTest()
            {
                Assert.AreEqual(0,
                                m_Pair.InnerTangents.Count());
            }

            [Test]
            public void IsUnknownDefaultTest()
            {
                Assert.False(m_Pair.IsUnknown);
            }

            [Test]
            public void IsValidForTrueTest()
            {
                Assert.False(m_Pair.IsValid);
            }

            [Test]
            public void NumberOfTangentsTest()
            {
                Assert.AreEqual(2,
                                m_Pair.NumberOfTangents);
            }

            [Test]
            public void OneTest()
            {
                Assert.AreEqual(m_Two,
                                m_Pair.One);
            }

            [Test]
            public void OuterTangentsCountTest()
            {
                Assert.AreEqual(2,
                                m_Pair.OuterTangents.Count());
            }

            [Test]
            public void OuterTangentsFirstTest()
            {
                var expected = new Line(1,
                                        7.0,
                                        2.0,
                                        7.0,
                                        5.0);

                Assert.AreEqual(expected,
                                m_Pair.OuterTangents.First());
            }

            [Test]
            public void OuterTangentsLastTest()
            {
                var expected = new Line(1,
                                        1.0,
                                        8.0,
                                        4.0,
                                        8.0);

                Assert.AreEqual(expected,
                                m_Pair.OuterTangents.Last());
            }

            [Test]
            public void TangentsCountTest()
            {
                Assert.AreEqual(2,
                                m_Pair.Tangents.Count());
            }

            [Test]
            public void TangentsFirstTest()
            {
                var expected = new Line(1,
                                        7.0,
                                        2.0,
                                        7.0,
                                        5.0);

                Assert.AreEqual(expected,
                                m_Pair.Tangents.First());
            }

            [Test]
            public void TangentsLastTest()
            {
                var expected = new Line(1,
                                        1.0,
                                        8.0,
                                        4.0,
                                        8.0);

                Assert.AreEqual(expected,
                                m_Pair.Tangents.Last());
            }

            [Test]
            public void ValidTangentsCountTest()
            {
                ILine[] actual = m_Pair.ValidTangents.ToArray();

                Assert.AreEqual(2,
                                actual.Length);
            }

            [Test]
            public void ValidTangentsFirstTest()
            {
                var expected = new Line(1,
                                        7.0,
                                        2.0,
                                        7.0,
                                        5.0);

                ILine actual = m_Pair.ValidTangents.First();

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void ValidTangentsLastTest()
            {
                var expected = new Line(1,
                                        1.0,
                                        8.0,
                                        4.0,
                                        8.0);

                ILine actual = m_Pair.ValidTangents.Last();

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void ZeroTest()
            {
                Assert.AreEqual(m_One,
                                m_Pair.Zero);
            }
        }

        #endregion

        #region Nested type: TurnCirclePairCaseTwoTests

        [TestFixture]
        internal sealed class TurnCirclePairCaseTwoTests
        {
            [SetUp]
            public void Setup()
            {
                m_Settings = Substitute.For <ISettings>();

                m_One = new TurnCircle(new Circle(16.0,
                                                  2.5,
                                                  2.5),
                                       Constants.CircleSide.Port,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Counterclockwise);

                m_Two = new TurnCircle(new Circle(10.0,
                                                  2.5,
                                                  2.5),
                                       Constants.CircleSide.Port,
                                       Constants.CircleOrigin.Finish,
                                       Constants.TurnDirection.Counterclockwise);

                m_Pair = new TurnCirclePair(m_Settings,
                                            m_One,
                                            m_Two);
            }

            private ITurnCircle m_One;
            private TurnCirclePair m_Pair;
            private ISettings m_Settings;
            private ITurnCircle m_Two;

            [Test]
            public void CirclePairOneTest()
            {
                ICircle actual = m_Pair.CirclePair.Zero;

                Assert.AreEqual(m_Two.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Two.Radius.Length,
                                actual.Radius,
                                "Radius");
            }

            [Test]
            public void CirclePairTwoTest()
            {
                ICircle actual = m_Pair.CirclePair.One;

                Assert.AreEqual(m_One.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_One.Radius.Length,
                                actual.Radius,
                                "Radius");
            }

            [Test]
            public void InnerTangentsCountTest()
            {
                ILine[] actual = m_Pair.InnerTangents.ToArray();

                Assert.AreEqual(2,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void InnerTangentsFirstTest()
            {
                ILine[] tangents = m_Pair.InnerTangents.ToArray();
                ILine actual = tangents [ 0 ];

                Assert.AreEqual(new Point(12.08,
                                          3.88),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(13.92,
                                          1.12),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void InnerTangentsSecondTest()
            {
                ILine[] tangents = m_Pair.InnerTangents.ToArray();
                ILine actual = tangents [ 1 ];

                Assert.AreEqual(new Point(12.08,
                                          1.12),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(13.92,
                                          3.88),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void IsUnknownDefaultTest()
            {
                Assert.False(m_Pair.IsUnknown);
            }

            [Test]
            public void IsValidForTrueTest()
            {
                Assert.True(m_Pair.IsValid);
            }

            [Test]
            public void NumberOfTangentsTest()
            {
                Assert.AreEqual(4,
                                m_Pair.NumberOfTangents);
            }

            [Test]
            public void OneTest()
            {
                Assert.AreEqual(m_One,
                                m_Pair.One);
            }

            [Test]
            public void OuterTangentsCountTest()
            {
                ILine[] actual = m_Pair.OuterTangents.ToArray();

                Assert.AreEqual(2,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void OuterTangentsFirstTest()
            {
                ILine[] tangents = m_Pair.OuterTangents.ToArray();
                ILine actual = tangents [ 0 ];

                Assert.AreEqual(new Point(10.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(16.0,
                                          0.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void OuterTangentsSecondTest()
            {
                ILine[] tangents = m_Pair.OuterTangents.ToArray();
                ILine actual = tangents [ 1 ];

                Assert.AreEqual(new Point(10.0,
                                          5.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(16.0,
                                          5.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void SelectTangentsTest()
            {
                var tinyLine = Substitute.For <ILine>();
                tinyLine.Length.Returns(1.0);

                var normalLine = Substitute.For <ILine>();
                normalLine.Length.Returns(100.0);

                var calculator = Substitute.For <ICirclePairTangentLinesCalculator>();
                calculator.OuterTangents.Returns(new List <ILine>
                                                 {
                                                     tinyLine
                                                 });
                calculator.InnerTangents.Returns(new List <ILine>
                                                 {
                                                     normalLine
                                                 });

                IEnumerable <ILine> selectTangents = m_Pair.SelectTangents(calculator);
                ILine[] actual = selectTangents.ToArray();

                Assert.AreEqual(2,
                                actual.Length,
                                "Length");
                Assert.AreEqual(tinyLine,
                                actual.First(),
                                "First");
                Assert.AreEqual(normalLine,
                                actual.Last(),
                                "First");
            }

            [Test]
            public void TangentsCountTest()
            {
                ILine[] actual = m_Pair.Tangents.ToArray();

                Assert.AreEqual(4,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void ValidTangentsCountTest()
            {
                ILine[] actual = m_Pair.ValidTangents.ToArray();

                Assert.AreEqual(4,
                                actual.Length,
                                "Length");
            }

            [Test]
            public void ValidTangentsFirstTest()
            {
                ILine[] tangents = m_Pair.ValidTangents.ToArray();
                ILine actual = tangents [ 0 ];

                Assert.AreEqual(new Point(10.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(16.0,
                                          0.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void ValidTangentsFourthTest()
            {
                ILine[] tangents = m_Pair.ValidTangents.ToArray();
                ILine actual = tangents [ 3 ];

                Assert.AreEqual(new Point(12.08,
                                          1.12),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(13.92,
                                          3.88),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void ValidTangentsSecondTest()
            {
                ILine[] tangents = m_Pair.ValidTangents.ToArray();
                ILine actual = tangents [ 1 ];

                Assert.AreEqual(new Point(10.0,
                                          5.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(16.0,
                                          5.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void ValidTangentsThirdTest()
            {
                ILine[] tangents = m_Pair.ValidTangents.ToArray();
                ILine actual = tangents [ 2 ];

                Assert.AreEqual(new Point(12.08,
                                          3.88),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(13.92,
                                          1.12),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void ZeroTest()
            {
                Assert.AreEqual(m_Two,
                                m_Pair.Zero);
            }
        }

        #endregion

        [TestFixture]
        internal sealed class TurnCirclePairUnknownTests
        {
            [SetUp]
            public void Setup()
            {
                m_Pair = TurnCirclePair.Unknown;
            }

            private ITurnCirclePair m_Pair;

            [Test]
            public void CirclePairDefaultTest()
            {
                Assert.True(m_Pair.CirclePair.IsUnknown);
            }

            [Test]
            public void IsUnknownDefaultTest()
            {
                Assert.True(m_Pair.IsUnknown);
            }

            [Test]
            public void OneDefaultTest()
            {
                Assert.True(m_Pair.One.IsUnknown);
            }

            [Test]
            public void ValidTangentsDefaultTest()
            {
                Assert.AreEqual(0,
                                m_Pair.ValidTangents.Count());
            }

            [Test]
            public void ZeroDefaultTest()
            {
                Assert.True(m_Pair.Zero.IsUnknown);
            }
        }
    }
}