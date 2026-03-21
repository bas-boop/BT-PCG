using System;

namespace Framework.BehaviourTreeSystem.Nodes
{
    public class SelectorNode : Node
    {
        private Node[] _nodes;
        private int _i;
        
        public SelectorNode(params Node[] nodes)
        {
            _nodes = nodes;
        }
        
        protected override NodeStatus OnUpdate()
        {
            for (; _i < _nodes.Length; _i++)
            {
                switch (_nodes[_i].Update())
                {
                    case NodeStatus.RUNNING:
                        _i = 0;
                        return NodeStatus.RUNNING;
                    case NodeStatus.FAILED:
                        continue;
                    case NodeStatus.SUCCES:
                        _i = 0;
                        return NodeStatus.SUCCES;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return NodeStatus.RUNNING;
        }
        
        protected override void OnEnter()
        {
            base.OnEnter();
            
            foreach (Node node in _nodes)
            {
                node.SetDictWrapper(p_dictWrapper);
            }
        }
        
        public override string NodeName() => "SelectorNode " + _nodes[_i].NodeName();
    }
}