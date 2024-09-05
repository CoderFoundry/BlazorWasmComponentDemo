using BlazorWasmComponentDemo.Models;
using BlazorWasmComponentDemo.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Text.Json;


namespace BlazorWasmComponentDemo.Services
{

    public class TaskerService(IJSRuntime jsRuntime) : ITaskerService
    {
        public async Task AddTaskerItem(TaskerItem item)
        {
            item.Id = Guid.NewGuid();
            item.IsComplete = false;

            List<TaskerItem> newTaskList = [.. await GetTaskerItemsAsync(), item];
            await SaveTaskerItemsAsync(newTaskList);
        }
           
        public async Task RemoveTaskerItem(Guid id)
        {
            List<TaskerItem> taskList = await GetTaskerItemsAsync();
            taskList = taskList.Where(t => t.Id != id).ToList();

            await SaveTaskerItemsAsync(taskList);
        }

        public async Task<List<TaskerItem>> GetTaskerItemsAsync()
        {
            try
            {
                string? taskListState = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "TaskerList") ?? "[]";
                List<TaskerItem> taskList = JsonSerializer.Deserialize<List<TaskerItem>>(taskListState)!;
                return taskList;
            }
            catch (Exception? e)
            {
                List<TaskerItem> taskList = new();
                await SaveTaskerItemsAsync(taskList);
                return taskList;
            }
        }

        public async Task SaveTaskerItemsAsync(List<TaskerItem> taskList)
        {
            string? taskListState = JsonSerializer.Serialize(taskList);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "TaskerList", taskListState);
        }
       
    }
}
