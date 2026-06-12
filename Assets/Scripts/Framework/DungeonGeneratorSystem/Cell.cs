using System.Collections.Generic;
using UnityEngine;

namespace Framework.DungeonGeneratorSystem
{
    public struct Cell
    {
        public CellType Type;
        public Doors Doors;
        public SpriteRenderer Renderer;
        public Dictionary<Doors, GameObject> DoorsObject;
        
        public Cell(CellType type, Doors doors, SpriteRenderer renderer)
        {
            Type = type;
            Doors = doors;
            Renderer = renderer;

            DoorsObject = new ()
            {
                { Doors.NORTH, renderer.transform.Find("DoorNorth").gameObject },
                { Doors.EAST,  renderer.transform.Find("DoorEast").gameObject  },
                { Doors.SOUTH, renderer.transform.Find("DoorSouth").gameObject },
                { Doors.WEST,  renderer.transform.Find("DoorWest").gameObject  },
            };
        }
    }
}