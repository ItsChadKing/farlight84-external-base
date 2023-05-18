using System.Collections.Concurrent;
using System.Numerics;
using External.Farlight84.Drawing;
using External.Farlight84.Game;
using External.Farlight84.Game.Internal.Models;
using External.Farlight84.Game.Models;
using External.Farlight84.Memory;
using Microsoft.Extensions.Hosting;

namespace External.Farlight84.Services
{
    internal class EspRetrievalService : BackgroundService
    {
        public static ConcurrentDictionary<long, Player> Players = new();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(5));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                DoWork();
            }
        }

        private void DoWork()
        {
            UpdateLocalPlayer();
            UpdatePlayers();
        }

        private static void UpdatePlayers()
        {
            var currentPlayers = new List<long>();

            var persistencePtr = MemoryService.Read<long>(WorldRetrievalService.WorldPointer + 0x30);

            var actorArrayPtr = MemoryService.Read<long>(persistencePtr + 0x98);
            var actorArrayCount = MemoryService.Read<int>(persistencePtr + 0xA0);

            for (var i = 0; i < actorArrayCount; i++)
            {
                var aActor = MemoryService.Read<long>(actorArrayPtr + i * 0x8);

                if (aActor == 0)
                    continue;

                if (aActor == LocalPlayer.GetInstance.PlayerPawnPointer)
                {
                    continue;
                }

                var someActorId = MemoryService.Read<int>(aActor + 0x18);
                var name = GameUtilities.GetNameFromFName(Offsets.GName, someActorId);

                if (!name.Contains("BP_Character_"))
                {
                    continue;
                }

                var aActorRootComponent = MemoryService.Read<long>(aActor + Offsets.Actor.RootComponent);

                if (aActorRootComponent == 0)
                    continue;

                var actorRelativeLocation = MemoryService.Read<FVector>(aActorRootComponent + Offsets.Actor.RootComponentStartLocation).ToVector3();

                if (actorRelativeLocation == Vector3.Zero)
                    continue;

                var distance = LocalPlayer.GetInstance.CameraCacheEntry.Pov.Location.DistTo(actorRelativeLocation);
                var distanceInMeters = GameUtilities.ToMeters(distance);

                if (distanceInMeters > 350)
                {
                    continue;
                }

                var aActorMesh = MemoryService.Read<long>(aActor + Offsets.Actor.Mesh);

                if (aActorMesh == 0)
                    continue;

                var playerState = MemoryService.Read<long>(aActor + 0x248);

                if (Players.TryGetValue(aActor, out var existingPlayer))
                {
                    existingPlayer.Location = Renderer.WorldToScreenX(actorRelativeLocation, LocalPlayer.GetInstance.CameraCacheEntry);
                    existingPlayer.Distance = distanceInMeters;
                    existingPlayer.Bones = existingPlayer.GetBones();
                    existingPlayer.Health = existingPlayer.GetHealth();
                    existingPlayer.Shield = existingPlayer.GetShield();
                }
                else
                {
                    var player = new Player
                    {
                        ActorMeshPointer = aActorMesh,
                        ActorStatePointer = playerState,
                        Location = Renderer.WorldToScreenX(actorRelativeLocation, LocalPlayer.GetInstance.CameraCacheEntry),
                        Distance = distanceInMeters
                    };

                    player.Bones = player.GetBones();
                    player.Health = player.GetHealth();
                    player.MaxHealth = player.GetMaxHealth();
                    player.Shield = player.GetShield();
                    player.Name = player.GetPlayerName();

                    Players.TryAdd(aActor, player);
                }

                currentPlayers.Add(aActor);
            }

            RemoveNonExistingPlayers(currentPlayers);
        }

        private static void RemoveNonExistingPlayers(IEnumerable<long> currentPlayers)
        {
            var playersToRemove = Players.Keys.Except(currentPlayers).ToList();

            foreach (var player in playersToRemove)
            {
                Players.TryRemove(player, out _);
            }
        }

        private static void UpdateLocalPlayer()
        {
            if (LocalPlayer.GetInstance.LocalPlayerPointer == 0)
            {
                return;
            }

            var localPlayerControllerPtr = MemoryService.Read<long>(LocalPlayer.GetInstance.LocalPlayerPointer + Offsets.LocalActor.PlayerController);
            LocalPlayer.GetInstance.PlayerPawnPointer = MemoryService.Read<long>(localPlayerControllerPtr + Offsets.LocalActor.AcknowledgedPawn);
            var localPlayerCameraManager = MemoryService.Read<long>(localPlayerControllerPtr + Offsets.LocalActor.PlayerCameraManager);
            LocalPlayer.GetInstance.CameraCacheEntry = MemoryService.Read<FCameraCacheEntry>(localPlayerCameraManager + Offsets.LocalActor.CameraCacheEntry);
        }
    }
}
