using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Windsor;

namespace Selkie.Racetrack.Calculators
{
    [ProjectComponent(Lifestyle.Transient)]
    public class RacetracksCalculator : IRacetracksCalculator
    {
        public RacetracksCalculator(
            [NotNull] IFeaturesToFeaturesForwardToForwardRacetrackCalculator
                featuresToFeaturesForwardToForwardCalculator,
            [NotNull] IFeaturesToFeaturesForwardToReverseRacetrackCalculator
                featuresToFeaturesForwardToReverseCalculator,
            [NotNull] IFeaturesToFeaturesReverseToForwardRacetrackCalculator
                featuresToFeaturesReverseToForwardCalculator,
            [NotNull] IFeaturesToFeaturesReverseToReverseRacetrackCalculator
                featuresToFeaturesReverseToReverseCalculator)
        {
            ReverseToReverse = new IPath[0][];
            ReverseToForward = new IPath[0][];
            ForwardToReverse = new IPath[0][];
            ForwardToForward = new IPath[0][];
            IsUnknown = false;
            IsStarboardTurnAllowed = true;
            IsPortTurnAllowed = true;
            TurnRadiusForPort = DefaultTurnRadius;
            TurnRadiusForStarboard = DefaultTurnRadius;
            m_FeaturesToFeaturesForwardToForwardCalculator = featuresToFeaturesForwardToForwardCalculator;
            m_FeaturesToFeaturesForwardToReverseCalculator = featuresToFeaturesForwardToReverseCalculator;
            m_FeaturesToFeaturesReverseToForwardCalculator = featuresToFeaturesReverseToForwardCalculator;
            m_FeaturesToFeaturesReverseToReverseCalculator = featuresToFeaturesReverseToReverseCalculator;
        }

        public static readonly Distance DefaultTurnRadius = new Distance(60.0);

        private readonly IFeaturesToFeaturesForwardToForwardRacetrackCalculator
            m_FeaturesToFeaturesForwardToForwardCalculator;

        private readonly IFeaturesToFeaturesForwardToReverseRacetrackCalculator
            m_FeaturesToFeaturesForwardToReverseCalculator;

        private readonly IFeaturesToFeaturesReverseToForwardRacetrackCalculator
            m_FeaturesToFeaturesReverseToForwardCalculator;

        private readonly IFeaturesToFeaturesReverseToReverseRacetrackCalculator
            m_FeaturesToFeaturesReverseToReverseCalculator;

        private IEnumerable <ISurveyFeature> m_Features = new List <ISurveyFeature>();

        public void Calculate()
        {
            ForwardToForward = CalculateGeneral(m_FeaturesToFeaturesForwardToForwardCalculator,
                                                m_Features,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);

            ForwardToReverse = CalculateGeneral(m_FeaturesToFeaturesForwardToReverseCalculator,
                                                m_Features,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);

            ReverseToForward = CalculateGeneral(m_FeaturesToFeaturesReverseToForwardCalculator,
                                                m_Features,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);

            ReverseToReverse = CalculateGeneral(m_FeaturesToFeaturesReverseToReverseCalculator,
                                                m_Features,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);
        }

        public Distance TurnRadiusForStarboard { get; set; }

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public IEnumerable <ISurveyFeature> Features
        {
            get
            {
                return m_Features;
            }
            set
            {
                m_Features = value.ToArray();
            }
        }

        public Distance TurnRadiusForPort { get; set; }

        public bool IsUnknown { get; private set; }

        public IPath[][] ForwardToForward { get; private set; }

        public IPath[][] ForwardToReverse { get; private set; }

        public IPath[][] ReverseToForward { get; private set; }

        public IPath[][] ReverseToReverse { get; private set; }

        [NotNull]
        internal IPath[][] CalculateGeneral([NotNull] IBaseFeaturesToFeaturesRacetracksCalculator calculator,
                                            [NotNull] IEnumerable <ISurveyFeature> features,
                                            [NotNull] Distance turnRadiusForPort,
                                            [NotNull] Distance turnRadiusForStarboard)
        {
            calculator.Features = features;
            calculator.TurnRadiusForPort = turnRadiusForPort;
            calculator.TurnRadiusForStarboard = turnRadiusForStarboard;
            calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            calculator.Calculate();

            return calculator.Paths;
        }
    }
}