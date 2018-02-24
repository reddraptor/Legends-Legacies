using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "prefab_tables", menuName = "Virtual Raptor/Prefab Tables")]

    public class PrefabTables : ScriptableObject
    {
        public TileSet[] tileSetTable;
    }
}
