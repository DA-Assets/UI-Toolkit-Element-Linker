using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkTextField : UitkLinker<TextField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(TextFieldG))]
    public partial class TextFieldG : UnityEngine.UIElements.TextField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public TextFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class TextFieldG : UnityEngine.UIElements.TextField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<TextFieldG, UxmlTraits> { }

        public new class UxmlTraits : TextInputBaseField<string>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private static readonly UxmlStringAttributeDescription k_Value = new UxmlStringAttributeDescription
            {
                name = "value",
                obsoleteNames = new string[1] { "text" }
            };

            private UxmlBoolAttributeDescription m_Multiline = new UxmlBoolAttributeDescription
            {
                name = "multiline"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                TextFieldG obj = (TextFieldG)ve;
                obj.multiline = m_Multiline.GetValueFromBag(bag, cc);
                base.Init(ve, bag, cc);
                string value = string.Empty;
                if (k_Value.TryGetValueFromBag(bag, cc, ref value))
                {
                    obj.SetValueWithoutNotify(value);
                }

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}