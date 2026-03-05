namespace Framework.BehaviourTreeSystem
{
    public abstract class Node
    {
        protected DictWrapper p_dictWrapper;
        protected NodeStatus p_status;
     
        private bool _hasEntered;

        public virtual void OnReset() { }

        public NodeStatus Update()
        {
            if (!_hasEntered)
            {
                OnEnter();
                _hasEntered = true;
            }

            p_status = OnUpdate();

            if (p_status == NodeStatus.RUNNING)
                return p_status;
            
            OnExit();
            _hasEntered = false;
            return p_status;
        }

        public virtual void SetDictWrapper(DictWrapper dictWrapper) => p_dictWrapper = dictWrapper;

        public NodeStatus GetStatus() => p_status;

        protected abstract NodeStatus OnUpdate();
        
        protected virtual void OnEnter() { }
        
        protected virtual void OnExit() { }
    }
}