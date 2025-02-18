#if UNITY_2022_1_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkUnsignedIntegerField : UitkLinker<UnsignedIntegerField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(UnsignedIntegerFieldG))]
    public partial class UnsignedIntegerFieldG : UnityEngine.UIElements.UnsignedIntegerField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public UnsignedIntegerFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class UnsignedIntegerFieldG : UnityEngine.UIElements.UnsignedIntegerField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<UnsignedIntegerFieldG, UxmlTraits> { }

        public new class UxmlTraits : TextValueFieldTraits<uint, UxmlUnsignedIntAttributeDescription>
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                UnsignedIntegerFieldG obj = ve as UnsignedIntegerFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif