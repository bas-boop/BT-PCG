namespace Framework.BehaviourTreeSystem.Nodes
{
    public class ConditionalNode : Node
    {
        private bool _condition;
        private string _conditionName = string.Empty;
        private Node _node;
        
        public ConditionalNode(bool condition, Node node)
        {
            _condition = condition;
            _node = node;
        }

        public ConditionalNode(string conditionName, Node node)
        {
            _conditionName = conditionName;
            _node = node;
        }
        
        public void UpdateCondition(bool newCondition) => _condition = newCondition;
        
        protected override NodeStatus OnUpdate()
        {
            if (_conditionName != string.Empty)
                _condition = p_dictWrapper.Get<bool>(_conditionName);
            
            return _condition ? _node.Update() : NodeStatus.SUCCES;
        }
        
        protected override void OnEnter()
        {
            base.OnEnter();
            _node.SetDictWrapper(p_dictWrapper);
        }
    }
}