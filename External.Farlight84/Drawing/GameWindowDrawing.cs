using External.Farlight84.Memory;
using GameOverlay.Drawing;
using GameOverlay.Windows;

namespace External.Farlight84.Drawing
{
    internal class GameWindowDrawing : IGameWindowDrawing
    {
        public static float Height;
        public static float Width;

        private Graphics _graphics = default!;
        private Font _defaultFont = default!;
        private SolidBrush _defaultWhiteBrush = default!;
        private SolidBrush _defaultGreenBrush = default!;
        private SolidBrush _defaultInvisibleBrush = default!;
        
        public void Initialize()
        {
            _graphics = new Graphics(MemoryService.MainWindowHandle)
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true,
                VSync = true,
                UseMultiThreadedFactories = true
            };

            var window = new StickyWindow(MemoryService.MainWindowHandle, _graphics)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = 120
            };

            window.Create();
            _graphics.WindowHandle = window.Handle;
            _graphics.Setup();

            _defaultWhiteBrush = _graphics.CreateSolidBrush(255, 255, 255);
            _defaultInvisibleBrush = _graphics.CreateSolidBrush(0, 0, 0, 0);
            _defaultGreenBrush = _graphics.CreateSolidBrush(0, 255, 0);
            _defaultFont = _graphics.CreateFont("Futura", 12);
        }

        public void DrawText(float x, float y, string text)
        {
            _graphics.DrawText(_defaultFont, _defaultWhiteBrush, x, y, text);
        }

        public void DrawLine(float x, float y, float endX, float endY, float stroke)
        {
            _graphics.DrawLine(_defaultWhiteBrush, x, y, endX, endY, stroke);
        }

        public void DrawBox(Rectangle rectangle, float stroke)
        {
            _graphics.DrawBox2D(_defaultWhiteBrush, _defaultInvisibleBrush, rectangle, stroke);
        }

        public void DrawProgessBar(Rectangle rectangle, float stroke, float percentage)
        {
            _graphics.DrawHorizontalProgressBar(_defaultWhiteBrush, _defaultGreenBrush, rectangle, stroke, percentage);
        }

        public void BeginScene()
        {
            Width = _graphics.Width;
            Height = _graphics.Height;

            _graphics.BeginScene();
            _graphics.ClearScene();
        }

        public void EndScene()
        {
            _graphics.EndScene();
        }
    }
}
