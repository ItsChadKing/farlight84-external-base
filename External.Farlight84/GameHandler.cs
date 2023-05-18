using External.Farlight84.Drawing;
using External.Farlight84.Events;
using External.Farlight84.Features;
using External.Farlight84.Memory;

namespace External.Farlight84
{
    internal class GameHandler
    {
        private readonly IGameEvent _gameEvent;
        private readonly IGameWindowDrawing _gameWindowDrawing;

        public GameHandler(IGameEvent gameEvent, IGameWindowDrawing gameWindowDrawing)
        {
            _gameEvent = gameEvent;
            _gameWindowDrawing = gameWindowDrawing;
        }

        public async Task Initialize()
        {
            _gameEvent.ProcessName = "SolarlandClient-Win64-Shipping.exe";
            _gameEvent.OnGameActive += OnGameActive;
            _gameEvent.OnGameIsRunning += OnGameIsRunning;
            _gameEvent.OnGameInactive += OnGameInactive;

            Console.WriteLine($"Waiting on {_gameEvent.ProcessName}...");

            await _gameEvent.Subscribe();
        }

        private Task Start(int processId)
        {
            MemoryService.Initialize(processId);
            _gameWindowDrawing.Initialize();

            var espTask = new Esp(_gameWindowDrawing);
            espTask.Start();

            return Task.CompletedTask;
        }

        private async void OnGameActive(object? sender, int processId)
        {
            await Start(processId);
        }

        private async void OnGameIsRunning(object? sender, int processId)
        {
            await Start(processId);
        }

        private void OnGameInactive(object? sender, int processId)
        {
            Environment.Exit(0);
        }
    }
}
