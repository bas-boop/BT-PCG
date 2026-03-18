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
        [SerializeField] private bool range;
        
        private Node _tree;
        private Node _patrol;
        private Node _attack;

        private readonly DictWrapper _dictWrapper = new ();
        
        private bool _canSeePlayer;
        private bool _hasWeapon;
        private bool _isInRange;

        private void Start()
        {
            Node screachWeapon = new LogNode("");
            Vector2 playerPos = Vector2.zero;

            _dictWrapper.Set("canSeePlayer", false);
            _dictWrapper.Set("hasWeapon", false);
            _dictWrapper.Set("isInRange", false);
            
            // todo: make it work, this is only structure
            _attack = new SequenceNode(
                new ConditionalNode(new []{"canSeePlayer", "hasWeapon"}, new SequenceNode(
                    new ConditionalNode("canSeePlayer"),
                    new InvertNode(new ConditionalNode("hasWeapon")),
                    new WaitNode(1),
                    new FunctionNode(Search)
                    )
                ),
                //new LogNode("walk to player"),
                new ConditionalNode("isInRange", new MoveNode(gameObject, playerPos, speed)),
                new LogNode("Make an attack node")
            );

            // todo: fix patrol
            _patrol = new SequenceNode();
            
            _tree = new SelectorNode(
                _attack,
                _patrol
            );
            
            _tree.SetDictWrapper(_dictWrapper);
        }

        private void Update()
        {
            // todo: update all bools
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
            _dictWrapper.Set("hasWeapon", true);
        }
    }
}