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
    [Transaction(TransactionMode.Manual)]
    public class Cmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //LoadAssembly();
            Config config = new Config(commandData);
            var service = config.Build();
            var mainWindow = service.GetRequiredService<MainWindow>();
            mainWindow.DataContext = service.GetRequiredService<MainWindowViewModel>();
            mainWindow?.Show();
            return Result.Succeeded;
        }
        private void LoadAssembly()
        {
            //string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Autodesk\Revit\Addins\2022\CreateSheets\";
            // string path = System.Reflection.Assembly.GetAssembly(typeof(Cmd)).Location;
            //Assembly.LoadFrom($@"{path}Microsoft.Xaml.Behaviors.dll");
            //Assembly.LoadFrom($@"{path}System.Data.SQLite.dll");

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(assemblyLocation) ;
            Assembly.LoadFrom($@"{path}\CSharpFunctionalExtensions.dll");
            Assembly.LoadFrom($@"{path}\Microsoft.Extensions.DependencyInjection.dll");
            Assembly.LoadFrom($@"{path}\Microsoft.Extensions.DependencyInjection.Abstractions.dll");
            
        }
    }
}
