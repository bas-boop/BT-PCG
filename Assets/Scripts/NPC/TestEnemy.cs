using UnityEngine;

using Framework.BehaviourTreeSystem;
using Framework.BehaviourTreeSystem.Nodes;
using Framework.BehaviourTreeSystem.Nodes.TaskNodes;

namespace NPC
{
    public sealed class TestEnemy : MonoBehaviour
    {
        [SerializeField] private float a;
        private Node _tree;
        
        private void Start()
        {
            DictWrapper dictWrapper = new();
            dictWrapper.Set("A", a);

            _tree = new SequenceNode(
                new LogNode("1"),
                new ParallelNode(
                    new TestNode2("2"),
                    new LogNode("3"),
                    new ConditionalNode(
                        false,
                        new LogNode("condition")
                    ),
                    new SequenceNode(
                        new YesNode(),
                new NoNode(),
                        new WaitNode(2),
                        new LogNode("wait done")
                    ),
                    new WaitNode(1),
                    new TestNode2("4"),
                    new SelectorNode(
                        new TestNode2("a"),
                        new LogNode("b"),
                        new TestNode2("c")
                    )
                ),
                new InvertNode(new LogNode("5")),
                new LogNode("6")
            );
            
            _tree.SetDictWrapper(dictWrapper);
        }

        private void Update()
        {
            _tree.Update();
        }
    }
}