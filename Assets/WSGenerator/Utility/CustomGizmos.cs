using UnityEngine;

namespace SkyCrush.Utility
{
#if UNITY_EDITOR
    public static class CustomGizmos
    {
        public static void DrawRect(Vector2 center, Vector2 size, Color color)
        {
            var point1 = center + (new Vector2(-1, -1) * size / 2);
            var point2 = center + (new Vector2(-1, 1) * size / 2);
            var point3 = center + (new Vector2(1, 1) * size / 2);
            var point4 = center + (new Vector2(1, -1) * size / 2);

            Gizmos.color = color;
            Gizmos.DrawLine(point1, point2);
            Gizmos.DrawLine(point2, point3);
            Gizmos.DrawLine(point3, point4);
            Gizmos.DrawLine(point4, point1);
        }
    }
#endif
}
