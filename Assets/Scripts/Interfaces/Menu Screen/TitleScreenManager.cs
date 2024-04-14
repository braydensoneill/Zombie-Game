using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Zombie
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.GetInstance().LoadNewGame());
        }
    }
}

