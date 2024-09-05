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
