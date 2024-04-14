using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace zombie
{
    public class RoomCreator : MonoBehaviour
    {
        private List<GameObject> prefabList;
        private bool roomCreated = false;

        public void Initialize(List<GameObject> prefabs)
        {
            prefabList = prefabs;
        }

        public void CreateInitialRoom()
        {
            if (roomCreated) return; // If room has already been created, exit the method

            List<GameObject> filteredPrefabs = new List<GameObject>();

            // Filter prefabs by the specified criteria
            foreach (GameObject prefab in prefabList)
            {
                string prefabName = prefab.name;
                if (prefabName.Length >= 8 && prefabName.Substring(5, 1) == "2") // Check if third character after 'Room_' is '2'
                {
                    filteredPrefabs.Add(prefab);
                }
            }

            if (filteredPrefabs.Count > 0)
            {
                // Select a random prefab from the filtered list
                int randomIndex = Random.Range(0, filteredPrefabs.Count);
                GameObject randomPrefab = filteredPrefabs[randomIndex];

                // Instantiate the random prefab
                GameObject roomObject = Instantiate(randomPrefab, Vector3.zero, Quaternion.identity);

                // Find the child with the name "3"
                Transform thirdChild = roomObject.transform.GetChild(3);

                // Find the child "Path Node" of the third child and destroy it
                Transform pathNode = thirdChild.Find("Path Node");
                if (pathNode != null)
                {
                    Destroy(pathNode.gameObject);
                }
                else
                {
                    Debug.LogWarning("Path Node not found in the third child of the room prefab.");
                }

                // Set flag to true indicating that room has been created
                roomCreated = true;

                // Destroy the GameObject that holds this script
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("No room with '2' in the specified position of the name found in the prefab list.");
            }
        }
    }
}
