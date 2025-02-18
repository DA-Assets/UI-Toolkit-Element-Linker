#if UNITY_2022_1_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkDoubleField : UitkLinker<DoubleField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(DoubleFieldG))]
    public partial class DoubleFieldG : UnityEngine.UIElements.DoubleField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public DoubleFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class DoubleFieldG : UnityEngine.UIElements.DoubleField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<DoubleFieldG, UxmlTraits> { }

        public new class UxmlTraits : TextValueFieldTraits<double, UxmlDoubleAttributeDescription>
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                DoubleFieldG obj = ve as DoubleFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif