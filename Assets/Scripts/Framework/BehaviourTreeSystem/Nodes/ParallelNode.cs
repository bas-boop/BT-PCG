using System.Linq;

namespace Framework.BehaviourTreeSystem.Nodes
{
    public class ParallelNode : Node
    {
        private Node[] _nodes;
        
        public ParallelNode(params Node[] nodes)
        {
            _nodes = nodes;
        }
        
        protected override NodeStatus OnUpdate()
        {
            foreach (Node node in _nodes)
            {
                node.Update();
            }

            return _nodes.Any(node => node.GetStatus() != NodeStatus.RUNNING) ? NodeStatus.SUCCES : NodeStatus.RUNNING;
        }
    }
}