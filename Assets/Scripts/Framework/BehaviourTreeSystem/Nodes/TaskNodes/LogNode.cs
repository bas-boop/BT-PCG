using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public sealed class LogNode : Node
    {
        private readonly string _message;
        
        public LogNode(string message) => _message = message;

        protected override NodeStatus OnUpdate()
        {
            Debug.Log(_message);
            return NodeStatus.SUCCES;
        }
    }
}