using External.Farlight84.Drawing;
using External.Farlight84.Game.Internal.Enums;
using External.Farlight84.Services;
using System.Numerics;
using External.Farlight84.Game.Internal.Models;
using External.Farlight84.Game.Models;
using GameOverlay.Drawing;

namespace External.Farlight84.Features
{
    internal class Esp
    {
        private readonly IGameWindowDrawing _gameWindowDrawing;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task? _drawTask;

        public Esp(IGameWindowDrawing gameWindowDrawing)
        {
            _gameWindowDrawing = gameWindowDrawing;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _drawTask = Task.Run(DrawEsp);
        }

        public async Task Stop()
        {
            if (_drawTask == null)
                return;

            _cancellationTokenSource.Cancel();
            await _drawTask;

            _cancellationTokenSource.Dispose();
            _drawTask.Dispose();
        }

        private async Task DrawEsp()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                _gameWindowDrawing.BeginScene();

                foreach (var player in EspRetrievalService.Players.Values)
                {
                    DrawPlayerBox(player);
                    foreach (var (boneFrom, boneTo) in UnrealBoneConnections.BoneConnections)
                    {
                        DrawBone(boneFrom, boneTo, player.Bones);
                    }
                }

                _gameWindowDrawing.EndScene();

                await Task.Delay(5);
            }
        }

        private void DrawBone(PlayerBone startBone, PlayerBone endBone, IReadOnlyDictionary<PlayerBone, Vector3> bones)
        {
            var start = bones.GetValueOrDefault(startBone);
            var end = bones.GetValueOrDefault(endBone);

            _gameWindowDrawing.DrawLine(start.X, start.Y, end.X, end.Y, 1);
        }

        private void DrawPlayerBox(Player player)
        {
            var head = player.Bones.GetValueOrDefault(PlayerBone.Head);
            var root = player.Bones.GetValueOrDefault(PlayerBone.Root);

            var boxHeight = Math.Abs(head.Y - root.Y);
            var boxWidth = boxHeight / 2;

            var rectangle = Rectangle.Create(head.X - boxWidth / 2, head.Y, boxWidth, boxHeight);

            _gameWindowDrawing.DrawBox(rectangle, 2);

            var hpBarRectangle = Rectangle.Create(head.X - 10 - boxWidth / 2, head.Y, 4, boxHeight);
            var hpPercentage = (player.Health / player.MaxHealth) * 100;

            _gameWindowDrawing.DrawProgessBar(hpBarRectangle, 1, hpPercentage);

            _gameWindowDrawing.DrawText(root.X, root.Y, player.Name);
            _gameWindowDrawing.DrawText(root.X, root.Y-10, $"Shield: {player.Shield}");
            // Add player distance etc.. do w/e
        }
    }
}