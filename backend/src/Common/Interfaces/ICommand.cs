using System.Threading.Tasks;

namespace StoreBackend.Common.Interfaces
{
    public interface ICommand
    {
        Task Execute();
    }

    public interface ICommandWithReturn<TReturn>
    {
        Task<TReturn> Execute();
    }

    public interface ICommandWithParam<TParameter>
    {
        Task Execute(TParameter parameter);
    }

    public interface ICommandWithReturnAndParam<TReturn, TParameter>
    {
        Task<TReturn> Execute(TParameter parameter);
    }
}