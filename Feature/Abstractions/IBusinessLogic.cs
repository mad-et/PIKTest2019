using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Feature.Abstractions
{
    public interface IBusinessLogic
    {
        Task<Result> DoIt();
    }
}
