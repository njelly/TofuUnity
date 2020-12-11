using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace Tofunaut.TofuUnity.UI.Editor
{
    /// <summary>
    /// Based on the implementation described at:
    /// https://answers.unity.com/questions/1091618/ui-panel-without-image-component-as-raycast-target.html
    /// </summary>
    [CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
    public class NonDrawingGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();
            EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
            base.RaycastControlsGUI();
            base.serializedObject.ApplyModifiedProperties();
        }
    }
}
