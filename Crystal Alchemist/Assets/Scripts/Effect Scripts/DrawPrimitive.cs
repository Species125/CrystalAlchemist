using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum PrimitiveType
    {
        line,
        triangle,
        rectangle,
        diamond,
        pentagon,
        hexagon,
        octagon,
        circle,
        collider,
        laser
    }

    [RequireComponent(typeof(LineRenderer))]
    public class DrawPrimitive : MonoBehaviour
    {
        #region Attributes 

        [BoxGroup("Settings")]
        [SerializeField]
        private PrimitiveType type = PrimitiveType.circle;
        [BoxGroup("Settings")]

        private bool drawColliderEnabled = false;

        [BoxGroup("Settings")]
        [SerializeField]
        [ShowIf("type", PrimitiveType.collider)]
        private Collider2D collider;

        [BoxGroup("Settings")]
        [HideIf("type", PrimitiveType.collider)]
        [HideIf("type", PrimitiveType.rectangle)]
        [HideIf("type", PrimitiveType.laser)]
        [SerializeField]
        [Range(0, 25)]
        private float radius = 3f;

        [BoxGroup("Settings")]
        [ShowIf("type", PrimitiveType.rectangle)]
        [SerializeField]
        private Vector2 size;

        [BoxGroup("Settings")]
        [ShowIf("type", PrimitiveType.laser)]
        [SerializeField]
        private GameObject sender;

        [BoxGroup("Settings")]
        [ShowIf("type", PrimitiveType.laser)]
        [SerializeField]
        private GameObject target;

        [BoxGroup("Settings")]
        [HideIf("type", PrimitiveType.collider)]
        [HideIf("type", PrimitiveType.laser)]
        [HideIf("type", PrimitiveType.line)]
        [SerializeField]
        private Vector2 offset;

        [BoxGroup("Appearance")]
        [SerializeField]
        [Range(0, 10)]
        private float lineWidth = 0.05f;

        [BoxGroup("Appearance")]
        [SerializeField]
        private Color color = Color.white;

        [BoxGroup("Appearance")]
        [SerializeField]
        private Material material;

        private LineRenderer line = null;

        #endregion


        #region Unity Functions

        private void Start()
        {
            this.line = this.GetComponent<LineRenderer>();
            this.line.useWorldSpace = false;
        }

        private void OnValidate()
        {
            this.line = this.GetComponent<LineRenderer>();
            this.line.useWorldSpace = false;
            Render();
        }

        public void UpdateGUI()
        {
            OnValidate();
        }

        private void FixedUpdate()
        {
            Render();
        }

        public void SetCollider(Collider2D collider)
        {
            this.type = PrimitiveType.collider;
            this.collider = collider;
        }

        public void SetLine(float distance)
        {
            this.type = PrimitiveType.line;
            this.radius = distance;
        }

        public void SetPrimitive(Material material, float lineWidth, Color color)
        {
            this.material = material;
            this.lineWidth = lineWidth;
            this.color = color;

            this.line = this.GetComponent<LineRenderer>();
            this.line.useWorldSpace = false;
            Render();
        }

        private void Render()
        {
            setProperties();

            if (this.type == PrimitiveType.collider) drawColliders();
            else if (this.type == PrimitiveType.rectangle) drawRectangle(this.size, this.offset);
            else if (this.type == PrimitiveType.laser) drawLaser(this.sender, this.target);
            else if (this.type == PrimitiveType.line) drawLine();
            else drawPrimitive();
        }

        #endregion


        #region Set Functions 

        private int setSegments(PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.triangle: return 3;
                case PrimitiveType.diamond: return 4;
                case PrimitiveType.pentagon: return 5;
                case PrimitiveType.hexagon: return 6;
                case PrimitiveType.octagon: return 8;
                case PrimitiveType.circle: return 45;
                default: return 0;
            }
        }

        private void setColor()
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(this.color, 0.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(this.color.a, 0.0f) }
            );
            this.line.colorGradient = gradient;
            this.line.material = this.material;
        }

        private void setProperties()
        {
            setColor();
            this.line.startWidth = this.lineWidth;
            this.line.endWidth = this.lineWidth;

        }

        #endregion


        #region Draw Methods 

        private void drawPrimitive()
        {
            int segments = setSegments(this.type);
            drawPrimitive(this.radius, segments, this.offset);
        }

        private void drawColliders()
        {
            if (this.collider.GetType() == typeof(PolygonCollider2D))
            {
                PolygonCollider2D poly = (PolygonCollider2D)this.collider;
                drawPolygon(poly);
            }
            else if (this.collider.GetType() == typeof(BoxCollider2D))
            {
                BoxCollider2D box = (BoxCollider2D)this.collider;
                drawBox(box);
            }
            else if (this.collider.GetType() == typeof(CircleCollider2D))
            {
                CircleCollider2D circle = (CircleCollider2D)this.collider;
                drawCircle(circle);
            }
        }

        private void drawPrimitive(float radius, int segments, Vector2 offset)
        {
            int pointCount = segments + 1;
            this.line.positionCount = pointCount;
            Vector3[] points = new Vector3[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                float rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3((Mathf.Sin(rad) * radius) + offset.x, (Mathf.Cos(rad) * radius) + offset.y, 0);
            }

            this.line.SetPositions(points);
        }

        private void drawLine()
        {
            Vector3[] points = new Vector3[2];

            points[0] = Vector2.zero;
            points[1] = new Vector2(this.radius, 0);

            this.line.positionCount = points.Length;
            this.line.SetPositions(points);
        }

        private void drawLaser(GameObject sender, GameObject target)
        {
            if (sender != null && target != null)
                drawLaser(sender.transform.position, target.transform.position);
        }

        private void drawLaser(Vector2 sender, Vector2 target)
        {
            Vector3[] points = new Vector3[2];

            points[0] = new Vector3(sender.x, sender.y, 0);
            points[1] = new Vector3(target.x, target.y, 0);

            this.line.positionCount = points.Length;
            this.line.SetPositions(points);
        }

        private void drawRectangle(Vector2 size, Vector2 offset)
        {
            Vector3[] points = new Vector3[5];

            points[0] = new Vector3(offset.x - (size.x / 2), offset.y - (size.y / 2), 0);
            points[1] = new Vector3(offset.x + (size.x / 2), offset.y - (size.y / 2), 0);
            points[2] = new Vector3(offset.x + (size.x / 2), offset.y + (size.y / 2), 0);
            points[3] = new Vector3(offset.x - (size.x / 2), offset.y + (size.y / 2), 0);
            points[4] = new Vector3(offset.x - (size.x / 2), offset.y - (size.y / 2), 0);

            this.line.positionCount = points.Length;
            this.line.SetPositions(points);
        }

        private void drawPolygon(PolygonCollider2D polygon)
        {
            List<Vector3> points = new List<Vector3>();

            foreach (Vector2 point in polygon.points)
            {
                points.Add(new Vector3(point.x + polygon.offset.x, point.y + polygon.offset.y, 0));
            }

            points.Add(points[0]);

            this.line.positionCount = points.Count;
            this.line.SetPositions(points.ToArray());
        }

        private void drawBox(BoxCollider2D box)
        {
            Vector2 offset = box.offset;
            Vector2 size = box.size;

            drawRectangle(size, offset);
        }

        private void drawCircle(CircleCollider2D circle)
        {
            int segments = setSegments(PrimitiveType.circle);
            drawPrimitive(circle.radius, segments, circle.offset);
        }

        #endregion
    }
}