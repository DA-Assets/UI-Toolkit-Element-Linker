using System.Reflection;

namespace DA_Assets.ULB
{
    public class UitkConstants
    {
        public const string Product = "UITK Linker";
        public const string Publisher = "D.A. Assets";
        public const int DEFAULT_INDEX = -1;

        public const string NO_FIELD_PROP = "<No Field or Property>";
        public const string MISSING_FIELD_PROP = "<Missing {0}>";

        public const int RECOMENDED_DEPTH = 3;
        public const string MAX_DEPTH_TOOLTIP = "A value that indicates how deep a field you can access.\n\nFor example, with a depth of 2: MyComponent/MyField.";
        public const string SHOW_PRIMITIVE_TYPE_TOOLTIP = "If enabled, the names of primitive types will be shown in the field names within the binding hierarchy.\n\nExample: string, float, bool.";
        public const string SHOW_OBJ_TYPE_TOOLTIP = "If enabled, the field types will be displayed in the field names within the binding hierarchy.\n\nExample: MonoBehaviour, Image, Text.";
        public const string SHOW_TO_STRING_TOOLTIP = "In Unity, some fields have a built-in capability to provide a formatted result using the ToString method.\n\nIf you enable this checkbox, you will see which fields support automatic formatting.\n\nFor example, the 'Color' structure will look like this: RGBA(1.000, 1.000, 1.000, 1.000).";
        public const string OBSOLETE_WARNING_TOOLTIP = "It is not recommended to bind fields that have the Obsolete attribute, as they may be removed in future versions of Unity, and the binding with your field will be lost.";

        public const char FIELD_SEPARATOR = '/';
        public const BindingFlags BINDING_FLAGS =
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public |
            BindingFlags.NonPublic;

        public static bool SHOW_TOSTRING = true;
        public static bool SHOW_OBSOLETE = false;
        public static bool SHOW_PRIMITIVE_TYPE = true;
        public static bool SHOW_OBJECT_TYPE = true;
    }
}