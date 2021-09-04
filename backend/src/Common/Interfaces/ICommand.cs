using System.Threading.Tasks;

namespace StoreBackend.Common.Interfaces
{
    public interface ICommandAsync
    {
        Task Execute();
    }

    public interface ICommandWithReturnAsync<TReturn>
    {
        Task<TReturn> Execute();
    }

    public interface ICommandWithParamAsync<TParameter>
    {
        Task Execute(TParameter parameter);
    }

    public interface ICommandWithReturnAndParam<TReturn, TParameter>
    {
        TReturn Execute(TParameter parameter);
    }

    public interface ICommandWithReturnAndParamAsync<TReturn, TParameter>
    {
        Task<TReturn> Execute(TParameter parameter);
    }
}