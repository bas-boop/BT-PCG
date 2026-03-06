using UnityEngine;

using Framework.BehaviourTreeSystem;
using Framework.BehaviourTreeSystem.Nodes;

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
                new ParallelNode(
                    new TestNode2("2"),
                    new TestNode("3"),
                    new ConditionalNode(
                        false,
                        new TestNode("condition")
                    ),
                    new SequenceNode(
                        new WaitNode(2),
                        new TestNode("wait done")
                    ),
                    new WaitNode(1),
                    new TestNode2("4"),
                    new SelectorNode(
                        new TestNode2("a"),
                        new TestNode("b"),
                        new TestNode2("c")
                    )
                ),
                new InvertNode(new TestNode("5")),
                new TestNode("6")
            );
            
            _tree.SetDictWrapper(dictWrapper);
        }

        private void Update()
        {
            _tree.Update();
        }
    }
}