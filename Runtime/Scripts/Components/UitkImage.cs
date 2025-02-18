using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkImage : UitkLinker<Image> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(ImageG))]
    public partial class ImageG : UnityEngine.UIElements.Image, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public ImageG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class ImageG : UnityEngine.UIElements.Image, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<ImageG, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get
                {
                    yield break;
                }
            }

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                ImageG obj = ve as ImageG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}