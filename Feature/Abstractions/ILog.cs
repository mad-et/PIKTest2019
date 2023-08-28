using System;

namespace Feature.Abstractions
{
    /// <summary>
    /// Интерфейс, который определеяет структура класса логирования
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Событие, которое будет вызываться при поступлении нового сообщения
        /// </summary>
        event Action<string> LogChanged;
        /// <summary>
        /// Метод для вывода информационных сообщений
        /// </summary>
        /// <param name="info">Текст сообщения</param>
        void Information(string info);
        /// <summary>
        /// Метод для вывода сообщений об ошибке
        /// </summary>
        /// <param name="error">Текст ошибки</param>
        void Error(string error);
    }
}
