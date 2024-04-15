using UnityEngine;
using System.Collections.Generic;

namespace zombie
{
    public class RoomCreator : MonoBehaviour
    {
        private List<GameObject> prefabList;
        private bool roomCreated = false;
        private GameObject dungeonGameObject;

        public void Initialize(List<GameObject> prefabs, GameObject dungeon)
        {
            prefabList = prefabs;
            dungeonGameObject = dungeon;
        }

        public void CreateRoom()
        {
            if (roomCreated) return; // If room has already been created, exit the method

            if (prefabList.Count == 0)
            {
                Debug.LogWarning("No prefabs available to create a room.");
                return;
            }

            // Select a random prefab from the list
            int randomIndex = Random.Range(0, prefabList.Count);
            GameObject randomPrefab = prefabList[randomIndex];

            // Instantiate the random prefab as a child of the Dungeon GameObject
            GameObject newRoom = Instantiate(randomPrefab, transform.position, transform.rotation);
            newRoom.transform.parent = dungeonGameObject.transform;

            // Set flag to true indicating that room has been created
            roomCreated = true;

            // Destroy the RoomCreator
            Destroy(gameObject);
        }

        public bool IsRoomCreated()
        {
            return roomCreated;
        }
    }
}
