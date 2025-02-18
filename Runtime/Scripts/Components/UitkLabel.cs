using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkLabel : UitkLinker<Label> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(LabelG))]
    public partial class LabelG : Label, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public LabelG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class LabelG : Label, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<LabelG, UxmlTraits> {  }

        public new class UxmlTraits : TextElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                LabelG obj = ve as LabelG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}