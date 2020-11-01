using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public abstract class InputValue
    {
        public float TimePressed { get; protected set; } = float.MinValue;
        public float TimeReleased { get; protected set; } = float.MinValue;

        public bool Held => TimePressed >= 0f && TimePressed >= TimeReleased;
        public bool WasReleased => Mathf.Abs(TimeReleased - Time.time).IsApproximately(0f);
        public bool WasPressed => Mathf.Abs(TimePressed - Time.time).IsApproximately(0f);
    }
    
    public class InputButton : InputValue
    {
        public void Press() => TimePressed = Time.time;
        public void Release() => TimeReleased = Time.time;

        public static implicit operator bool(InputButton b) => b.Held;
    }

    public class InputSingleAxis : InputValue
    {
        public float Axis { get; private set; }

        public void SetAxis(float axis)
        {
            if (Axis.IsApproximately(0f) && !axis.IsApproximately(0f))
                TimePressed = Time.time;
            else if (!Axis.IsApproximately(0f) && axis.IsApproximately(0f))
                TimeReleased = Time.time;

            Axis = axis;
        }

        public static explicit operator float(InputSingleAxis s) => s.Held ? s.Axis : 0f;
    }

    public class InputDoubleAxis : InputValue
    {
        public Vector2 Axis { get; private set; }

        public void SetAxis(Vector2 axis)
        {
            if (Axis.IsApproximately(Vector2.zero) && !axis.IsApproximately(Vector2.zero))
                TimePressed = Time.time;
            else if (!Axis.IsApproximately(Vector2.zero) && axis.IsApproximately(Vector2.zero))
                TimeReleased = Time.time;

            Axis = axis;
        }

        public static explicit operator Vector2(InputDoubleAxis d) => d.Held ? d.Axis : Vector2.zero;
    }
}