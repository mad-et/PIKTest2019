using Feature.Abstractions;
using System;

namespace Feature.Services
{
    ///<inheritdoc/>
    public class ProgressPercent : IProgressPercent
    {
        ///<inheritdoc/>
        public event Action<double> PercentChanged;

        ///<inheritdoc/>
        public void SetProgress(double percent)
        {
            PercentChanged?.Invoke(percent);
        }
    }
}
