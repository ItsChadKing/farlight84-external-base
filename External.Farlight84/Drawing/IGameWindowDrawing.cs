using GameOverlay.Drawing;

namespace External.Farlight84.Drawing
{
    internal interface IGameWindowDrawing
    {
        void DrawText(float x, float y, string text);
        void DrawLine(float x, float y, float endX, float endY, float stroke);
        void DrawBox(Rectangle rectangle, float stroke);
        void DrawProgessBar(Rectangle rectangle, float stroke, float percentage);
        void BeginScene();
        void EndScene();
        void Initialize();
    }
}
