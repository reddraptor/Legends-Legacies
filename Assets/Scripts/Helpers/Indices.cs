using System;
using Assets.Scripts.EditorAttributes;

namespace Assets.Scripts.Helpers
{
    [Serializable]
    public class Indices
    {
        [ReadOnly] public int i = 0;
        [ReadOnly] public int j = 0;

        public Indices(int i, int j)
        {
            this.i = i; this.j = j;
        }
    }

}
