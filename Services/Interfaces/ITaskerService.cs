using BlazorWasmComponentDemo.Models;

namespace BlazorWasmComponentDemo.Services.Interfaces
{
    public interface ITaskerService
    {
        Task<List<TaskerItem>> GetTaskerItemsAsync();

        Task SaveTaskerItemsAsync(List<TaskerItem> taskList);

        Task AddTaskerItem(TaskerItem item);

        Task RemoveTaskerItem(Guid id);
    }
}
