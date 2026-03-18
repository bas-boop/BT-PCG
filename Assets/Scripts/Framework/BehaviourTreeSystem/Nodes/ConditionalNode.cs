namespace Framework.BehaviourTreeSystem.Nodes
{
    public class ConditionalNode : Node
    {
        private bool _condition;
        private string _conditionName = string.Empty;
        private string[] _conditionNames;
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
        
        public ConditionalNode(string[] conditionNameName, Node node)
        {
            _conditionNames = conditionNameName;
            _node = node;
        }

        public ConditionalNode(string conditionName)
        {
            _conditionName = conditionName;
        }
        
        public ConditionalNode(string[] conditionNameName)
        {
            _conditionNames = conditionNameName;
        }
        
        public void UpdateCondition(bool newCondition) => _condition = newCondition;
        
        protected override NodeStatus OnUpdate()
        {
            if (_conditionName != string.Empty)
                _condition = p_dictWrapper.Get<bool>(_conditionName);
            else if (_conditionNames.Length > 0)
            {
                foreach (string conditionName in _conditionNames)
                {
                    if (!p_dictWrapper.Get<bool>(conditionName))
                    {
                        _condition = false;
                        break;
                    }
                    
                    _condition = true;
                }
            }

            if (_node != null)
                // todo: this was the wrong way around, check the tree, perhaps remake
                return _condition ? NodeStatus.SUCCES : _node.Update();
            
            return _condition ? NodeStatus.SUCCES : NodeStatus.FAILED;
        }
        
        protected override void OnEnter()
        {
            base.OnEnter();
            _node?.SetDictWrapper(p_dictWrapper);
        }
    }
}