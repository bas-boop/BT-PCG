using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteFlasher : MonoBehaviour
    {
        [SerializeField] private float flashTime = 1;
        [SerializeField] private float colorDarkness = 0.75f;
        
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private Color _flashColor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            
            _flashColor = new (
                _startColor.r * colorDarkness,
                _startColor.g * colorDarkness,
                _startColor.b * colorDarkness,
                _startColor.a
            );
        }

        public void Flash() => StartCoroutine(FlashSequence());

        private IEnumerator FlashSequence()
        {
            _spriteRenderer.color = _flashColor;
            yield return new WaitForSeconds(flashTime);
            _spriteRenderer.color = _startColor;
        }
    }
}