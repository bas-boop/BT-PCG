using UnityEngine;

namespace Framework.BehaviourTreeSystem
{
    public sealed class TestNode : Node
    {
        private string test;
        
        public TestNode(string test)
        {
            this.test = test;
        }
        
        protected override NodeStatus OnUpdate()
        {
            Debug.Log(test);
            return NodeStatus.SUCCES;
        }
    }
}