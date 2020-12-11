using UnityEngine.UI;

namespace Tofunaut.TofuUnity.UI
{
    /// <summary>
    /// A non-drawing graphic is an efficient way to add the ability to hit a UI element that does not need any drawn graphics. Like an input rect.
    /// Based on the implementation described at:
    /// https://answers.unity.com/questions/1091618/ui-panel-without-image-component-as-raycast-target.html
    /// </summary>
    public class NonDrawingGraphic : Graphic
    {
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}