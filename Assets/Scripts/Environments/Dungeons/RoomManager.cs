using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zombie
{
    public class RoomManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DestroyRandomDoorWalls();
        }

        private void DestroyRandomDoorWalls()
        {
            // Get the transform component of the parent GameObject
            Transform parentTransform = gameObject.transform;

            // Loop through each child of the parent GameObject
            foreach (Transform childTransform in parentTransform)
            {
                // Check if the child's name is equal to "2"
                if (childTransform.name == "2")
                {
                    // Generate a random number between 0 and 1
                    int randomNumber = Random.Range(0, 2);

                    // If the random number is 0, destroy the first child, otherwise destroy the second child
                    if (randomNumber == 0)
                    {
                        Destroy(childTransform.GetChild(0).gameObject); // Destroy the first child
                    }
                    else
                    {
                        Destroy(childTransform.GetChild(1).gameObject); // Destroy the second child
                    }

                    // We have destroyed one child, so break out of the loop
                    break;
                }
            }
        }
    }
}
