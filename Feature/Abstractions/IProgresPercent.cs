using System;

namespace Feature.Abstractions
{
    /// <summary>
    /// Интерфейс, который определеяет структура класса для отслеживания прогресса операции
    /// </summary>
    public interface IProgressPercent
    {
        /// <summary>
        /// Событие, которое будет вызываться при изменении програсса
        /// </summary>
        event Action<double> PercentChanged;

        /// <summary>
        /// Метод, устанавливающий новое состояние прогресса
        /// </summary>
        /// <param name="percent">Число - новое состояние прогресса</param>
        void SetProgress(double percent);
    }
}
