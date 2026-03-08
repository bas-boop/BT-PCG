using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public class YesNode : Node
    {
        protected override NodeStatus OnUpdate()
        {
            Debug.Log(p_dictWrapper.Get<float>("A"));
            Debug.Log(p_dictWrapper.Get<float>("Test"));
            return NodeStatus.SUCCES;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
        }
    }
}