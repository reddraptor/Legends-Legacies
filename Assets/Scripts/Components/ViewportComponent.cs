using UnityEngine;
using System.Collections;
using Assets.Scripts.Serialization;
using Assets.Scripts.EditorAttributes;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(LocationComponent), typeof(Camera))]
    public class ViewportComponent : MonoBehaviour
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
            map = location.map;
        }

        public bool InView(MapComponent givenMap, Vector2 givenPosition)
        {
            if (givenMap == map)
            {
                if (givenPosition.x + 1 > lowerLeftPosition.x && givenPosition.x - 1 < upperRightPosition.x && givenPosition.y + 1 > lowerLeftPosition.y && givenPosition.y - 1 < upperRightPosition.y)
                {
                    return true;
                }
            }

            return false;
        }
    }

}