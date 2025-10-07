#if UNITY_2021_3_OR_NEWER
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkDropdownField : UitkLinker<DropdownField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(DropdownFieldG))]
    public partial class DropdownFieldG : UnityEngine.UIElements.DropdownField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public DropdownFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class DropdownFieldG : UnityEngine.UIElements.DropdownField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<DropdownFieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<string>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlIntAttributeDescription m_Index = new UxmlIntAttributeDescription
            {
                name = "index"
            };

            private UxmlStringAttributeDescription m_Choices = new UxmlStringAttributeDescription
            {
                name = "choices"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                DropdownFieldG dropdownField = (DropdownFieldG)ve;
                List<string> list = ParseChoiceList(m_Choices.GetValueFromBag(bag, cc));
                if (list != null)
                {
#if UNITY_2021_3_OR_NEWER
                    dropdownField.choices = list;
#endif
                }

                dropdownField.index = m_Index.GetValueFromBag(bag, cc);

                DropdownFieldG obj = ve as DropdownFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }

            private static List<string> ParseChoiceList(string choicesFromBag)
            {
                if (string.IsNullOrEmpty(choicesFromBag.Trim()))
                    return null;

                string[] array = choicesFromBag.Split(',');

                if (array.Length != 0)
                {
                    List<string> list = new List<string>();
                    string[] array2 = array;

                    foreach (string text in array2)
                        list.Add(text.Trim());

                    return list;
                }

                return null;
            }
        }
    }
#endif
}
#endif