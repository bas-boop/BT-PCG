namespace Framework.BehaviourTreeSystem
{
    public abstract class Node
    {
        protected DictWrapper p_dictWrapper;
     
        private bool _hasEntered;

        public virtual void OnReset() { }

        public NodeStatus Update()
        {
            if (!_hasEntered)
            {
                OnEnter();
                _hasEntered = true;
            }

            NodeStatus result = OnUpdate();

            if (result == NodeStatus.RUNNING)
                return result;
            
            OnExit();
            _hasEntered = false;
            return result;
        }

        public virtual void SetDictWrapper(DictWrapper dictWrapper) => p_dictWrapper = dictWrapper;

        protected abstract NodeStatus OnUpdate();
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
    }
}