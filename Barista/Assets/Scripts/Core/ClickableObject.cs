using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Funksoft.Barista
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ClickableObject : MonoBehaviour
    {
        [SerializeField]
        private bool _repeatIfHeld;
        [SerializeField]
        private Sprite _hoverSprite;
        [SerializeField]
        private Sprite _clickedSprite;

        private Sprite _defaultSprite;

        private SpriteRenderer _spriteRenderer;
        private IClickable _clickableComponent;

        private void Awake()
        {
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);
            TryGetComponent<IClickable>(out _clickableComponent);
            if (_clickableComponent == null)
                Debug.LogError("Object contains no clickable behaviour component.");

            _defaultSprite = _spriteRenderer.sprite;
        }

        private void Update()
        {
            //Switch back from clicksprite when released
            if (Input.GetMouseButtonUp(0) && _spriteRenderer.sprite == _clickedSprite)
            {
                _spriteRenderer.sprite = _defaultSprite;
            }
        }

        //Switch to hover sprite when hovering, unless already done, or click is held.
        private void OnMouseOver()
        {
            if (_spriteRenderer.sprite == _hoverSprite || _spriteRenderer.sprite == _clickedSprite)
                return;
            _spriteRenderer.sprite = _hoverSprite;
        }

        //Switch back to default sprite when no longer hovering
        private void OnMouseExit()
        {
            if (_spriteRenderer.sprite == _clickedSprite)
                return;
            _spriteRenderer.sprite = _defaultSprite;
        }

        public void Activate()
        {
            _spriteRenderer.sprite = _clickedSprite;
            _clickableComponent.OnActivation();
        }

        public bool GetRepeatIfHeld()
        {
            return _repeatIfHeld;
        }

    }

    public interface IClickable
    {
        public void OnActivation();
    }
}
