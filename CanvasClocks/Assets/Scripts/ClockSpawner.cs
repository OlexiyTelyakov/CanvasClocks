using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanvasClocks
{
    public class ClockSpawner : MonoBehaviour
    {
        #region Singleton
        //Lazy singleton, without the instantiation part.
        private static ClockSpawner spawner;
        public static ClockSpawner Spawner
        {
            get
            {
                if(spawner == null)
                {
                    spawner = FindObjectOfType<ClockSpawner>();
                }
                return spawner;
            }
        }
        #endregion

        [SerializeField]
        private GameObject clockPrefab;
        [SerializeField]
        private int startingObjectPool = 10;

        private List<GameObject> clockObjectPool = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            //Populate the initial object pool.
            for (int i = 0; i < startingObjectPool; i++)
            {
                GameObject clock = Instantiate(clockPrefab, transform);
                clockObjectPool.Add(clock);
                clock.SetActive(false);
            }
            //Activate the first clock
            AddClock();
        }

        //Methods that will be called by UI buttons.
        #region OnClickedButtonMethods
        public void AddClock()
        {
            //Attempt to find an inactive clock in the object pool.
            GameObject clock = clockObjectPool.Find(x => x.activeSelf == false);
            //If it cannot be done (ie all the clocks have been spawned), create a new clock and add it to the pool.
            if (clock == null)
            {
                clock = Instantiate(clockPrefab, transform);
                clockObjectPool.Add(clock);
            }
            //Activate the clock and set it into a random position on the screen to prevent them stacking on one another.
            clock.SetActive(true);
            clock.transform.localPosition = new Vector3(Random.Range(-300, 200), Random.Range(200, -300), 0);
        }

        public static void OnClockRemoved()
        {
            //Check if all clocks in the object pool are deactivated and if so, deactivate this object.
            if (Spawner.clockObjectPool.TrueForAll(x => x.activeSelf == false))
            {
                Spawner.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}