// ReSharper disable CoVariantArrayConversion

using System.Collections.Generic;
using UnityEngine;

using Framework.BehaviourTreeSystem;
using Framework.BehaviourTreeSystem.Nodes;
using Framework.BehaviourTreeSystem.Nodes.TaskNodes;
using Framework.Extensions;
using Gameplay;
using UI;
using CollectionExtensions = Framework.Extensions.CollectionExtensions;

namespace NPC
{
    public sealed class GuardEnemy : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject player;
        [SerializeField] private Weapon weapon;
        [SerializeField] private GameObject[] waypoints;

        [Header("Stats")]
        [SerializeField] private float playerSeeRange = 5;
        [SerializeField] private float speed = 1;
        [SerializeField] private float attackRange = 1;
        [SerializeField] private float attackCooldown = 1;
        [SerializeField] private int damage = 1;
        
        [Header("Debug")]
        [SerializeField] private NodeDisplay nodeDisplay;
        
        private Node _tree;
        private Node _patrolTree;
        private Node _attackTree;

        private readonly DictWrapper _dictWrapper = new ();

        private void Start()
        {
            _dictWrapper.Set("playerPosition", Vector2.zero);
            _dictWrapper.Set("weaponPosition", Vector2.zero);
            _dictWrapper.Set("isPlayerDead", false);
            _dictWrapper.Set("canSeePlayer", false);
            _dictWrapper.Set("hasWeapon", false);
            _dictWrapper.Set("isInRange", false);
            
            SetupTree();
            
            _tree.SetDictWrapper(_dictWrapper);
        }

        private void Update()
        {
            if (!_dictWrapper.Get<bool>("isPlayerDead"))
            {
                _dictWrapper.Set("playerPosition", (Vector2) player.transform.position);
                _dictWrapper.Set("canSeePlayer", player.transform.position.Compare(transform.position, playerSeeRange));
                _dictWrapper.Set("isInRange", player.transform.position.Compare(transform.position, attackRange));
            }
            
            _tree.Update();
            
            nodeDisplay.SetNode(_tree.NodeName()); // name here
        }

        private void SetupTree()
        {
            #region attackTree

            _attackTree = new SequenceNode(
                new ConditionalNode(new SequenceNode(
                        new ConditionalNode("canSeePlayer"),
                        new InvertNode(new ConditionalNode("hasWeapon")),
                        new WaitNode(1),
                        new FunctionNode(FindWeapon),
                        new MoveNode(gameObject, "weaponPosition", speed),
                        new FunctionNode(PickUp),
                        new FunctionNode(Search)
                    ),
                    false,
                    "canSeePlayer", "hasWeapon"
                ),
                new MoveNode(gameObject, "playerPosition", speed),
                new ConditionalNode(
                    new SequenceNode(
                        new AttackNode(player.GetComponent<Health>(), damage, attackCooldown),
                        new FunctionNode(PlayerDeath)
                    )
                )
            );

            #endregion

            #region patrolTree

            List<MoveNode> positions = new ();
            
            foreach (GameObject waypoint in waypoints)
                positions.Add(new (gameObject, waypoint.transform.position, speed));
            
            _patrolTree = new SequenceNode(positions.ToArray());

            #endregion
            
            _tree = new SelectorNode(
                _attackTree,
                _patrolTree
            );
        }
        
        private void FindWeapon()
        {
            if (weapon)
                return;
            
            while (true)
            {
                Weapon[] w = FindObjectsByType<Weapon>(FindObjectsSortMode.None);
                weapon = CollectionExtensions.GetRandomItem(w);
                
                if (weapon.IsInUse)
                    continue;
                
                break;
            }
            
            _dictWrapper.Set("weaponPosition", (Vector2) weapon.transform.position);
        }

        private void PickUp() => weapon.transform.SetParent(transform);

        private void Search() => _dictWrapper.Set("hasWeapon", true);

        private void PlayerDeath()
        {
            if (player.activeSelf)
                return;
            
            _dictWrapper.Set("canSeePlayer", false);
            _dictWrapper.Set("isPlayerDead", true);
        }
    }
}