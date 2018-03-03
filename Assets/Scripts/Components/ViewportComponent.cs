using UnityEngine;
using System.Collections;
using Assets.Scripts.Serialization;
using Assets.Scripts.EditorAttributes;

namespace Assets.Scripts.Components
{
    public class ViewPortComponent : MonoBehaviour
    {
        public MapComponent map;
        public new Camera camera;
        public LocationComponent location;
        [ReadOnly]public Vector2 lowerLeftPosition;
        [ReadOnly]public Vector2 upperRightPosition;

        private Vector3 viewPortLowerLeft = new Vector3(0,0);
        private Vector3 viewPortUpperRight = new Vector3(1,1);

        // Use this for initialization
        void Start()
        {
            location = GetComponent<LocationComponent>();
            camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            lowerLeftPosition = camera.ViewportToWorldPoint(viewPortLowerLeft);
            upperRightPosition = camera.ViewportToWorldPoint(viewPortUpperRight);
            location.SetFromWorldPosition(transform.position);
        }

        public bool InView(Vector2 position)
        {
            if (position.x + 1 > lowerLeftPosition.x && position.x - 1 < upperRightPosition.x && position.y + 1 > lowerLeftPosition.y && position.y - 1 < upperRightPosition.y)
            {
                return true;
            }

            return false;
        }
    }

}