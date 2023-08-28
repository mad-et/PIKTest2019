using Feature.Abstractions;
using System;

namespace Feature.Services
{
    ///<inheritdoc/>
    public class Log : ILog
    {
        ///<inheritdoc/>
        public event Action<string> LogChanged;

        /// <summary>
        /// Метод, который уведомляет подписчиков о событии
        /// </summary>
        /// <param name="message"></param>
        private void NotifyPropertyChanged(string message)
        {
            LogChanged?.Invoke(message);
        }

        ///<inheritdoc/>
        public void Information(string info)
        {
            string message = $"{DateTime.Now:T}: [Информация] {info} \n";
            NotifyPropertyChanged(message);
        }

        ///<inheritdoc/>
        public void Error(string error)
        {
            string message = $"{DateTime.Now:T}: [Ошибка] {error} \n";
            NotifyPropertyChanged(message);
        }
    }
}
