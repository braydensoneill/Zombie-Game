using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace zombie
{
    public class DungeonManager : MonoBehaviour
    {
        [Header("Rooms")]
        [SerializeField] private int currentRoomCount;
        [SerializeField] private int requiredRoomCount;

        [Header("Room Prefabs")]
        [SerializeField] private string folderPath = "Assets/Components/Environments/Rooms/";
        [SerializeField] private GameObject dungeonGameObject;
        [SerializeField] private GameObject Room_1111_Prefab;

        private List<GameObject> prefabList = new List<GameObject>();

        private void Awake()
        {
            currentRoomCount = 0;
            requiredRoomCount = Random.Range(1, 20);
            LoadPrefabsFromFolders();
            InitializeRoomCreators();
            CreateInitialRoom();
            InvokeRepeating("CheckForNewRoomCreators", 0.15f, 0.15f); // Invoke method with delay and repeat every 0.5 seconds
        }

        private void CreateInitialRoom()
        {
            List<GameObject> filteredPrefabs = FilterPrefabsForInitialRoom();
            if (filteredPrefabs.Count == 0)
            {
                Debug.LogWarning("No room with '3' in the specified position of the name found in the prefab list.");
                return;
            }

            int randomIndex = Random.Range(0, filteredPrefabs.Count);
            GameObject randomPrefab = filteredPrefabs[randomIndex];

            GameObject newRoom = InstantiateRoom(randomPrefab);
            if (newRoom != null)
            {
                currentRoomCount++;
            }
        }

        private void CheckForNewRoomCreators()
        {
            RoomCreator[] newRoomCreators = FindObjectsOfType<RoomCreator>();
            foreach (RoomCreator newRoomCreator in newRoomCreators)
            {
                // Check if the room creator has already created a room
                if (!newRoomCreator.IsRoomCreated())
                {
                    // Check if the room creator is touching a plane game object
                    Collider[] colliders = Physics.OverlapSphere(newRoomCreator.transform.position, 0.5f);
                    foreach (Collider collider in colliders)
                    {
                        if (collider.gameObject.CompareTag("Ground"))
                        {
                            // If touching a plane, destroy the room creator and return
                            Destroy(newRoomCreator.gameObject);
                            return;
                        }
                    }

                    // If not touching a plane, proceed to create the room
                    newRoomCreator.Initialize(prefabList, dungeonGameObject);
                    newRoomCreator.CreateRoom();
                    currentRoomCount++;

                    // If the required room count is reached, exit the method
                    if (currentRoomCount >= requiredRoomCount)
                    {
                        return;
                    }
                }
            }

            // If the required room count is not reached and there are remaining room creators,
            // continue creating rooms until the required count is reached or until a room creator
            // touching a plane is encountered
            if (currentRoomCount < requiredRoomCount)
            {
                foreach (RoomCreator newRoomCreator in newRoomCreators)
                {
                    // Check if the room creator has already created a room
                    if (!newRoomCreator.IsRoomCreated())
                    {
                        // Check if the room creator is touching a plane game object
                        Collider[] colliders = Physics.OverlapSphere(newRoomCreator.transform.position, 0.5f);
                        foreach (Collider collider in colliders)
                        {
                            if (collider.gameObject.CompareTag("Ground"))
                            {
                                // If touching a plane, destroy the room creator and break out of the loop
                                Destroy(newRoomCreator.gameObject);
                                break;
                            }
                        }

                        // If not touching a plane, proceed to create the room
                        if (!newRoomCreator.IsRoomCreated())
                        {
                            // Check if the room count allows for creating another room
                            if (currentRoomCount < requiredRoomCount)
                            {
                                // Create Room_1111 prefab
                                GameObject newRoom = InstantiateRoom(Room_1111_Prefab);
                                if (newRoom != null)
                                {
                                    currentRoomCount++;
                                }
                            }
                        }

                        // If the required room count is reached, exit the method
                        if (currentRoomCount >= requiredRoomCount)
                        {
                            return;
                        }
                    }
                }
            }

            // Loop through children under the "dungeon" gameobject and check if any room needs to be created
            foreach (Transform child in dungeonGameObject.transform)
            {
                RoomCreator roomCreator = child.GetComponent<RoomCreator>();
                if (roomCreator != null && !roomCreator.IsRoomCreated())
                {
                    // If a room creator is found and it hasn't created a room yet, create the room
                    roomCreator.Initialize(prefabList, dungeonGameObject);
                    roomCreator.CreateRoom();
                    currentRoomCount++;

                    // If the required room count is reached, exit the method
                    if (currentRoomCount >= requiredRoomCount)
                    {
                        return;
                    }
                }
            }
        }

        private GameObject InstantiateRoom(GameObject prefab)
        {
            GameObject newRoom = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newRoom.transform.parent = dungeonGameObject.transform;
            return newRoom;
        }

        private List<GameObject> FilterPrefabsForInitialRoom()
        {
            List<GameObject> filteredPrefabs = new List<GameObject>();
            foreach (GameObject prefab in prefabList)
            {
                string prefabName = prefab.name;
                if (prefabName.Length >= 8 && prefabName.Substring(5, 1) == "2")
                {
                    filteredPrefabs.Add(prefab);
                }
            }
            return filteredPrefabs;
        }

        private void LoadPrefabsFromFolders()
        {
            for (int i = 0; i < 6; i++)
            {
                string folderName = "H_Paths_" + i;
                string fullPath = Path.Combine(folderPath, folderName);

                if (Directory.Exists(fullPath))
                {
                    string[] prefabPaths = Directory.GetFiles(fullPath, "*.prefab");
                    foreach (string prefabPath in prefabPaths)
                    {
                        GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                        if (prefab != null)
                        {
                            prefabList.Add(prefab);
                        }
                    }
                }
            }
        }

        private void InitializeRoomCreators()
        {
            RoomCreator[] foundRoomCreators = FindObjectsOfType<RoomCreator>();
            foreach (RoomCreator roomCreator in foundRoomCreators)
            {
                roomCreator.Initialize(prefabList, dungeonGameObject);
                roomCreator.CreateRoom();
            }
        }

        public int RequiredRoomCount { get { return requiredRoomCount; } }
    }
}
