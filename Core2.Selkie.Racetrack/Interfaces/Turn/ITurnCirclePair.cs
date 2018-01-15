using System.Collections.Generic;
using Core2.Selkie.Geometry.Shapes;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Turn
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