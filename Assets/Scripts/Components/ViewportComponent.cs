using UnityEngine;
using System.Collections;
using Assets.Scripts.Serialization;
using Assets.Scripts.EditorAttributes;

namespace Assets.Scripts.Components
{
    public class ViewPortComponent : MonoBehaviour
    {
        public new Camera camera;
        [ReadOnly]public Vector2 lowerLeft;
        [ReadOnly]public Vector2 upperRight;

        // Use this for initialization
        void Start()
        {
            camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            lowerLeft = camera.ViewportToWorldPoint(new Vector3(0, 0));
            upperRight = camera.ViewportToWorldPoint(new Vector3(1, 1));
        }

        public bool InView(Vector2 position)
        {
            if (position.x + 1 > lowerLeft.x && position.x - 1 < upperRight.x && position.y + 1 > lowerLeft.y && position.y - 1 < upperRight.y)
            {
                return true;
            }

            return false;
        }
    }

}