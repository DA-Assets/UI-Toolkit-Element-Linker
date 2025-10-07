using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkLinker<TElement, TBinding> : UitkLinker<TElement>, IUITKLinkerWithBinding where TElement : VisualElement
    {
        [SerializeField] protected bool _bindingEnabled = false;
        public bool BindingEnabled { get => _bindingEnabled; set => _bindingEnabled = value; }

        [SerializeField] protected MemberBinding _binding;
        public MemberBinding Binding { get => _binding; set => _binding = value; }

        [SerializeField] protected Vector2Int _bindingRateMs = new Vector2Int(50, 100);
        public Vector2Int BindingRateMs { get => _bindingRateMs; set => _bindingRateMs = value; }

        [SerializeField] protected UitkBindingSettings _bindingSettings;
        public UitkBindingSettings BindingSettings { get => _bindingSettings; set => _bindingSettings = value; }

        private float _bindingTimer = 0f;

        public void Initialize()
        {
            LinkElement();
            Bind();
        }

        public override void Start()
        {
            base.Start();
            Bind();
        }

        public void Bind()
        {
            if (!_bindingEnabled)
            {
                return;
            }

            if (_binding.TargetObject == null || string.IsNullOrWhiteSpace(_binding.Member))
            {
                return;
            }

            _binding.SetFieldPropInfo();
        }

        protected virtual void Update()
        {
            if (!_bindingEnabled)
            {
                return;
            }

            if (_bindingRateMs.x <= 0 || _bindingRateMs.y <= 0)
            {
                UpdateBinding();
            }
            else
            {
                if (_bindingTimer <= 0f)
                {
                    UpdateBinding();

                    if (_bindingRateMs.x == _bindingRateMs.y)
                    {
                        _bindingTimer = _bindingRateMs.x;
                    }
                    else
                    {
                        _bindingTimer = UnityEngine.Random.Range(_bindingRateMs.x, _bindingRateMs.y);
                    }
                }
                else
                {
                    _bindingTimer -= Time.deltaTime * 1000f;
                }
            }
        }

        protected virtual void UpdateBinding()
        {
            if (_element == null)
            {
                return;
            }

            object value = _binding.GetValue();

            if (value == null)
            {
                return;
            }

            try
            {
                TBinding typedValue = (TBinding)Convert.ChangeType(value, typeof(TBinding));
                SetValue(typedValue);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to convert value for binding. Type '{value.GetType()}' to '{typeof(TBinding)}'.\n{ex}");
            }
        }

        protected virtual void SetValue(TBinding value)
        {
            switch (_element)
            {
                case INotifyValueChanged<TBinding> bindable:
                    Debug.Log($"Setting value of '{_element.name}' {bindable.GetType().Name} to '{value}'");
                    bindable.value = value;
                    break;
                case ProgressBar progressBar when value is float f:
                    progressBar.value = f;
                    break;
                case Image image when value is Sprite s:
                    image.sprite = s;
                    break;
                case Image image when value is Texture2D t:
                    image.image = t;
                    break;
                default:
                    if (_debug)
                    {
                        Debug.LogWarning($"SetValue not implemented for type {_element.GetType()} with binding type {typeof(TBinding)}. Attempting to set 'value' property via reflection.");
                    }
                    try
                    {
                        _element.GetType().GetProperty("value")?.SetValue(_element, value, null);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Fallback binding failed for element '{_element.name}': {ex.Message}");
                    }
                    break;
            }
        }
    }

    public interface IUITKLinkerWithBinding
    {
        bool BindingEnabled { get; set; }
        MemberBinding Binding { get; set; }
        Vector2Int BindingRateMs { get; set; }

        void Initialize();
    }
}
