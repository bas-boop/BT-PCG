using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public sealed class LogNode : Node
    {
        private readonly string _test;
        
        public LogNode(string test)
        {
            _test = test;
        }
        
        protected override NodeStatus OnUpdate()
        {
            Debug.Log(_test);
            return NodeStatus.SUCCES;
        }
    }
}