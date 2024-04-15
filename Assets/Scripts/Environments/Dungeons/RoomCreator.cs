using UnityEngine;
using System.Collections.Generic;

namespace zombie
{
    public class RoomCreator : MonoBehaviour
    {
        [SerializeField] private bool cameFromNorth;
        [SerializeField] private bool cameFromEast;
        [SerializeField] private bool cameFromSouth;
        [SerializeField] private bool cameFromWest;

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

            // Select a random prefab based on the direction the player came from
            List<GameObject> filteredPrefabs = FilterPrefabs();
            if (filteredPrefabs.Count == 0)
            {
                Debug.LogWarning("No suitable prefabs found based on the direction.");
                return;
            }

            int randomIndex = Random.Range(0, filteredPrefabs.Count);
            GameObject randomPrefab = filteredPrefabs[randomIndex];

            // Instantiate the random prefab as a child of the Dungeon GameObject
            GameObject newRoom = Instantiate(randomPrefab, transform.position, transform.rotation);
            newRoom.transform.parent = dungeonGameObject.transform;

            // Destroy RoomCreators in the new room if they are touching a GameObject with the "Ground" tag
            DestroyRoomCreators(newRoom);

            // Set flag to true indicating that room has been created
            roomCreated = true;

            // Destroy the RoomCreator
            Destroy(gameObject);
        }

        private List<GameObject> FilterPrefabs()
        {
            List<GameObject> filteredPrefabs = new List<GameObject>();

            foreach (GameObject prefab in prefabList)
            {
                string prefabName = prefab.name;

                // Check conditions based on the direction the path came from

                // If coming from North, spawn a random South entrance
                if (cameFromNorth && (prefabName[7] == '0' || prefabName[7] == '2'))
                    filteredPrefabs.Add(prefab);

                // If coming from East, spawn a random West entrance
                else if (cameFromEast && (prefabName[8] == '0' || prefabName[8] == '2'))
                    filteredPrefabs.Add(prefab);

                // If coming from South, spawn a random North entrance
                else if (cameFromSouth && (prefabName[5] == '0' || prefabName[5] == '2'))
                    filteredPrefabs.Add(prefab);

                // If coming from West, spawn a random East entrance
                else if (cameFromWest && (prefabName[6] == '0' || prefabName[6] == '2'))
                    filteredPrefabs.Add(prefab);
            }

            return filteredPrefabs;
        }

        private void DestroyRoomCreators(GameObject room)
        {
            RoomCreator[] roomCreators = room.GetComponentsInChildren<RoomCreator>();

            foreach (RoomCreator creator in roomCreators)
            {
                Collider[] colliders = Physics.OverlapSphere(creator.transform.position, 0.5f);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Ground"))
                    {
                        Debug.Log("gibblygoo");
                        Destroy(creator.gameObject);
                        break;
                    }
                }
            }
        }

        public bool IsRoomCreated()
        {
            return roomCreated;
        }
        
        public void SetRoomCreated(bool flag)
        {
            roomCreated = flag;
        }
    }
}