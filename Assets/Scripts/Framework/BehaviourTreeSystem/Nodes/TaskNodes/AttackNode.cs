using Gameplay;

namespace Framework.BehaviourTreeSystem.Nodes.TaskNodes
{
    public sealed class AttackNode : WaitNode
    {
        private Health _healthToDamage;
        private int _damage;
        
        public AttackNode(float time) : base(time)
        {
            p_maxWaitTime = time;
        }

        public AttackNode(Health healthToDamage, int damage, float attackCooldown) : this(attackCooldown)
        {
            _healthToDamage = healthToDamage;
            _damage = damage;
        }

        protected override void OnEnter()
        {
            _healthToDamage.TakeDamage(_damage);
            base.OnEnter();
        }
        
        public override string NodeName() => "AttackNode";
    }
}