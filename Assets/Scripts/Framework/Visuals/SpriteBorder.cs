using UnityEngine;

namespace Framework.Visuals
{
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteBorder : MonoBehaviour
    {
        [SerializeField] private ShapeType shape;
        [SerializeField] private Color color;
        
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            SpriteMaker.MakeSprite(_spriteRenderer, shape, color);
        }
    }
}