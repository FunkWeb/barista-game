using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//CLICKABLE OBJECT SCRIPT RUNS AT CUSTOM SCRIPT ORDER EXECUTION Default + 5. Check Script order Execution in project settings if unexpected order bugs appears.
//This is done so the customer DataObject can be set in the start method, while ensuring this scipts start method runs after it, as to avoid Nullreference should the order of the Start() events be different. 
namespace Funksoft.Barista
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ClickableObject : MonoBehaviour
    {
        [SerializeField]
        private bool _repeatIfHeld;

        private Sprite _defaultSprite;
        private Sprite _hoverSprite;
        private Sprite _clickedSprite;

        private SpriteRenderer _spriteRenderer;
        private IClickable _clickableComponent;

        private void Awake()
        {
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);
            TryGetComponent<IClickable>(out _clickableComponent);
            if (_clickableComponent == null)
                Debug.LogError("Object contains no clickable behaviour component.");
        }

        private void Start()
        {
            _defaultSprite = _spriteRenderer.sprite;
            _hoverSprite = _clickableComponent.GetHoverSprite();
            _clickedSprite = _clickableComponent.GetClickedSprite();
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
        public Sprite GetHoverSprite();
        public Sprite GetClickedSprite();
    }
}
