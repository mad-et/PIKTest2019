using System;

namespace Feature.Abstractions
{
    public interface IProgressPercent
    {
        event Action<double> PercentChanged;
        void SetProgress(double percent);
    }
}
