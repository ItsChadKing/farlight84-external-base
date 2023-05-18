using External.Farlight84.Game;
using External.Farlight84.Game.Models;
using External.Farlight84.Memory;
using Microsoft.Extensions.Hosting;

namespace External.Farlight84.Services
{
    internal class WorldRetrievalService : BackgroundService
    {
        public static long WorldPointer;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(2));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                DoWork();
            }
        }

        private void DoWork()
        {
            var possibleNewWorldPointer = MemoryService.Read<long>(MemoryService.BaseAddress + Offsets.GWorld);

            if (possibleNewWorldPointer == WorldPointer)
                return;

            WorldPointer = possibleNewWorldPointer;
            var gameInstance = MemoryService.Read<long>(WorldPointer + 0x220);
            var localPlayers = MemoryService.Read<long>(gameInstance + 0x38);
            LocalPlayer.GetInstance.LocalPlayerPointer = MemoryService.Read<long>(localPlayers);
        }
    }
}
