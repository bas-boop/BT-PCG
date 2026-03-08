using System;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public class FunctionNode : Node
    {
        private Action _func;
        
        public FunctionNode(Action func)
        {
            _func = func;
        }
        
        protected override NodeStatus OnUpdate()
        {
            return NodeStatus.SUCCES;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _func?.Invoke();
        }
    }
}