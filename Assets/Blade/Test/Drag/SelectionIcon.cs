using UnityEngine;
using UnityEngine.UI;

namespace Blade.Test.Drag
{
    public class SelectionIcon : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        public RectTransform RectTrm => transform as RectTransform;

        public void SetPosition(Vector2 position)
        {
            RectTrm.anchoredPosition = position;
        }

        public void SetSize(Vector2 size)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = size.x < 0 ? -1 : 1;
            localScale.y = size.y < 0 ? -1 : 1;

            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);

            transform.localScale = localScale;
            RectTrm.sizeDelta = size;
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}