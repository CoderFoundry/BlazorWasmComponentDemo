using BlazorWasmComponentDemo.Models;
using BlazorWasmComponentDemo.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Text.Json;


namespace BlazorWasmComponentDemo.Services
{

    /// <summary>
    /// Service for managing tasker items.
    /// </summary>
    public class TaskerService(IJSRuntime jsRuntime) : ITaskerService
    {
       
        /// <summary>
        /// Adds a new tasker item.
        /// </summary>
        /// <param name="item">The tasker item to add.</param>
        public async Task AddTaskerItem(TaskerItem item)
        {
            item.Id = Guid.NewGuid();
            item.IsComplete = false;

            List<TaskerItem> newTaskList = (await GetTaskerItemsAsync()).ToList();
            newTaskList.Add(item);

            await SaveTaskerItemsAsync(newTaskList);
        }

        /// <summary>
        /// Removes a tasker item by its ID.
        /// </summary>
        /// <param name="id">The ID of the tasker item to remove.</param>
        public async Task RemoveTaskerItem(Guid id)
        {
            List<TaskerItem> taskList = (await GetTaskerItemsAsync()).ToList();
            taskList = taskList.Where(t => t.Id != id).ToList();

            await SaveTaskerItemsAsync(taskList);
        }

        /// <summary>
        /// Retrieves all tasker items.
        /// </summary>
        /// <returns>A list of tasker items.</returns>
        public async Task<List<TaskerItem>> GetTaskerItemsAsync()
        {
            try
            {
                string? taskListState = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "TaskerList") ?? "[]";
                List<TaskerItem> taskList = JsonSerializer.Deserialize<List<TaskerItem>>(taskListState)!;
                return taskList;
            }
            catch (Exception)
            {
                List<TaskerItem> taskList = new List<TaskerItem>();
                await SaveTaskerItemsAsync(taskList);
                return taskList;
            }
        }

        /// <summary>
        /// Saves the tasker items.
        /// </summary>
        /// <param name="taskList">The list of tasker items to save.</param>
        public async Task SaveTaskerItemsAsync(List<TaskerItem> taskList)
        {
            string? taskListState = JsonSerializer.Serialize(taskList);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "TaskerList", taskListState);
        }
    }
}
