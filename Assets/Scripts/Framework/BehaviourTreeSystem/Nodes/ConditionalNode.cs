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
        
        public ConditionalNode(bool condition, Node node, bool inverted = false)
        {
            _condition = condition;
            _node = node;
            _inverted = inverted;
        }
        
        public ConditionalNode(string conditionName, Node node, bool inverted = false)
        {
            _conditionName = conditionName;
            _node = node;
            _inverted = inverted;
        }
        
        public ConditionalNode(Node node, bool inverted = false, params string[] conditionNameName)
        {
            _node = node;
            _inverted = inverted;
            _conditionNames = conditionNameName;
        }

        public ConditionalNode(string conditionName, bool inverted = false)
        {
            _conditionName = conditionName;
            _inverted = inverted;
        }
        
        public ConditionalNode(bool inverted = false, params string[] conditionNameName)
        {
            _inverted = inverted;
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