using UnityEngine;

using Framework.BehaviourTreeSystem;
using Framework.BehaviourTreeSystem.Nodes;
using Framework.BehaviourTreeSystem.Nodes.TaskNodes;

namespace NPC
{
    public sealed class GuardEnemy : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private bool see;
        [SerializeField] private bool weapon;
        [SerializeField] private bool range;
        
        private Node _tree;
        private Node _patrol;
        private Node _attack;

        DictWrapper dictWrapper = new ();
        
        private bool _canSeePlayer;
        private bool _hasWeapon;
        private bool _isInRange;

        private void Start()
        {
            Node screachWeapon = new TestNode("");
            Vector2 playerPos = Vector2.zero;

            dictWrapper.Set("canSeePlayer", false);
            dictWrapper.Set("hasWeapon", false);
            dictWrapper.Set("isInRange", false);
            
            // todo: make it work, this is only structure
            _attack = new SelectorNode(
                new InvertNode(new ConditionalNode("canSeePlayer", new ConditionalNode("hasWeapon", screachWeapon))),
                new InvertNode(new ConditionalNode("isInRange", new MoveNode(gameObject, playerPos, speed))),
                new TestNode("Make an attack node")
            );

            // todo: fix patrol
            _patrol = new SequenceNode();
            
            _tree = new SelectorNode(
                _attack,
                _patrol
            );
            
            _tree.SetDictWrapper(dictWrapper);
        }

        private void Update()
        {
            // todo: update all bools
            dictWrapper.Set("canSeePlayer", see);
            dictWrapper.Set("hasWeapon", weapon);
            dictWrapper.Set("isInRange", range);
            
            _tree.Update();
        }
    }
}