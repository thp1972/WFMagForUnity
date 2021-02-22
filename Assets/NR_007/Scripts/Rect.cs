using UnityEngine;

namespace NR_007
{
    struct Rect
    {
        public GameObject Go { get; }
        public Vector3 Position { get; }
        public Vector3 Size { get; }

        public Rect(GameObject go, Vector2 position, Vector2 size)
        {
            Go = go;
            Size = new Vector3(size.x, size.y, 1);
            Position = position;
            Go.transform.localScale = Size;
            // starting from top-left and taking in account center pivot
            Go.transform.position = new Vector2(
                ScreenUtility.TopLeft.x + position.x + size.x / 2,
                ScreenUtility.TopLeft.y - position.y - size.y / 2);
        }

        public float X // in Unity coordinate system
        {
            get { return Go.transform.position.x; }
        }

        public float Y // in Unity coordinate system
        {
            get { return Go.transform.position.y; }
        }

        public void SetColor(Color32 color)
        {
            Go.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}