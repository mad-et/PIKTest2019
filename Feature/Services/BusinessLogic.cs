using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using CSharpFunctionalExtensions;
using Feature.Abstractions;
using Feature.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Feature.Services
{
    ///<inheritdoc/>
    internal class BusinessLogic : IBusinessLogic
    {
        private readonly RevitTask _revitTask;
        private readonly ILog _log;
        private readonly IProgressPercent _progressPercent;
        private readonly Document _doc;
        private readonly StringAlphanumericComparer _comparer;

        /// <summary>
        /// Конструктор получает из контейнера необходимые для работы объекты
        /// </summary>
        /// <param name="revitTask">Объект для синхронизации контекста</param>
        /// <param name="log">Объект для отображения логов в UI</param>
        /// <param name="progressPercent">Объект для отображения прогресса в UI</param>
        /// <param name="doc">Объект для работы с Revit-документом</param>
        /// <param name="comparer">Объект для сортировки строк с числами (типа "Квартира 01") в порядке возрастания чисел</param>
        public BusinessLogic(RevitTask revitTask, ILog log, IProgressPercent progressPercent, Document doc, StringAlphanumericComparer comparer)
        {
            _revitTask = revitTask;
            _log = log;
            _progressPercent = progressPercent;
            _doc = doc;
            _comparer = comparer;
        }

        ///<inheritdoc/>
        public async Task<Result> DoIt()
        {
            _progressPercent.SetProgress(0);

            _log.Information("Начало работы плагина");

            var setParamResult = await SetParams();
            if (setParamResult.IsFailure)
                return Result.Failure(setParamResult.Error);

            _log.Information($"Окончание работы плагина");

            _progressPercent.SetProgress(100);

            return Result.Success();
        }

        /// <summary>
        /// Метод получает список квартир, комнаты в которых необходимо перекрасить, в виде объектов ApartmentInformation. 
        /// Затем устанавливает для комнат значение параметра ROM_Подзона_Index
        /// </summary>
        /// <returns>Возвращает результат выполнения операции в виде объекта Result</returns>
        private async Task<Result> SetParams()
        {
            List<ApartmentInformation> listApartInfo = GetApartmentInformation();
            return await SetToneParams(listApartInfo);
        }

        /// <summary>
        /// Метод получает список квартир, комнаты в которых необходимо перекрасить, в виде объектов ApartmentInformation
        /// </summary>
        /// <returns>Список квартир, комнаты в которых необходимо перекрасить, в виде объектов ApartmentInformation</returns>
        private List<ApartmentInformation> GetApartmentInformation()
        {
            List<ApartmentInformation> result = new List<ApartmentInformation>();

            var allRooms = new FilteredElementCollector(_doc)
                           .OfCategory(BuiltInCategory.OST_Rooms)
                           .OfClass(typeof(SpatialElement))
                           .OfType<Room>()
                           .Where(x => x.LookupParameter("ROM_Зона")?.AsString().Contains("Квартира") ?? false)
                           .ToList();

            foreach (var groupByLevel in allRooms.GroupBy(x => x.get_Parameter(BuiltInParameter.ROOM_LEVEL_ID).AsElementId()))
            {
                foreach (var groupBySection in groupByLevel.GroupBy(x => x.LookupParameter("BS_Блок")?.AsString() ?? String.Empty))
                {
                    if (String.IsNullOrEmpty(groupBySection.Key))
                    {
                        _log.Error($"Не задан параметр BS_Блок у элементов {String.Join(", ", groupBySection.Select(x => x.Id).ToList())}");
                        continue;
                    }
                    List<ApartmentInformation> listApartInfoByLevelSection = new List<ApartmentInformation>();
                    foreach (IGrouping<string, Room> groupBySubZone in groupBySection.GroupBy(x => x.LookupParameter("ROM_Подзона")?.AsString() ?? String.Empty))
                    {
                        if (String.IsNullOrEmpty(groupBySubZone.Key))
                        {
                            _log.Error($"Не задан параметр ROM_Подзона у элементов {String.Join(", ", groupBySubZone.Select(x => x.Id).ToList())}");
                            continue;
                        }
                        foreach (IGrouping<string, Room> groupByZone in groupBySubZone.GroupBy(x => x.LookupParameter("ROM_Зона")?.AsString() ?? String.Empty))
                        {
                            if (String.IsNullOrEmpty(groupByZone.Key))
                            {
                                _log.Error($"Не задан параметр ROM_Зона у элементов {String.Join(", ", groupBySubZone.Select(x => x.Id).ToList())}");
                                continue;
                            }
                            listApartInfoByLevelSection.Add(new ApartmentInformation()
                            {
                                LevelId = groupByLevel.Key,
                                BsBlock = groupBySection.Key,
                                RomZone = groupByZone.Key,
                                RomSubZone = groupBySubZone.Key,
                                RoomIds = groupByZone.Select(x => x.Id).ToList(),
                                Tone = false
                            });
                        }
                    }

                    listApartInfoByLevelSection = listApartInfoByLevelSection.OrderBy(x => x.RomZone, _comparer).ToList();
                    for (int i = 1; i < listApartInfoByLevelSection.Count; i++)
                    {
                        if (listApartInfoByLevelSection[i].RomSubZone == listApartInfoByLevelSection[i - 1].RomSubZone)
                            listApartInfoByLevelSection[i].Tone = !listApartInfoByLevelSection[i - 1].Tone;
                    }

                    result.AddRange(listApartInfoByLevelSection.Where(x => x.Tone == true));
                }
            }
            return result;
        }

        /// <summary>
        /// Затем устанавливает для комнат значение параметра ROM_Подзона_Index
        /// </summary>
        /// <param name="listApartInfo"> список квартир, комнаты в которых необходимо перекрасить</param>
        /// <returns>Возвращает результат выполнения операции в виде объекта Result</returns>
        private async Task<Result> SetToneParams(List<ApartmentInformation> listApartInfo)
        {
            if (listApartInfo.Count() > 0)
            {
                Result setParamsResult = await _revitTask.Run(app =>
                {
                    Transaction transactionSetParam = new Transaction(_doc);
                    try
                    {
                        transactionSetParam.Start($"Задание значений параметра ROM_Подзона_Index");
                        foreach (ApartmentInformation apartment in listApartInfo)
                        {
                            foreach (ElementId id in apartment.RoomIds)
                            {
                                Room room = _doc.GetElement(id) as Room;
                                if (room == null)
                                    continue;
                                string paramValue = room.LookupParameter("ROM_Расчетная_подзона_ID")?.AsString() ?? String.Empty;
                                if (String.IsNullOrEmpty(paramValue))
                                {
                                    _log.Error($"Не задан параметр ROM_Расчетная_подзона_ID у элемента {id}");
                                    continue;
                                }
                                paramValue += ".Полутон";
                                bool setParamResult = room.LookupParameter("ROM_Подзона_Index")?.Set(paramValue) ?? false;
                                if (!setParamResult)
                                {
                                    _log.Error($"Не удалось установить параметр ROM_Подзона_Index у элемента {id}");
                                    continue;
                                }
                            }
                            _log.Information($"Установлены параметры. Этаж {apartment.LevelId}, секция {apartment.BsBlock}, квартира {apartment.RomZone}");
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
                if (setParamsResult.IsFailure)
                {
                    _log.Error(setParamsResult.Error);
                }
            }
            return Result.Success();
        }
    }
}
