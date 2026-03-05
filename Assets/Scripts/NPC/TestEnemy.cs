using UnityEngine;

using Framework.BehaviourTreeSystem;

namespace NPC
{
    public sealed class TestEnemy : MonoBehaviour
    {
        private Node _tree;
        
        private void Start()
        {
            DictWrapper dictWrapper = new();
            //float a = 1;
            //dictWrapper.Set("", a);

            _tree = new SequenceNode(
                new TestNode("1"),
                new TestNode("2"),
                new TestNode("3")
            );
            
            _tree.SetDictWrapper(dictWrapper);
        }

        private void Update()
        {
            _tree.Update();
        }
    }
}