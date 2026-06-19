# BT-PCG

HKU assigments for behaviour trees and procedural content generation.

# BT
```mermaid
classDiagram
    class DictWrapper {
        -Dictionary~string,object~ _data
        +Get~T~(key) T
        +Set~T~(key, value)
        +Log()
    }

    class Node {
        <<abstract>>
        #DictWrapper p_dictWrapper
        #NodeStatus p_status
        -bool _hasEntered
        +Update() NodeStatus
        +SetDictWrapper(DictWrapper)
        +GetStatus() NodeStatus
        #OnUpdate()* NodeStatus
        #OnEnter()
        #OnExit()
        +NodeName() string
    }

    class NodeStatus {
        <<enumeration>>
        RUNNING
        FAILED
        SUCCES
    }

    class SelectorNode {
        -Node[] _nodes
        -int _i
        #OnUpdate() NodeStatus
        #OnEnter()
        +NodeName() string
    }

    class SequenceNode {
        -Node[] _nodes
        -int _i
        #OnUpdate() NodeStatus
        #OnEnter()
        +NodeName() string
    }

    class ParallelNode {
        -Node[] _nodes
        #OnUpdate() NodeStatus
        #OnEnter()
        +NodeName() string
    }

    class ConditionalNode {
        -bool _condition
        -bool _inverted
        -string _conditionName
        -string[] _conditionNames
        -Node _node
        +UpdateCondition(bool)
        #OnUpdate() NodeStatus
        #OnEnter()
        +NodeName() string
    }

    class InvertNode {
        <<sealed>>
        -Node _nodeToInvert
        -NodeStatus _otherNodeStatus
        #OnUpdate() NodeStatus
        #OnEnter()
        +NodeName() string
    }

    class WaitNode {
        #float p_maxWaitTime
        #float p_currentWaitTime
        #OnEnter()
        #OnUpdate() NodeStatus
        +NodeName() string
    }

    class MoveNode {
        -GameObject _objectToMove
        -Vector2 _targetPosition
        -string _targetPositionName
        -float _speed
        +SetTargetPosition(Vector2)
        #OnUpdate() NodeStatus
        +NodeName() string
    }

    class AttackNode {
        <<sealed>>
        -Health _healthToDamage
        -int _damage
        #OnEnter()
        +NodeName() string
    }

    class GuardEnemy {
        -GameObject player
        -Weapon weapon
        -GameObject[] waypoints
        -float playerSeeRange
        -float speed
        -float attackRange
        -float attackCooldown
        -int damage
        -NodeDisplay nodeDisplay
        -Node _tree
        -Node _patrolTree
        -Node _attackTree
        -DictWrapper _dictWrapper
        -Start()
        -Update()
        -SetupTree()
        -FindWeapon()
        -PickUp()
        -Search()
        -PlayerDeath()
    }

    Node <|-- SelectorNode
    Node <|-- SequenceNode
    Node <|-- ParallelNode
    Node <|-- ConditionalNode
    Node <|-- InvertNode
    Node <|-- WaitNode
    Node <|-- MoveNode
    WaitNode <|-- AttackNode

    Node --> NodeStatus
    Node --> DictWrapper

    SelectorNode o-- Node
    SequenceNode o-- Node
    ParallelNode o-- Node
    ConditionalNode o-- Node

    GuardEnemy --> Node
    GuardEnemy --> DictWrapper
```

```mermaid
flowchart TD
    A([Start]) --> B[Patrol]
    B --> C{Can see player?}
    C -->|No| B
    C -->|Yes| D{Has weapon?}
    D -->|No| E[Screach and pick-up weapon]
    E --> C
    D -->|Yes| F[Attack]
    F --> G{In attack range?}
    G -->|No| H[Move to player]
    H --> G
    G -->|Yes| I[Attack player]
    I --> J{Player out of sight or dead}
    J -->|No| F
    J -->|Yes| K[Repeat form start]
    K --> B
```

# PCG
```mermaid
classDiagram
    namespace DungeonGeneratorSystem {
        class RandomSeedSystem {
            <<static>>
            -int _seed
            -Random _random
            +SetSeed(string seed)
            +GetRandomDouble() double
            +GetRandomInt() int
            +GetRandomInt(int min, int max) int
            +GetRandomFloat() float
            +GetRandom() Random
        }
        class DiceRoller {
            <<static>>
            -Random _random
            +Roll(int count, int sides) int
        }
        class CardinalDirections {
            <<enumeration>>
            NORTH
            EAST
            SOUTH
            WEST
        }
        class CardinalHelper {
            <<static>>
            +ToDoor(CardinalDirections dir) Doors
            +ToOppositeDoor(CardinalDirections dir) Doors
        }
        class Doors {
            <<enumeration>>
            <<Flags>>
            NORTH
            EAST
            SOUTH
            WEST
        }
        class CellType {
            <<enumeration>>
            EMPTY
            NORMAL
            START
            END
            NEGATIVE
            POSTIVE
            SECRET
            BOSS
        }
        class Cell {
            <<struct>>
            +CellType Type
            +Doors Doors
            +SpriteRenderer Renderer
            +Dictionary~Doors,GameObject~ DoorsObject
            +Cell(CellType type, Doors doors, SpriteRenderer renderer)
        }
        class Grid {
            -Dictionary~Vector2Int,Cell~ _cells
            -Vector2Int _size
            +Vector2Int Size
            +Grid(Vector2Int size)
            +AllCells IEnumerable
            +ActiveCells IEnumerable
            +Contains(Vector2Int pos) bool
            +InBounds(Vector2Int pos) bool
            +Get(Vector2Int pos) Cell
            +TryGet(Vector2Int pos, out Cell cell) bool
            +Set(Vector2Int pos, Cell cell)
            +Clear()
            +AddDoor(Vector2Int pos, Doors door)
            +SetCellType(Vector2Int pos, CellType type)
            +SetCellType(Vector2Int pos, CellType type, Doors doors)
            +GetDoorCount(Doors doors) int$
            -ShowCellDoors(Cell targetCell)$
        }
        class Walker {
            -Grid _grid
            -int _stepAmount
            +Vector2Int StartPos
            +Vector2Int EndPos
            +Walker(Grid grid, int stepAmount)
            +Walk(Vector2Int startPos)
            -Step(Vector2Int currentPos) Vector2Int
            -FixEndPosition()
        }
        class Generator {
            <<MonoBehaviour>>
            -SpriteRenderer cell
            -Vector2Int size
            -int stepAmount
            -GenerationRule[] generationRules
            -GenerationRule[] funkyGenerationRules
            -Grid _grid
            -GenerationRule[] _rules
            -Vector2Int _startPos
            -Vector2Int _endPos
            +Generate()
            +ToggleFunkyMode()
            -BuildGrid() Grid
            -ColorGrid()
            -ApplyGenerationRules()
        }
        class TestSeed {
            <<MonoBehaviour>>
            -string seed
            -SpriteRenderer[] sprites
            -int _times
            +SetRandomColors()
            -Walk()
        }
    }
    namespace Rules {
        class GenerationRule {
            <<ScriptableObject>>
            +int amount
            +CellType roomType
            +TryPlace(Grid grid) bool
        }
        class DeadEndGenerationRule {
            +TryPlace(Grid grid) bool
        }
        class DistanceGenerationRule {
            +CellType otherRoomType
            +Distances distance
            +TryPlace(Grid grid) bool
        }
        class DoorCountGenerationRule {
            +int minDoors
            +int maxDoors
            +TryPlace(Grid grid) bool
            +TryPlaceByDoorCount(Grid grid, CellType roomType, int minDoors, int maxDoors) bool$
        }
        class ScatterGenerationRule {
            +int scatterCount
            +TryPlace(Grid grid) bool
        }
        class TeleportSwapGenerationRule {
            +CellType otherRoomType
            +TryPlace(Grid grid) bool
        }
        class WildcardGenerationRule {
            +CellType[] possibleTypes
            +TryPlace(Grid grid) bool
        }
        class Distances {
            <<enumeration>>
            CLOSE
            FAR
        }
    }
    namespace Extensions {
        class CollectionExtensions {
            <<static>>
            +GetRandomItem~T~(IList~T~ list, Random r) T
            +GetRandomItem~T~(ICollection~T~ collection) T
        }
        class EnumExtensions {
            <<static>>
            +GetStringValue(Enum value) string
            +GetCharValue(Enum value) char
            +GetVector2(Enum value) Vector2
            +GetVector2Int(Enum value) Vector2Int
            +GetVector3(Enum value) Vector3
            +GetRandomEnumValue~T~(Random r) T
        }
    }
    namespace UI {
        class SeedUI {
            <<MonoBehaviour>>
            -TMP_InputField inputField
            -string _seed
            +SetCurrentSeed(string seed)
            +SetCurrentSeed()
            +SetRandomSeed()
        }
    }

    GenerationRule <|-- DeadEndGenerationRule
    GenerationRule <|-- DistanceGenerationRule
    GenerationRule <|-- DoorCountGenerationRule
    GenerationRule <|-- ScatterGenerationRule
    GenerationRule <|-- TeleportSwapGenerationRule
    GenerationRule <|-- WildcardGenerationRule
    DeadEndGenerationRule ..> DoorCountGenerationRule : delegates to
    DistanceGenerationRule ..> Distances : uses
    GenerationRule ..> CollectionExtensions : uses
    GenerationRule ..> RandomSeedSystem : uses

    Generator "1" *-- "1" Grid : builds
    Generator "1" *-- "1" Walker : carves with
    Generator "1" o-- "*" GenerationRule : applies
    Walker ..> Grid : modifies
    Walker ..> CardinalDirections : steps with
    Walker ..> CardinalHelper : uses
    Walker ..> CollectionExtensions : uses
    Walker ..> RandomSeedSystem : uses
    Grid "1" o-- "*" Cell : contains
    Grid ..> CellType : uses
    Cell ..> Doors : has
    Cell ..> CellType : has
    CardinalHelper ..> CardinalDirections : extends
    CardinalHelper ..> Doors : produces

    TestSeed ..> RandomSeedSystem : uses
    TestSeed ..> EnumExtensions : uses
    SeedUI ..> RandomSeedSystem : uses
    SeedUI ..> DiceRoller : uses
    EnumExtensions ..> CardinalDirections : reflects on
```
