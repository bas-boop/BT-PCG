using UnityEngine;

using Framework.BehaviourTreeSystem;
using Framework.BehaviourTreeSystem.Nodes;
using Framework.BehaviourTreeSystem.Nodes.TaskNodes;

namespace NPC
{
    public sealed class GuardEnemy : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        
        [SerializeField] private float speed = 1;
        [SerializeField] private bool see;
        [SerializeField] private bool range;
        
        private Node _tree;
        private Node _patrolTree;
        private Node _attackTree;

        private readonly DictWrapper _dictWrapper = new ();
        
        private bool _canSeePlayer;
        private bool _hasWeapon;
        private bool _isInRange;

        private void Start()
        {
            Node screachWeapon = new LogNode("");

            _dictWrapper.Set("playerPosition", Vector2.zero);
            _dictWrapper.Set("canSeePlayer", false);
            _dictWrapper.Set("hasWeapon", false);
            _dictWrapper.Set("isInRange", false);
            
            // todo: make it work, this is only structure
            _attackTree = new SequenceNode(
                new ConditionalNode(new SequenceNode(
                    new ConditionalNode("canSeePlayer"),
                        new InvertNode(new ConditionalNode("hasWeapon")),
                        new WaitNode(1),
                        new FunctionNode(Search)
                    ),
                    "canSeePlayer", "hasWeapon"
                ),
                new MoveNode(gameObject, "playerPosition", speed),
                new ConditionalNode("isInRange", new LogNode("Make an attack node"), true)
            );

            // todo: fix patrol
            _patrol = new SequenceNode();
            
            _tree = new SelectorNode(
                _attackTree,
                _patrolTree
            );
            
            _tree.SetDictWrapper(_dictWrapper);
        }

        private void Update()
        {
            _dictWrapper.Set("playerPosition", (Vector2) player.transform.position);
            
            // todo: update all bools correctly
            _dictWrapper.Set("canSeePlayer", see);
            _dictWrapper.Set("isInRange", range);
            
            _tree.Update();

            // to see in inspector
            _canSeePlayer = _dictWrapper.Get<bool>("canSeePlayer");
            _hasWeapon = _dictWrapper.Get<bool>("hasWeapon");
            _isInRange = _dictWrapper.Get<bool>("isInRange");
        }

        private void Search()
        {
            Debug.Log("picked up weapon");
            _dictWrapper.Set("hasWeapon", true);
        }
    }
}