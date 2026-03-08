namespace Framework.BehaviourTreeSystem.Nodes
{
    public class ConditionalNode : Node
    {
        private bool _condition;
        private Node _node;
        
        public ConditionalNode(bool condition, Node node)
        {
            _condition = condition;
            _node = node;
        }
        
        protected override NodeStatus OnUpdate()
        {
            return _condition ? _node.Update() : NodeStatus.SUCCES;
        }
        
        protected override void OnEnter()
        {
            base.OnEnter();
            _node.SetDictWrapper(p_dictWrapper);
        }
    }
}