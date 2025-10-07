#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkVector3Field : UitkLinker<Vector3FieldG, Vector3> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(Vector3FieldG))]
    public partial class Vector3FieldG : UnityEngine.UIElements.Vector3Field, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public Vector3FieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class Vector3FieldG : UnityEngine.UIElements.Vector3Field, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<Vector3FieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<Vector3>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlFloatAttributeDescription m_XValue = new UxmlFloatAttributeDescription
            {
                name = "x"
            };

            private UxmlFloatAttributeDescription m_YValue = new UxmlFloatAttributeDescription
            {
                name = "y"
            };

            private UxmlFloatAttributeDescription m_ZValue = new UxmlFloatAttributeDescription
            {
                name = "z"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                Vector3FieldG obj = (Vector3FieldG)ve;
                obj.SetValueWithoutNotify(new Vector3(m_XValue.GetValueFromBag(bag, cc), m_YValue.GetValueFromBag(bag, cc), m_ZValue.GetValueFromBag(bag, cc)));

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif