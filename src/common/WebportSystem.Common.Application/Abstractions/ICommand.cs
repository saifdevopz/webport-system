namespace WebportSystem.Common.Application.Abstractions;

public interface ICommand : ICommand<Result>;

public interface ICommand<TResponse> : IBaseCommand;

public interface IBaseCommand;