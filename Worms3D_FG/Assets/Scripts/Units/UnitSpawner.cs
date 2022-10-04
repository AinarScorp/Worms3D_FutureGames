using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WormsGame.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        public static int BatUnitCount = -1;
        public static int SlimeUnitCount = -1;
        public static int RabbitUnitCount = -1;
        public static int GhostUnitCount = -1;

        [SerializeField] Transform _spawnPointsContainer;
        [SerializeField] Unit _batPrefab, _slimePrefab, _rabbitPrefab, _ghostPrefab;

        List<SpawnPoint> _allSpawnPoints = new List<SpawnPoint>();
        List<SpawnPoint> _openSpawnPoints = new List<SpawnPoint>();


        void Start()
        {
            foreach (Transform spawnTransform in _spawnPointsContainer)
            {
                if (spawnTransform.TryGetComponent(out SpawnPoint spawnPoint))
                    _allSpawnPoints.Add(spawnPoint);
            }

            _openSpawnPoints.AddRange(_allSpawnPoints);

            SpawnUnits(BatUnitCount, _batPrefab);
            SpawnUnits(RabbitUnitCount, _rabbitPrefab);
            SpawnUnits(SlimeUnitCount, _slimePrefab);
            SpawnUnits(GhostUnitCount, _ghostPrefab);

            FindObjectOfType<TeamsHandler>().FindAllUnits();
            
        }


        void SpawnUnits(int unitCount, Unit unitPrefab)
        {
            for (int i = 0; i < unitCount; i++)
            {
                int rnd = Random.Range(0, _openSpawnPoints.Count);
                if (_openSpawnPoints.Count <= 0)
                    _openSpawnPoints.AddRange(_allSpawnPoints);

                Instantiate(unitPrefab, _openSpawnPoints[rnd].UseSpawnPoint(), Quaternion.identity, this.transform);
                _openSpawnPoints.RemoveAt(rnd);
            }
        }

        void OnDisable()
        {
            BatUnitCount = -1;
            SlimeUnitCount = -1;
            RabbitUnitCount = -1;
            GhostUnitCount = -1;
        }
    }
}
