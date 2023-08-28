using Autodesk.Revit.UI;
using Feature.Abstractions;
using Feature.Services;
using Feature.ViewModels;
using Feature.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Feature
{
    /// <summary>
    /// Класс, конфигурирующий DI-контейнер
    /// </summary>
    internal class Config
    {
        public ExternalCommandData _commandData { get; set; }

        /// <summary>
        /// Конструктор. Получает аргументом объект для взаимодействия с Revit
        /// </summary>
        /// <param name="commandData">Объект для взаимодействия с Revit</param>
        public Config(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }

        /// <summary>
        /// Метод выполняет построение DI-контейнера
        /// </summary>
        /// <returns></returns>
        public IServiceProvider Build()
        {
            var services = new ServiceCollection();
            ServiceConfig(services);
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Метод добавляет заданные объекты в контейнер
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        private void ServiceConfig(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<ILog, Log>();
            services.AddSingleton<IProgressPercent, ProgressPercent>();

            services.AddSingleton<RevitTask>();
            services.AddTransient(n => _commandData.Application.ActiveUIDocument.Document);
            services.AddSingleton<StringAlphanumericComparer>();
            services.AddTransient<IBusinessLogic, BusinessLogic>();
        }
    }
}
