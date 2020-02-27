using UnityEngine;
using System.Collections;


namespace KirinUtil {
    public static class TimerDelegate {

        public delegate void VoidDelegate();
        public delegate void BoolDelegate(bool value);
        public delegate void FloatDelegate(float value);
        public delegate void VectorDelegate(Vector2 value);
        public delegate void StringDelegate(string value);
        public delegate void ObjectDelegate(GameObject value);
        public delegate void KeyCodeDelegate(KeyCode value);

        public delegate void VoidGameObjectDelegate(GameObject gameObject);
        public delegate void BoolGameObjectDelegate(GameObject gameObject, bool value);
        public delegate void FloatGameObjectDelegate(GameObject gameObject, float value);
        public delegate void VectorGameObjectDelegate(GameObject gameObject, Vector2 value);
        public delegate void StringGameObjectDelegate(GameObject gameObject, string value);
        public delegate void ObjectGameObjectDelegate(GameObject gameObject, GameObject gameObject2);
        public delegate void KeyCodeGameObjectDelegate(GameObject gameObject, KeyCode value);
    }
}