using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Converter;
using Selkie.Racetrack.Turn;
using Selkie.Windsor;

namespace Selkie.Racetrack.Tests.Converter.NUnit
{
    internal sealed class TurnCirclePairsToPathsConverterTests
    {
        [TestFixture]
        [ExcludeFromCodeCoverage]
        // ReSharper disable MethodTooLong
        internal sealed class TurnCirclePairsToPathsConverterPortAndStarboardRadiusAreTheSameTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_StartPoint = new Point(12.5,
                                         14.0);
                m_FinishPoint = new Point(8.5,
                                          2.5);

                m_Settings = new Settings(m_StartPoint,
                                          Angle.FromDegrees(0.0),
                                          m_FinishPoint,
                                          Angle.FromDegrees(180.0),
                                          new Distance(100.0),
                                          new Distance(100.0),
                                          true,
                                          true);

                m_TurnCircleZeroOne = new TurnCircle(new Circle(11.0,
                                                                2.5,
                                                                2.5),
                                                     Constants.CircleSide.Starboard,
                                                     Constants.CircleOrigin.Start,
                                                     Constants.TurnDirection.Counterclockwise);

                m_TurnCircleOneOne = new TurnCircle(new Circle(15.0,
                                                               14.0,
                                                               2.5),
                                                    Constants.CircleSide.Port,
                                                    Constants.CircleOrigin.Finish,
                                                    Constants.TurnDirection.Clockwise);

                m_PairOne = new TurnCirclePair(m_Settings,
                                               m_TurnCircleZeroOne,
                                               m_TurnCircleOneOne);

                m_TurnCircleZeroTwo = new TurnCircle(new Circle(6.0,
                                                                2.5,
                                                                2.5),
                                                     Constants.CircleSide.Starboard,
                                                     Constants.CircleOrigin.Start,
                                                     Constants.TurnDirection.Clockwise);

                m_TurnCircleOneTwo = new TurnCircle(new Circle(10.0,
                                                               14.0,
                                                               2.5),
                                                    Constants.CircleSide.Port,
                                                    Constants.CircleOrigin.Finish,
                                                    Constants.TurnDirection.Counterclockwise);

                m_PairTwo = new TurnCirclePair(m_Settings,
                                               m_TurnCircleZeroTwo,
                                               m_TurnCircleOneTwo);

                m_PossibleTurnCirclePairs = Substitute.For <IPossibleTurnCirclePairs>();
                m_PossibleTurnCirclePairs.Pairs.Returns(new List <ITurnCirclePair>
                                                        {
                                                            m_PairOne,
                                                            m_PairTwo
                                                        });

                m_PathValidator = new PathValidator(m_Logger);
                m_TurnCirclePairToPathConverter = new TurnCirclePairToPathConverter(m_Logger,
                                                                                    m_PathValidator);

                m_Converter = new TurnCirclePairsToPathsConverter(m_TurnCirclePairToPathConverter,
                                                                  m_PossibleTurnCirclePairs)
                              {
                                  Settings = m_Settings
                              };

                m_Converter.Convert();
            }

            private TurnCirclePairsToPathsConverter m_Converter;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private TurnCirclePair m_PairOne;
            private TurnCirclePair m_PairTwo;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private TurnCircle m_TurnCircleOneOne;
            private TurnCircle m_TurnCircleOneTwo;
            private TurnCirclePairToPathConverter m_TurnCirclePairToPathConverter;
            private TurnCircle m_TurnCircleZeroOne;
            private TurnCircle m_TurnCircleZeroTwo;

            [Test]
            public void PathsForEmptyListTest()
            {
                var pairs = Substitute.For <IPossibleTurnCirclePairs>();
                pairs.Pairs.Returns(new List <ITurnCirclePair>());

                var pathConverter = Substitute.For <ITurnCirclePairToPathConverter>();

                var converter = new TurnCirclePairsToPathsConverter(pathConverter,
                                                                    pairs)
                                {
                                    Settings = m_Settings
                                };
                m_Converter.Convert();

                Assert.AreEqual(0,
                                converter.Paths.Count());
            }

            [Test]
            public void PathsTest()
            {
                Assert.AreEqual(2,
                                m_Converter.Paths.Count());
            }

            [Test]
            public void SettingsDefaultTest()
            {
                var pathConverter = Substitute.For <ITurnCirclePairToPathConverter>();

                var converter = new TurnCirclePairsToPathsConverter(pathConverter,
                                                                    m_PossibleTurnCirclePairs);

                Assert.True(converter.Settings.IsUnknown);
            }

            [Test]
            public void SettingsRoundtripTest()
            {
                var pathConverter = Substitute.For <ITurnCirclePairToPathConverter>();

                var converter = new TurnCirclePairsToPathsConverter(pathConverter,
                                                                    m_PossibleTurnCirclePairs);
                var settings = Substitute.For <ISettings>();

                converter.Settings = settings;

                Assert.AreEqual(settings,
                                converter.Settings);
            }
        }

        [TestFixture]
        [ExcludeFromCodeCoverage]
        // ReSharper disable MethodTooLong
        internal sealed class TurnCirclePairsToPathsConverterForPortAndStarboardWithDifferentRadiusTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_StartPoint = new Point(12.5,
                                         14.0);
                m_FinishPoint = new Point(8.5,
                                          2.5);

                m_RadiusForPortTurn = new Distance(2.5);
                m_RadiusForStarboardTurn = new Distance(5.0); // todo (BIG) Distance class should go away

                m_Settings = new Settings(m_StartPoint,
                                          Angle.FromDegrees(0.0),
                                          m_FinishPoint,
                                          Angle.FromDegrees(180.0),
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_TurnCircleOneOne = new TurnCircle(new Circle(17.5,
                                                               14.0,
                                                               m_RadiusForStarboardTurn.Length),
                                                    Constants.CircleSide.Starboard,
                                                    Constants.CircleOrigin.Start,
                                                    Constants.TurnDirection.Clockwise);

                m_TurnCircleZeroOne = new TurnCircle(new Circle(11.0,
                                                                2.5,
                                                                m_RadiusForPortTurn.Length),
                                                     Constants.CircleSide.Port,
                                                     Constants.CircleOrigin.Finish,
                                                     Constants.TurnDirection.Counterclockwise);

                m_PairOne = new TurnCirclePair(m_Settings,
                                               m_TurnCircleZeroOne,
                                               m_TurnCircleOneOne);

                m_TurnCircleZeroTwo = new TurnCircle(new Circle(10.0,
                                                                14.0,
                                                                m_RadiusForPortTurn.Length),
                                                     Constants.CircleSide.Port,
                                                     Constants.CircleOrigin.Start,
                                                     Constants.TurnDirection.Counterclockwise);

                m_TurnCircleOneTwo = new TurnCircle(new Circle(3.5,
                                                               2.5,
                                                               m_RadiusForStarboardTurn.Length),
                                                    Constants.CircleSide.Starboard,
                                                    Constants.CircleOrigin.Finish,
                                                    Constants.TurnDirection.Clockwise);

                m_PairTwo = new TurnCirclePair(m_Settings,
                                               m_TurnCircleZeroTwo,
                                               m_TurnCircleOneTwo);

                m_PossibleTurnCirclePairs = Substitute.For <IPossibleTurnCirclePairs>();
                m_PossibleTurnCirclePairs.Pairs.Returns(new List <ITurnCirclePair>
                                                        {
                                                            m_PairOne,
                                                            m_PairTwo
                                                        });

                m_PathValidator = new PathValidator(m_Logger);
                m_TurnCirclePairToPathConverter = new TurnCirclePairToPathConverter(m_Logger,
                                                                                    m_PathValidator);

                m_Converter = new TurnCirclePairsToPathsConverter(m_TurnCirclePairToPathConverter,
                                                                  m_PossibleTurnCirclePairs)
                              {
                                  Settings = m_Settings
                              };

                m_Converter.Convert();
            }

            private TurnCirclePairsToPathsConverter m_Converter;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private TurnCirclePair m_PairOne;
            private TurnCirclePair m_PairTwo;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private TurnCircle m_TurnCircleOneOne;
            private TurnCircle m_TurnCircleOneTwo;
            private TurnCirclePairToPathConverter m_TurnCirclePairToPathConverter;
            private TurnCircle m_TurnCircleZeroOne;
            private TurnCircle m_TurnCircleZeroTwo;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathsForEmptyListTest()
            {
                var pairs = Substitute.For <IPossibleTurnCirclePairs>();
                pairs.Pairs.Returns(new List <ITurnCirclePair>());

                var pathConverter = Substitute.For <ITurnCirclePairToPathConverter>();

                var converter = new TurnCirclePairsToPathsConverter(pathConverter,
                                                                    pairs)
                                {
                                    Settings = m_Settings
                                };
                m_Converter.Convert();

                Assert.AreEqual(0,
                                converter.Paths.Count());
            }

            [Test]
            public void PathsTest()
            {
                Assert.AreEqual(2,
                                m_Converter.Paths.Count());
            }

            [Test]
            public void SettingsDefaultTest()
            {
                var pathConverter = Substitute.For <ITurnCirclePairToPathConverter>();

                var converter = new TurnCirclePairsToPathsConverter(pathConverter,
                                                                    m_PossibleTurnCirclePairs);

                Assert.True(converter.Settings.IsUnknown);
            }

            [Test]
            public void SettingsRoundtripTest()
            {
                var pathConverter = Substitute.For <ITurnCirclePairToPathConverter>();

                var converter = new TurnCirclePairsToPathsConverter(pathConverter,
                                                                    m_PossibleTurnCirclePairs);
                var settings = Substitute.For <ISettings>();

                converter.Settings = settings;

                Assert.AreEqual(settings,
                                converter.Settings);
            }
        }
    }
}