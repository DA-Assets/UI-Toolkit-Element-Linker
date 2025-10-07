#if UNITY_2021_3_OR_NEWER
using UnityEngine;

namespace DA_Assets.ULB
{
    public class UelExample : MonoBehaviour
    {
        [SerializeField] UitkButton addPhotoBtn;

        private void Start()
        {
            //Access to your UITK element.
            UnityEngine.UIElements.Button btn = addPhotoBtn.E;
            Debug.Log(btn.name);
        }

        //You don't need to use 'RegisterCallback<ClickEvent>'.
        public void AddPhoto_OnClick()
        {
            Debug.Log($"{addPhotoBtn.E.name} clicked!");
        }
    }
}
#endif