using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Feature.Abstractions
{
    /// <summary>
    /// Интерфейс, который определяет структуру класса, описывающего бизнес-логику проекта
    /// </summary>
    public interface IBusinessLogic
    {
        /// <summary>
        /// Метод - "точка входа" для расчетов 
        /// </summary>
        /// <returns>Возвращает результат выполнения операции в виде объекта Result</returns>
        Task<Result> DoIt();
    }
}
