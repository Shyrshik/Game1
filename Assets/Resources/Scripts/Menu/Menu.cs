using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour, IMenu
    {
        private Canvas _Canvas;
        public void Hide()
        {
            if (_Canvas is not null)
                _Canvas.enabled = false;
        }
        public void Show()
        {
            if (_Canvas is not null)
                _Canvas.enabled = true;
        }
        private void Awake()
        {
            _Canvas = GetComponent<Canvas>();
        }
    }
}