using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Converters;
using Selkie.Racetrack.Interfaces.Turn;
using Selkie.Windsor;

namespace Selkie.Racetrack.Converters
{
    public class TurnCirclePairToPathConverter : ITurnCirclePairToPathConverter
    {
        private readonly ISelkieLogger m_Logger;
        private readonly IPathValidator m_PathValidator;
        private IEnumerable <IPath> m_Paths = new IPath[0];
        private IEnumerable <IPath> m_PossiblePaths = new IPath[0];
        private ISettings m_Settings = Racetrack.Settings.Unknown;
        private ITurnCirclePair m_TurnCirclePair = Turn.TurnCirclePair.Unknown;

        public TurnCirclePairToPathConverter([NotNull] ISelkieLogger logger,
                                             [NotNull] IPathValidator pathValidator)
        {
            m_Logger = logger;
            m_PathValidator = pathValidator;
        }

        public void Convert()
        {
            m_PossiblePaths = CreatePossiblePaths(m_Settings,
                                                  m_TurnCirclePair);

            m_Paths = ValidatePossiblePaths(m_PossiblePaths);
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

            if ( path.StartPoint != settings.StartPoint )
            {
                IPath reversed = path.Reverse();

                return reversed;
            }

            return path;
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