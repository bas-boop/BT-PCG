namespace Framework.BehaviourTreeSystem.Nodes
{
    public sealed class InvertNode : Node
    {
        private Node _nodeToInvert;
        private NodeStatus _otherNodeStatus;
        
        public InvertNode(Node nodeToInvert)
        {
            _nodeToInvert = nodeToInvert;
            _otherNodeStatus = _nodeToInvert.GetStatus();
        }

        protected override NodeStatus OnUpdate()
        {
            _nodeToInvert.Update();
            _otherNodeStatus = _nodeToInvert.GetStatus();
            
            return _otherNodeStatus switch
            {
                NodeStatus.SUCCES => NodeStatus.FAILED,
                NodeStatus.FAILED => NodeStatus.SUCCES,
                _ => NodeStatus.RUNNING
            };
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _nodeToInvert.SetDictWrapper(p_dictWrapper);
        }
        
        public override string NodeName() => $"InvertNode - {_nodeToInvert.NodeName()}";
    }
}