using Feature.Abstractions;
using System;

namespace Feature.Services
{
    public class Log : ILog
    {
        public event Action<string> LogChanged;
        private void NotifyPropertyChanged(string message)
        {
            LogChanged?.Invoke(message);
        }

        public void Information(string info)
        {
            string message = $"{DateTime.Now:T}: [Информация] {info} \n";
            NotifyPropertyChanged(message);
        }

        public void Error(string error)
        {
            string message = $"{DateTime.Now:T}: [Ошибка] {error} \n";
            NotifyPropertyChanged(message);
        }
    }
}
