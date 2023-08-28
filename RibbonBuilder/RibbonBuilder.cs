using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RibbonBuilder
{
    /// <summary>
    /// Класс приложения. Реализует интерфейс IExternalApplication
    /// </summary>
    public class RibbonBuilder : IExternalApplication
    {
        /// <summary>
        /// Метод выполняется при завершении Revit
        /// </summary>
        /// <param name="application">Объект, позволяющий взаимодействовать с Revit</param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Метод выполняется при загрузке Revit. 
        /// В методе создается панель инструментов с именем BIMToolKit. 
        /// На панели размещается кнопка "Сделать красиво"
        /// </summary>
        /// <param name="application">Объект, позволяющий взаимодействовать с Revit</param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string imagePath = Path.GetDirectoryName(assemblyLocation) + @"\icons\";
            string tabName = "BIMToolKit";
            application.CreateRibbonTab(tabName);

            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Инструменты");

            Type t = typeof(Feature.Cmd);
            string assemblyLocationCmd = Assembly.GetAssembly(t).Location;
            PushButtonData pushButtonData =
               new PushButtonData("MakeSomething", "Сделать красиво", assemblyLocationCmd, typeof(Feature.Cmd).FullName)
               {
                   LargeImage = new BitmapImage(new Uri(imagePath + "icon1.png"))
               };
            ribbonPanel.AddItem(pushButtonData);

            return Result.Succeeded;
        }
    }
}
