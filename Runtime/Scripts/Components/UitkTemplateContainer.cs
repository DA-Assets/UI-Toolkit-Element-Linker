#if UNITY_6000_0_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkTemplateContainer : UitkLinker<TemplateContainer> { }

    [UxmlElement(nameof(TemplateContainerG))]
    public partial class TemplateContainerG : UnityEngine.UIElements.TemplateContainer, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public TemplateContainerG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
}
#endif