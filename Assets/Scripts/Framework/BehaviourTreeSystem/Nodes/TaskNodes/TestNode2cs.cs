using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public sealed class TestNode2 : Node
    {
        private string test;
        
        public TestNode2(string test)
        {
            this.test = test;
        }
        
        protected override NodeStatus OnUpdate()
        {
            Debug.Log(test);
            return NodeStatus.FAILED;
        }
    }
}