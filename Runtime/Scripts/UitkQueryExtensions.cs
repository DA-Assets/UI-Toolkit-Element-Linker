using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public static class UitkQueryExtensions
    {
        public static T GetByPath<T>(this VisualElement root, params ElementIndexName[] path) where T : VisualElement =>
            UitkLinkerBase.FindComponentByHierarchy(
                root,
                path,
                ve => ve.name,
                depth: 0,
                debug: false,
                gameObject: null) as T;

        public static T GetByGuid<T>(this VisualElement root, string guid) where T : VisualElement =>
            UitkLinkerBase.FindGuidRecursive(root, guid) as T;

        public static T GetByGuidHierarchy<T>(this VisualElement root, params string[] guids) where T : VisualElement =>
            UitkLinkerBase.FindComponentByGuidHierarchy(
                root,
                guids,
                gh => gh.guid,
                depth: 0,
                debug: false,
                gameObject: null) as T;

        public static bool GetCanRead(this MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo mfi:
                    return true;
                case PropertyInfo mpi:
                    return mpi.CanRead;
                default:
                    throw new ArgumentException("MemberInfo must be if type FieldInfo or PropertyInfo", nameof(member));
            }
        }
    }
}