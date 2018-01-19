using System;
using System.Collections.Generic;
using System.Linq;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Converters
{
    [UsedImplicitly]
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

        public void Convert()
        {
            PossiblePaths = CreatePossiblePaths(Settings,
                                                TurnCirclePair);

            Paths = ValidatePossiblePaths(PossiblePaths);
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

            foreach ( ILine tangent in turnCirclePair.ValidTangents )
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

            Constants.TurnDirection defaultTurnDirection = Settings.DefaultTurnDirection;

            IEnumerable <IPath> valid = array.Where(x => m_PathValidator.IsValid(x,
                                                                                 defaultTurnDirection));

            return valid.ToList();
        }

        #region ITurnCirclePairToPathConverter Members

        public ISettings Settings { get; set; } = Racetrack.Settings.Unknown;

        public ITurnCirclePair TurnCirclePair { get; set; } = Turn.TurnCirclePair.Unknown;

        public IEnumerable <IPath> PossiblePaths { get; private set; } = new IPath[0];

        public IEnumerable <IPath> Paths { get; private set; } = new IPath[0];

        #endregion
    }
}