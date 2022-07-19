using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class SimpleArea : Area
    {
        public override void Start()
        {
            base.Start();
            RenderOutline();
        }

        private void RenderOutline()
        {
            Vector3[] points = new Vector3[5];
            _outline.positionCount = points.Length;

            points[0] = new Vector3(topPoint.position.x, 0f, topPoint.position.z);
            points[1] = new Vector3(topPoint.position.x, 0f, bottomPoint.position.z);
            points[3] = new Vector3(bottomPoint.position.x, 0f, topPoint.position.z);
            points[2] = new Vector3(bottomPoint.position.x, 0f, bottomPoint.position.z);
            points[4] = points[0];

            for (int i = 0; i < points.Length; i++)
                _outline.SetPosition(i, points[i]);
        }
    }
}