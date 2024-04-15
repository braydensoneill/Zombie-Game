using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace zombie
{
    public class DungeonManager : MonoBehaviour
    {
        [Header("Rooms")]
        [SerializeField] private int currentRoomCount;
        [SerializeField] private int maxRoomCount; // Renamed from requiredRoomCount

        [Header("Room Prefabs")]
        [SerializeField] private string folderPath = "Assets/Components/Environments/Rooms/";
        [SerializeField] private GameObject dungeonGameObject;
        [SerializeField] private GameObject Room_1111_Prefab;

        private List<GameObject> prefabList = new List<GameObject>();

        private void Awake()
        {
            currentRoomCount = 0;
            maxRoomCount = Random.Range(1, 20); // Renamed from requiredRoomCount
            LoadPrefabsFromFolders();
            InitializeRoomCreators();
            InvokeRepeating("CheckForNewRoomCreators", 0.15f, 0.15f); // Invoke method with delay and repeat every 0.5 seconds
        }

        private void CheckForNewRoomCreators()
        {
            // Stop generating rooms if currentRoomCount reaches maxRoomCount
            if (currentRoomCount >= maxRoomCount)
            {
                return;
            }

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

                    // If not touching a plane and still have to generate more rooms
                    if (currentRoomCount < maxRoomCount)
                    {
                        // Proceed to create the room
                        newRoomCreator.Initialize(prefabList, dungeonGameObject);
                        newRoomCreator.CreateRoom();
                        currentRoomCount++;
                    }

                    // If the required room count is reached, exit the method
                    if (currentRoomCount >= maxRoomCount)
                    {
                        return;
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
    }
}
