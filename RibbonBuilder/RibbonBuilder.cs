using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RibbonBuilder
{
    public class RibbonBuilder : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {           
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string imagePath = Path.GetDirectoryName(assemblyLocation) + @"\icons\";
            string tabName = "BIMToolKit";

            application.CreateRibbonTab(tabName);
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Инструменты");

            //PushButtonData pushButtonData1 =
            //    new PushButtonData("Creating", "Сделать красиво", assemblyLocation, typeof(Cmd).FullName)
            //    {
            //        LargeImage = new BitmapImage(new Uri(imagePath + "icon1.png"))       
            //    };
            //ribbonPanel.AddItem(pushButtonData1);

            Type t = typeof(Feature.Cmd);          
            string assemblyLocation2 = Assembly.GetAssembly(t).Location;
            PushButtonData pushButtonData2 =
               new PushButtonData("MakeSomething", "Сделать красиво", assemblyLocation2, typeof(Feature.Cmd).FullName)
               {
                   LargeImage = new BitmapImage(new Uri(imagePath + "icon2.png"))                  
               };
            ribbonPanel.AddItem(pushButtonData2);

            return Result.Succeeded;
        }
    }
}
