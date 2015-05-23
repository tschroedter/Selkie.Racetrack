using System;
using JetBrains.Annotations;
using Selkie.Common;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;
using SelkieRacetrack = Selkie.Racetrack;

namespace Selkie.Racetrack.Calculators
{
    [ProjectComponent(Lifestyle.Transient)]
    public sealed class LinePairToRacetrackCalculator
        : ILinePairToRacetrackCalculator,
          IDisposable
    {
        private readonly IPathCalculator m_Calculator;
        private readonly IDisposer m_Disposer;
        private readonly ICalculatorFactory m_Factory;
        private ILine m_FromLine = Line.Unknown;
        private bool m_IsPortTurnAllowed = true;
        private bool m_IsStarboardTurnAllowed = true;
        private Distance m_Radius = Distance.Unknown;
        private ISettings m_Settings = SelkieRacetrack.Settings.Unknown;
        private ILine m_ToLine = Line.Unknown;

        public LinePairToRacetrackCalculator([NotNull] IDisposer disposer,
                                             [NotNull] ICalculatorFactory factory)
        {
            m_Disposer = disposer;
            m_Factory = factory;

            m_Calculator = factory.Create <IPathCalculator>();

            m_Disposer.AddResource(ReleaseCalculator);
        }

        public void Dispose()
        {
            m_Disposer.Dispose();
        }

        public bool IsPortTurnAllowed
        {
            get
            {
                return m_IsPortTurnAllowed;
            }
            set
            {
                m_IsPortTurnAllowed = value;
            }
        }

        public bool IsStarboardTurnAllowed
        {
            get
            {
                return m_IsStarboardTurnAllowed;
            }
            set
            {
                m_IsStarboardTurnAllowed = value;
            }
        }

        public Distance Radius
        {
            get
            {
                return m_Radius;
            }
            set
            {
                m_Radius = value;
            }
        }

        public ILine FromLine
        {
            get
            {
                return m_FromLine;
            }
            set
            {
                m_FromLine = value;
            }
        }

        public ILine ToLine
        {
            get
            {
                return m_ToLine;
            }
            set
            {
                m_ToLine = value;
            }
        }

        public ISettings Settings
        {
            get
            {
                return m_Settings;
            }
        }

        public IPath Racetrack
        {
            get
            {
                return m_Calculator.Path;
            }
        }

        public void Calculate()
        {
            m_Settings = new Settings(m_FromLine.EndPoint,
                                      m_FromLine.AngleToXAxis,
                                      m_ToLine.StartPoint,
                                      m_ToLine.AngleToXAxis,
                                      m_Radius,
                                      m_IsPortTurnAllowed,
                                      m_IsStarboardTurnAllowed);

            m_Calculator.Settings = m_Settings;
            m_Calculator.Calculate();
        }

        internal void ReleaseCalculator()
        {
            m_Factory.Release(m_Calculator);
        }
    }
}