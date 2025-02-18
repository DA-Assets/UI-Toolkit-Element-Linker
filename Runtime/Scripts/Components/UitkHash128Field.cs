#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkHash128Field : UitkLinker<Hash128Field> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(Hash128FieldG))]
    public partial class Hash128FieldG : UnityEngine.UIElements.Hash128Field, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public Hash128FieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class Hash128FieldG : UnityEngine.UIElements.Hash128Field, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<Hash128FieldG, UxmlTraits> { }

        public new class UxmlTraits : TextValueFieldTraits<Hash128, UxmlHash128AttributeDescription>
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                Hash128FieldG obj = ve as Hash128FieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif