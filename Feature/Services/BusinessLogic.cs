using Autodesk.Revit.DB;
using CSharpFunctionalExtensions;
using Feature.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Services
{
    internal class BusinessLogic : IBusinessLogic
    {
        private readonly RevitTask _revitTask;
        private readonly ILog _log;
        private readonly IProgressPercent _progressPercent;      
        private readonly Document _doc;      


        public BusinessLogic(RevitTask revitTask, ILog log, IProgressPercent progressPercent, Document doc)
        {
            _revitTask = revitTask;
            _log = log;
            _progressPercent = progressPercent;           
            _doc = doc;           
        }

        public async Task<Result> DoIt()
        {
            _progressPercent.SetProgress(0);

            _log.Information("Начало работы плагина");

            var setParamResult = await SetParam();
            if (setParamResult.IsFailure)
                return Result.Failure(setParamResult.Error);

            _log.Information($"Окончание работы плагина");

            _progressPercent.SetProgress(100);

            return Result.Success();
        }

        private async Task<Result> SetParam()
        {
            var walls = new FilteredElementCollector(_doc)
                .OfClass(typeof(Wall))
                .OfType<Wall>()
                .ToList();
            Result setParamResult = await _revitTask.Run(app =>
            {
                Transaction transactionSetParam = new Transaction(_doc);
                try
                {
                    transactionSetParam.Start($"Задание значений параметров");

                    for (int i = 0; i < walls.Count; i++)
                    {
                        walls[i].LookupParameter($"Комментарии")?.Set("Дверь");
                    }

                    transactionSetParam.Commit();
                    return Result.Success();
                }

                catch (Exception ex)
                {
                    transactionSetParam.RollBack();
                    return Result.Failure($"Произошла ошибка при попытке задания параметра/n{ex.Message}");
                }
            });
            return setParamResult;
        }
    }
}
