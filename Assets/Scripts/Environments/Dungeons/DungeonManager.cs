using UnityEngine;
using System.Collections.Generic;
using System.IO;

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

        [Header("Rooms")]
        [SerializeField] private int currentRoomCount;
        [SerializeField] private int maxRoomCount;

        [Header("Room Prefabs")]
        [SerializeField] private string folderPath = "Assets/Components/Environments/Rooms/";

        [SerializeField] private List<GameObject> prefabList = new List<GameObject>();

        [SerializeField] private RoomCreator roomCreator;

        private void Awake()
        {
            currentRoomCount = 0;
            maxRoomCount = Random.Range(1, 20);
        }

        void Start()
        {
            LoadPrefabsFromFolders();
            roomCreator.Initialize(prefabList);
            roomCreator.CreateInitialRoom();
        }

        private void LoadPrefabsFromFolders()
        {
            for (int i = 0; i < 6; i++) // Loop through folders H_Paths_0 to H_Paths_5
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
