using UnityEngine;

namespace MyUtils
{
    public class GameSystem : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
