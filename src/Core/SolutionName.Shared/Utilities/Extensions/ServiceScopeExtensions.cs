using Microsoft.Extensions.DependencyInjection;

namespace SolutionName.Application.Shared.Utilities.Extensions;

public static class ServiceScopeExtensions
{
    public static async Task ExecuteScopedAsync<T>(
        this IServiceScopeFactory serviceScopeFactory,
        Func<T, CancellationToken, Task> action,
        CancellationToken cancellationToken = default
    )
        where T : notnull
    {
        using var scope = serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        var task = action(service, cancellationToken);
        await task;
    }


    /*How to use this extension method*/

    /* private ITodoRepository _repository;
     private IServiceScopeFactory _serviceScopeFactory;
 
     public async Task MarkMyTodosDone()
     {
         var todos = await _repository.GetMyTodos();
         var taskList = new List<Task>(todos.Count);
 
         foreach(var todo in todos)
         {
             taskList.Add(DoSomething(todo));
         }
 
         await Task.WhenAll(taskList);
     }
 
     private async Task DoSomething(Todo todo)
         => await _serviceScopeFactory.Do<ITodoRepository>(
             async(ITodoRepository newInstance) => await newInstance.MarkTodoDone(todo.Id);
         );*/
}