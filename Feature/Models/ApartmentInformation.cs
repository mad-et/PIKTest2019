using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Models
{
    /// <summary>
    /// Класс-оболочка для хранения информации о квартире
    /// </summary>
    public class ApartmentInformation
    {
        /// <summary>
        /// Id уровня
        /// </summary>
        public ElementId LevelId { get; set; }

        /// <summary>
        /// Значение параметра BS_Блок
        /// </summary>
        public string BsBlock { get; set; }

        /// <summary>
        /// Значение параметра ROM_Зона
        /// </summary>
        public string RomZone { get; set; }

        /// <summary>
        /// Значение параметра ROM_Подзона
        /// </summary>
        public string RomSubZone { get; set; }

        /// <summary>
        /// Список Id комнат, входящих в состав квартиры
        /// </summary>
        public List<ElementId> RoomIds { get; set; }

        /// <summary>
        /// Логическое значение, определяющее, нужно ли заполнять у комнат параметр ROM_Подзона_Index
        /// </summary>
        public bool Tone { get; set; }
    }
}
