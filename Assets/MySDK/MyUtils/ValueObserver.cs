using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MyUtils
{
    // 1 Lớp lưu trữ dữ liệu cho phép theo dõi sự thay đổi giá trị theo mô hình observer
    [Serializable, HideReferenceObjectPicker, InlineProperty]
    public class ValueObserver<T>
    {
        [SerializeField, HideLabel] protected T _value;

        private T _oldValue;

        // T - new value
        private Action<T> _onChanged;

        // T 1st - old value, T 2nd - new value
        private Action<T, T> _onChangedFromTo;

        public event Action<T> OnChanged
        {
            add => _onChanged += value;
            remove => _onChanged -= value;
        }

        public event Action<T, T> OnChangedFromTo
        {
            add => _onChangedFromTo += value;
            remove => _onChangedFromTo -= value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(this._value, value))
                {
                    return;
                }

                _oldValue = _value;
                _value = value;

                _onChanged?.Invoke(value);
                _onChangedFromTo?.Invoke(_oldValue, value);
            }
        }

        public ValueObserver()
        {

        }

        public ValueObserver(T value)
        {
            this._value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public void SetWithoutNotify(T value)
        {
            _oldValue = _value;
            _value = value;
        }

        public static implicit operator T(ValueObserver<T> variable)
        {
            return variable._value;
        }
    }
}
