using System;
using UnityEngine;
using UnityEngine.Events;

using Framework.Attributes;
using Framework.Extensions;

namespace Framework.TriggerArea
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class TriggerArea2D : MonoBehaviour
    {
        private const string PNG_SUFFIX = ".png";
        private const string SPRITE_PATH = "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/";
        
        [SerializeField] private StandardSprites shapeToUse;
        [SerializeField, Tag] private string tagToTriggerWith = "Player";
        [SerializeField] private TriggerBehaviour behaviour;
        [SerializeField] private bool isOneTimeUse;
        
        [Space(20)]
        [SerializeField] private UnityEvent<GameObject> onEnter = new();
        [SerializeField] private UnityEvent<GameObject> onExit = new();

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;
        private CircleCollider2D _sphereCollider;
        private CapsuleCollider2D _capsuleCollider;

        private bool _isTriggered;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _sphereCollider = GetComponent<CircleCollider2D>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();

            _boxCollider.isTrigger = true;
            _sphereCollider.isTrigger = true;
            _capsuleCollider.isTrigger = true;
            
            if (!_spriteRenderer)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            _spriteRenderer.sprite = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (behaviour == TriggerBehaviour.EXIT_ONLY
                || CheckOneTimeUse()
                || !other.CompareTag(tagToTriggerWith))
                return;

            _isTriggered = true;
            onEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (behaviour == TriggerBehaviour.ENTER_ONLY
                || CheckOneTimeUse()
                || !other.CompareTag(tagToTriggerWith))
                return;
            
            _isTriggered = true;
            onExit?.Invoke(other.gameObject);
        }
        
        public void TestTrigger() => Debug.Log(shapeToUse);

#if UNITY_EDITOR
        private void OnValidate() => UpdateMesh();

        private void UpdateMesh()
        {
            string spriteName = shapeToUse.GetStringValue();
            Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
                SPRITE_PATH + spriteName + PNG_SUFFIX
            );

            if (sprite != null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
                _spriteRenderer.sprite = sprite;
            }
            else
                Debug.LogWarning($"[TriggerArea] Sprite not found: {spriteName}");

            _boxCollider = GetComponent<BoxCollider2D>();
            _sphereCollider = GetComponent<CircleCollider2D>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            
            switch (shapeToUse)
            {
                case StandardSprites.BOX:
                    _boxCollider.enabled = true;
                    _sphereCollider.enabled = false;
                    _capsuleCollider.enabled = false;
                    break;
                
                case StandardSprites.CIRCLE:
                    _boxCollider.enabled = false;
                    _sphereCollider.enabled = true;
                    _capsuleCollider.enabled = false;
                    break;
                
                case StandardSprites.CAPSULE:
                    _boxCollider.enabled = false;
                    _sphereCollider.enabled = false;
                    _capsuleCollider.enabled = true;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
#endif

        private bool CheckOneTimeUse() => isOneTimeUse && _isTriggered;
    }
}