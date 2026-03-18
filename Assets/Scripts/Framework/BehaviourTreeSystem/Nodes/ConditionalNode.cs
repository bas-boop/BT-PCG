using UnityEngine;

namespace Framework.BehaviourTreeSystem.Nodes
{
    public class ConditionalNode : Node
    {
        private bool _condition;
        private bool _inverted;
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
        
        public ConditionalNode(string conditionName, Node node, bool inverted)
        {
            _conditionName = conditionName;
            _node = node;
            _inverted = inverted;
        }
        
        public ConditionalNode(Node node, params string[] conditionNameName)
        {
            _conditionNames = conditionNameName;
            _node = node;
        }

        public ConditionalNode(string conditionName)
        {
            _conditionName = conditionName;
        }
        
        public ConditionalNode(params string[] conditionNameName)
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
            {
                if (_inverted)
                    return _condition ? _node.Update() : NodeStatus.SUCCES;
                
                return _condition ? NodeStatus.SUCCES : _node.Update();
            }
            
            if (_inverted)
                return _condition ? NodeStatus.FAILED : NodeStatus.SUCCES;
            
            return _condition ? NodeStatus.SUCCES : NodeStatus.FAILED;
        }
        
        protected override void OnEnter()
        {
            base.OnEnter();
            _node?.SetDictWrapper(p_dictWrapper);
        }
    }
}