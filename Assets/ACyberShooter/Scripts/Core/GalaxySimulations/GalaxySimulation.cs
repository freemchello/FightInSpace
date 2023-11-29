using UnityEngine;


namespace Core.Galaxy
{

    public class GalaxySimulation : MonoBehaviour
    {

        [SerializeField] private Galaxy _galaxy;

        private void Awake()
        {

            _galaxy.InitGalaxy();
        }

        private void Update()
        {

            _galaxy.UpdateGalaxy(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _galaxy.Dispose();
        }
    }
}