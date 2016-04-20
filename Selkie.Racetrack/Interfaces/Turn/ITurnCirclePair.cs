using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Interfaces.Turn
{
    public interface ITurnCirclePair
    {
        [NotNull]
        IEnumerable <ILine> OuterTangents { get; }

        [NotNull]
        IEnumerable <ILine> InnerTangents { get; }

        int NumberOfTangents { get; }

        [NotNull]
        IEnumerable <ILine> Tangents { get; }

        [NotNull]
        ICirclePair CirclePair { get; }

        [NotNull]
        ITurnCircle Zero { get; }

        [NotNull]
        ITurnCircle One { get; }

        bool IsValid { get; }

        [NotNull]
        IEnumerable <ILine> ValidTangents { get; }

        bool IsUnknown { get; }
    }
}