using System.Diagnostics;

namespace External.Farlight84.Events
{
    public class GameEvent : IGameEvent
    {
        public string ProcessName { get; set; } = default!;
        public event EventHandler<int>? OnGameActive;
        public event EventHandler<int>? OnGameInactive;
        public event EventHandler<int>? OnGameIsRunning;

        public async Task Subscribe()
        {
            var isRunning = IsGameRunning();

            if (!isRunning)
            {
                await OnGameActiveEvent();
            }

            await Task.CompletedTask;
        }

        private bool IsGameRunning()
        {
            var procNameWithoutExe = ProcessName.Replace(".exe", "");
            var pid = Process.GetProcessesByName(procNameWithoutExe).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.MainWindowTitle));

            if (pid == null)
            {
                return false;
            }

            OnGameIsRunning?.Invoke(this, pid.Id);
            return true;

        }

        private async Task OnGameActiveEvent()
        {
            while (true)
            {
                var procNameWithoutExe = ProcessName.Replace(".exe", "");
                var pid = Process.GetProcessesByName(procNameWithoutExe).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.MainWindowTitle));

                if (pid != null)
                {
                    pid.EnableRaisingEvents = true;
                    pid.Exited += OnGameInactiveEvent;

                    OnGameActive?.Invoke(this, pid.Id);
                    return;
                }

                await Task.Delay(1000);
            }
        }

        private void OnGameInactiveEvent(object? sender, EventArgs e)
        {
            var process = (Process?)sender;

            if (process == null)
            {
                return;
            }

            OnGameInactive?.Invoke(this, process.Id);
        }
    }
}
