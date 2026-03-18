using UnityEngine;

using Framework.Extensions;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public class MoveNode : Node
    {
        private const float REACHED_TARGET_RANGE = 0.05F;
        
        private readonly GameObject _objectToMove;
        private Vector2 _targetPosition;
        private readonly string _targetPositionName = string.Empty;
        private readonly float _speed;
        
        public MoveNode(GameObject objectToMove, Vector2 targetPosition, float speed)
        {
            _objectToMove = objectToMove;
            _targetPosition = targetPosition;
            _speed = speed;
        }
        
        public MoveNode(GameObject objectToMove, string targetPositionName, float speed)
        {
            _objectToMove = objectToMove;
            _targetPositionName = targetPositionName;
            _speed = speed;
        }

        public void SetTargetPosition(Vector2 targetPosition) => _targetPosition = targetPosition;
        
        protected override NodeStatus OnUpdate()
        {
            if (_targetPositionName != string.Empty)
                _targetPosition = p_dictWrapper.Get<Vector2>("playerPosition");
            
            if (_objectToMove.transform.position.IsWithinRange(_targetPosition, REACHED_TARGET_RANGE))
                return NodeStatus.SUCCES;
            
            Vector2 dir = _targetPosition - (Vector2) _objectToMove.transform.position;
            _objectToMove.transform.Translate(dir.normalized * (_speed * Time.deltaTime), Space.World);
            
            return NodeStatus.RUNNING;
        }
    }
}