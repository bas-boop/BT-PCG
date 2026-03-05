using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes
{
    public class WaitNode : Node
    {
        protected float p_maxWaitTime;
        protected float p_currentWaitTime;

        public WaitNode(float time)
        {
            p_maxWaitTime = time;
        }
        
        protected override void OnEnter() => p_currentWaitTime = p_maxWaitTime;
        
        protected override NodeStatus OnUpdate()
        {
            if (p_currentWaitTime <= 0)
                return NodeStatus.SUCCES;
            
            p_currentWaitTime -= Time.deltaTime;
            
            return p_currentWaitTime > 0 ? NodeStatus.RUNNING : NodeStatus.SUCCES;
        }
    }
}