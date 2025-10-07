#if UNITY_2022_1_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkLongField : UitkLinker<LongField, long> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(LongFieldG))]
    public partial class LongFieldG : UnityEngine.UIElements.LongField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public LongFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class LongFieldG : UnityEngine.UIElements.LongField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<LongFieldG, UxmlTraits> { }

        public new class UxmlTraits : TextValueFieldTraits<long, UxmlLongAttributeDescription>
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                LongFieldG obj = ve as LongFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif