using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public class SetValueNode<T> : Node
    {
        public SetValueNode(string toChange, T value)
        {
            p_dictWrapper.Set(toChange, value);
        }
        
        protected override NodeStatus OnUpdate()
        {
            Debug.Log("Set value");
            return NodeStatus.SUCCES;
        }
    }
}