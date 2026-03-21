using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public class NoNode : Node
    {
        protected override NodeStatus OnUpdate()
        {
            return NodeStatus.SUCCES;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            p_dictWrapper.Set("Test", 3f);
        }
        
        public override string NodeName() => "NoNode";
    }
}