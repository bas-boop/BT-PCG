using Framework.BehaviourTreeSystem;
using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class NodeDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        private string _nodeName;

        private void Update()
        {
            text.text = _nodeName;
        }

        public void SetNode(string targetNode)
        {
            _nodeName = targetNode;
        }
    }
}