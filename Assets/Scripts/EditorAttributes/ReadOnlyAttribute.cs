using UnityEngine;

namespace Assets.Scripts.EditorAttributes
{
    // Forces the Inspector to use ReadOnlyDrawer to display the value of the property, but not be able to edit it.
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}
