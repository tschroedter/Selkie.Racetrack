using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Windsor;
using Core2.Selkie.Windsor.Interfaces;

namespace Core2.Selkie.Racetrack.Converters
{
    public class TurnCirclePairToPathConverter : ITurnCirclePairToPathConverter
    {
        public TurnCirclePairToPathConverter([NotNull] ISelkieLogger logger,
                                             [NotNull] IPathValidator pathValidator)
        {
            m_Logger = logger;
            m_PathValidator = pathValidator;
        }

        private readonly ISelkieLogger m_Logger;
        private readonly IPathValidator m_PathValidator;
        private IEnumerable <IPath> m_Paths = new IPath[0];
        private IEnumerable <IPath> m_PossiblePaths = new IPath[0];
        private ISettings m_Settings = Racetrack.Settings.Unknown;
        private ITurnCirclePair m_TurnCirclePair = Turn.TurnCirclePair.Unknown;

        public void Convert()
        {
            m_PossiblePaths = CreatePossiblePaths(m_Settings,
                                                  m_TurnCirclePair);

            m_Paths = ValidatePossiblePaths(m_PossiblePaths);
        }

        [NotNull]
        internal IEnumerable <IPath> CreatePossiblePaths([NotNull] ISettings settings,
                                                         [NotNull] ITurnCirclePair turnCirclePair)
        {
            var paths = new List <IPath>();

            if ( !turnCirclePair.IsValid )
            {
                return paths;
            }

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach ( ILine tangent in turnCirclePair.ValidTangents )
                // ReSharper restore LoopCanBeConvertedToQuery
            {
                var creator = new TurnCirclePairToPathCreator(m_Logger,
                                                              settings,
                                                              turnCirclePair,
                                                              tangent);

                IPath path = ReversePathIfNeccessary(settings,
                                                     creator.Path);

                paths.Add(path);
            }

            return paths;
        }

        [NotNull]
        internal IPath ReversePathIfNeccessary([NotNull] ISettings settings,
                                               [NotNull] IPath path)
        {
            if ( path.IsUnknown ||
                 path.Distance.Length < 0.0 ||
                 Math.Abs(path.Distance.Length) <= Constants.EpsilonDistance )
            {
                return path;
            }

            if ( path.StartPoint == settings.StartPoint )
            {
                return path;
            }

            IPath reversed = path.Reverse();

            return reversed;
        }

        [NotNull]
        private IEnumerable <IPath> ValidatePossiblePaths([NotNull] IEnumerable <IPath> allPossiblePaths)
        {
            IPath[] array = allPossiblePaths.ToArray();

            Constants.TurnDirection defaultTurnDirection = m_Settings.DefaultTurnDirection;

            IEnumerable <IPath> valid = array.Where(x => m_PathValidator.IsValid(x,
                                                                                 defaultTurnDirection));

            return valid.ToList();
        }

        #region ITurnCirclePairToPathConverter Members

        public ISettings Settings
        {
            get
            {
                return m_Settings;
            }
            set
            {
                m_Settings = value;
            }
        }

        public ITurnCirclePair TurnCirclePair
        {
            get
            {
                return m_TurnCirclePair;
            }
            set
            {
                m_TurnCirclePair = value;
            }
        }

        public IEnumerable <IPath> PossiblePaths
        {
            get
            {
                return m_PossiblePaths;
            }
        }

        public IEnumerable <IPath> Paths
        {
            get
            {
                return m_Paths;
            }
        }

        #endregion
    }
}