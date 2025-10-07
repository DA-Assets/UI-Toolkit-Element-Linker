using UnityEngine;

namespace DA_Assets.ULB
{
    public class ManualTest : MonoBehaviour
    {
        void Start()
        {
            var t = new UitkLinkerBindingTests();
            t.Setup();
            StartCoroutine(t.AllBindings_UpdateCorrectly_Over5Seconds());
        }
    }
}