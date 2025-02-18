using System;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public static class GuidGenerator
    {
        public static string GenerateGuid(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                guid = Guid.NewGuid().ToString("N");
            }

            return guid;
        }

        public static void GenerateGuid(UxmlStringAttributeDescription m_Guid, IHaveGuid ihg, IUxmlAttributes bag, CreationContext cc)
        {
            string guid = Guid.NewGuid().ToString();
            guid = guid.Replace("-", "");

            ihg.guid = m_Guid.GetValueFromBag(bag, cc);

            if (ihg.guid == "default_value")
            {
                ihg.guid = guid;
            }
        }

        public static UxmlStringAttributeDescription GetGuidField()
        {
            return new UxmlStringAttributeDescription { name = nameof(IHaveGuid.guid), defaultValue = "default_value" };
        }
    }

    public interface IHaveGuid
    {
        string guid { get; set; }
    }
}