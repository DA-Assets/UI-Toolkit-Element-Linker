using System;
using System.Reflection;

namespace DA_Assets.UEL
{
    public class JsonSerializerInternal
    {
        private static MethodInfo serializeMethod;

        public static string Serialize(object obj, bool pretty = false, string indentText = "  ")
        {
            if (serializeMethod == null)
            {
                Type jsonType = Type.GetType("UnityEditor.Json+Serializer, UnityEditor.CoreModule");

                serializeMethod = jsonType.GetMethod("Serialize", BindingFlags.Public | BindingFlags.Static, null, new Type[]
                {
                    typeof(object), typeof(bool), typeof(string)
                }, null);
            }
        
            object[] parameters = new object[] { obj, pretty, indentText };
            string result = (string)serializeMethod.Invoke(null, parameters);

            return result;
        }
    }
}