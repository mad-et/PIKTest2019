using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Feature.ViewModels;
using Feature.Views;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace Feature
{
    /// <summary>
    /// Класс команды. Реализует интерфейс IExternalCommand
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class Cmd : IExternalCommand
    {
        /// <summary>
        /// Метод - "точка входа" плагина
        /// </summary>
        /// <param name="commandData">Объект, позволяющий взаимодействовать с Revit</param>
        /// <param name="message">Сообщение, которое будет отображаться при неуспешном завершении</param>
        /// <param name="elements">Набор элементов, которые "подсветит" Revit</param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            LoadAssembly();
            Config config = new Config(commandData);
            var service = config.Build();
            var mainWindow = service.GetRequiredService<MainWindow>();
            mainWindow.DataContext = service.GetRequiredService<MainWindowViewModel>();
            mainWindow?.Show();
            return Result.Succeeded;
        }

        /// <summary>
        /// Метод добавляет в сборку некоторые зависимости, которые Revit не может найти сам
        /// </summary>
        private void LoadAssembly()
        {
            //string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Autodesk\Revit\Addins\2022\CreateSheets\";           

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(assemblyLocation);
            Assembly.LoadFrom($@"{path}\CSharpFunctionalExtensions.dll");
            Assembly.LoadFrom($@"{path}\Microsoft.Extensions.DependencyInjection.dll");
            Assembly.LoadFrom($@"{path}\Microsoft.Extensions.DependencyInjection.Abstractions.dll");
            Assembly.LoadFrom($@"{path}\FontAwesome.WPF.dll");
        }
    }
}
