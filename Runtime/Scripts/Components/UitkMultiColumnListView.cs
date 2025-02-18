#if UNITY_6000_0_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkMultiColumnListView : UitkLinker<MultiColumnListView> { }

    [UxmlElement(nameof(MultiColumnListViewG))]
    public partial class MultiColumnListViewG : UnityEngine.UIElements.MultiColumnListView, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public MultiColumnListViewG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
}
#endif