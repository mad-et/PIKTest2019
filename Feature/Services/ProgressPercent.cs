using Feature.Abstractions;
using System;

namespace Feature.Services
{
    public class ProgressPercent : IProgressPercent
    {
        public event Action<double> PercentChanged;

        public void SetProgress(double percent)
        {
            PercentChanged?.Invoke(percent);
        }
    }
}
