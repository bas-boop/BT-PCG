using UnityEngine;

using Framework.BehaviourTreeSystem;
using Framework.BehaviourTreeSystem.Nodes;
using Framework.BehaviourTreeSystem.Nodes.TaskNodes;

namespace NPC
{
    public class MoveEnemy : MonoBehaviour
    {
        [SerializeField] private Vector2 beginPosition;
        [SerializeField] private Vector2 endPosition;
        [SerializeField] private Vector2 changePosition;
        [SerializeField] private float speed;
        [SerializeField] private float waitTime;
        
        private Node _tree;
        private MoveNode _moveNode;
        private ConditionalNode _distanceCheckNode;
        
        private void Start()
        {
            DictWrapper dictWrapper = new ();
            dictWrapper.Set("EndPosition", endPosition);

            _moveNode = new (gameObject, endPosition, speed);
            _distanceCheckNode = new ((endPosition - (Vector2)transform.position).magnitude <= 1,
                new FunctionNode(Test));
            
            _tree = new SequenceNode(
                new MoveNode(gameObject, beginPosition, speed),
                new WaitNode(waitTime),
                _moveNode,
                new WaitNode(waitTime),
                //new ConditionalNode((endPosition - (Vector2) transform.position).magnitude <= 1, new SetValueNode<Vector2>("EndPosition", changePosition))
                _distanceCheckNode
            );
        }

        private void Update()
        {
            _distanceCheckNode.UpdateCondition((endPosition - (Vector2)transform.position).magnitude <= 1);
            _tree.Update();
        }

        private void Test()
        {
            _moveNode.SetTargetPosition(changePosition);
            Debug.Log("change");
        }
    }
}