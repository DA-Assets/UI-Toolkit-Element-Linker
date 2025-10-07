#if UNITY_6000_0_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkTwoPaneSplitView : UitkLinker<TwoPaneSplitView> { }

    [UxmlElement(nameof(TwoPaneSplitViewG))]
    public partial class TwoPaneSplitViewG : UnityEngine.UIElements.TwoPaneSplitView, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public TwoPaneSplitViewG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
}
#endif