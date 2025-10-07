using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkTextElement : UitkLinker<TextElement> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(TextElementG))]
    public partial class TextElementG : UnityEngine.UIElements.TextElement, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public TextElementG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class TextElementG : UnityEngine.UIElements.TextElement, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<TextElementG, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
            {
                name = "text"
            };

            private UxmlBoolAttributeDescription m_EnableRichText = new UxmlBoolAttributeDescription
            {
                name = "enable-rich-text",
                defaultValue = true
            };

            private UxmlBoolAttributeDescription m_ParseEscapeSequences = new UxmlBoolAttributeDescription
            {
                name = "parse-escape-sequences",
                defaultValue = false
            };

            private UxmlBoolAttributeDescription m_DisplayTooltipWhenElided = new UxmlBoolAttributeDescription
            {
                name = "display-tooltip-when-elided"
            };

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

                TextElementG obj = (TextElementG)ve;
                obj.text = m_Text.GetValueFromBag(bag, cc);
#if UNITY_2021_3_OR_NEWER
                obj.enableRichText = m_EnableRichText.GetValueFromBag(bag, cc);
#endif
#if UNITY_2022_1_OR_NEWER
                obj.parseEscapeSequences = m_ParseEscapeSequences.GetValueFromBag(bag, cc);
#endif
#if UNITY_2021_3_OR_NEWER
                obj.displayTooltipWhenElided = m_DisplayTooltipWhenElided.GetValueFromBag(bag, cc);
#endif

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}