using System;

namespace Framework.BehaviourTreeSystem.Nodes
{
    public class SequenceNode : Node
    {
        private Node[] _nodes;
        private int _i;

        public SequenceNode(params Node[] nodes) => _nodes = nodes;

        protected override NodeStatus OnUpdate()
        {
            for (; _i < _nodes.Length; _i++)
            {
                switch (_nodes[_i].Update())
                {
                    case NodeStatus.RUNNING:
                        return NodeStatus.RUNNING;
                    case NodeStatus.FAILED:
                        _i = 0;
                        return NodeStatus.FAILED;
                    case NodeStatus.SUCCES:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            _i = 0;
            return NodeStatus.SUCCES;
        }
    }
}