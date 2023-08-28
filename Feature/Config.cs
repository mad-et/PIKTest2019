using Autodesk.Revit.UI;
using Feature.Abstractions;
using Feature.Services;
using Feature.ViewModels;
using Feature.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Feature
{
    internal class Config
    {
        public ExternalCommandData _commandData { get; set; }
        public Config(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }
        public IServiceProvider Build()
        {
            var services = new ServiceCollection();
            ServiceConfig(services);

            return services.BuildServiceProvider();
        }

        private void ServiceConfig(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<ILog, Log>();
            services.AddSingleton<IProgressPercent, ProgressPercent>();

            services.AddSingleton<RevitTask>();
            services.AddTransient(n => _commandData.Application.ActiveUIDocument.Document);
            services.AddTransient<IBusinessLogic, BusinessLogic>();
            //services.AddTransient<RevitModelUtils>();
        }
    }
}
