#if UNITY_6000_0_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkMultiColumnTreeView : UitkLinker<MultiColumnTreeView> { }

    [UxmlElement(nameof(MultiColumnTreeViewG))]
    public partial class MultiColumnTreeViewG : UnityEngine.UIElements.MultiColumnTreeView, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public MultiColumnTreeViewG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
}
#endif