using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum SpawnPositionType
    {
        none,
        spawnPoints,
        randomPoints,
        area,
        mainTarget,
        allTargets,
        noMainTarget,
        sender,
        custom
    }

    public enum RotationType
    {
        none,
        random,
        identity,
        mainTarget,
        parent
    }

    public enum RotationDirection
    {
        none,
        down,
        left
    }

    [System.Serializable]
    public class SequenceProperty
    {
        public SpawnPositionType spawnPositonType = SpawnPositionType.none;

        [ShowIf("spawnPositonType", SpawnPositionType.custom)]
        public Vector2 position;


        [ShowIf("spawnPositonType", SpawnPositionType.randomPoints)]
        public bool uniqueSpawn = true;

        [HideInInspector]
        public List<GameObject> spawnPoints = new List<GameObject>();

        public RotationType rotationType = RotationType.none;

        [ShowIf("rotationType", RotationType.random)]
        [MaxValue(360)]
        [MinValue(0)]
        public int rotationFactor;

        [HideIf("rotationType", RotationType.none)]
        [HideIf("rotationType", RotationType.random)]
        [HideIf("rotationType", RotationType.identity)]
        public RotationDirection direction;

        public void AddSpawnPoints(Transform transform)
        {
            this.spawnPoints.Clear();
            foreach (Transform t in transform) this.spawnPoints.Add(t.gameObject);
        }

        public virtual int GetMax()
        {
            if (this.spawnPositonType == SpawnPositionType.spawnPoints) return this.spawnPoints.Count;
            else return 0;
        }

        public float GetOffset()
        {
            if (this.direction == RotationDirection.down) return 90f;
            else if (this.direction == RotationDirection.left) return 0f;
            return 0f;
        }

        public bool GetDelete()
        {
            if (this.spawnPositonType == SpawnPositionType.randomPoints) return this.uniqueSpawn;
            return true;
        }
    }
}
