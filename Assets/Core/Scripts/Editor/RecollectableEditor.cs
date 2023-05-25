using InterOrbital.Recollectables;
using UnityEditor;

namespace InterOrbital.EditorTools
{
    [CustomEditor(typeof(RecollectableScriptableObject), true)]
    public class RecollectableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}