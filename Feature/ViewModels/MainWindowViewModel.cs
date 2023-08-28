using CSharpFunctionalExtensions;
using Feature.Abstractions;
using Prism.Commands;
using Prism.Mvvm;

namespace Feature.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly ILog _log;
        private readonly IProgressPercent _progressPercent;
        private readonly IBusinessLogic _businessLogic;

        /// <summary>
        /// Конструктор получает из контейнера необходимые для работы объекты
        /// </summary>
        /// <param name="log">Объект для отображения логов в UI</param>
        /// <param name="progressPercent">Объект для отображения прогресса в UI</param>
        /// <param name="businessLogic">Объект, содержащий бизнес-логику проекта</param>
        public MainWindowViewModel(
            ILog log,
            IProgressPercent progressPercent,
            IBusinessLogic businessLogic)
        {
            _log = log;
            _log.LogChanged += OnLogChanged;
            _progressPercent = progressPercent;
            _progressPercent.PercentChanged += OnProgressPercent;
            _businessLogic = businessLogic;
        }

        /// <summary>
        /// Состояние ProgressBar
        /// </summary>
        #region ProgressBar
        private double progressBarStatus;
        public double ProgressBarStatus
        {
            get => progressBarStatus;
            set => SetProperty(ref progressBarStatus, value);
        }

        private void OnProgressPercent(double persent)
        {
            ProgressBarStatus = persent;
        }
        #endregion

        /// <summary>
        /// Лог
        /// </summary>
        #region LogInfo
        private string logInfo;
        public string LogInfo
        {
            get => logInfo;
            set => SetProperty(ref logInfo, value);
        }

        private void OnLogChanged(string message)
        {
            LogInfo += message;
        }
        #endregion

        /// <summary>
        /// Команда, выполняющаяся при нажатии на кнопку
        /// </summary>
        public DelegateCommand DoIt
        {
            get
            {
                return new DelegateCommand(
                     async () =>
                     {
                         Result result = await _businessLogic.DoIt();
                     });
            }
        }
    }
}
