using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zombie
{
    public class DungeonManager : MonoBehaviour
    {
        // Every dungeon will begin with a room that has a door to the south

        // Every door/space will have an empty gameobject located at the orgin of the next room
        // These empty gameobjects will have a CreateRoom script attached to them

        // These gameobjects will check which direction the door/space created it from
        // Spawn any room with a door/space from this direction
        // This should cause a ripple effect

        // Every room will have a Room script which contains details about the current room
        // For example, a 'hasPaths' variable
        // This will check if there is more than door/space
        
        // If too many rooms are being created
        // Only spawn rooms where hasPaths = false

        // For now focus on the ground floor
        // Will add floors in the future
    }
}

