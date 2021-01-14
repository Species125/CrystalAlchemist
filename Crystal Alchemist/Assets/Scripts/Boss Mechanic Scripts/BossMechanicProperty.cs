using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class BossMechanicProperty : MonoBehaviour
    {
        public enum SpawnPositionType
        {
            none,
            spawnPoints,
            randomPoints,
            area,
            target,
            sender,
            custom
        }

        public enum RotationType
        {
            none,
            random,
            identity,
            target,
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

        [HideInInspector]
        public Character sender;
        [HideInInspector]
        public Character target;
        [HideInInspector]
        public int counter;


        public void Initialize(Character sender, Character target)
        {
            this.sender = sender;
            this.target = target;
        }

        public GameObject GetSpawnPosition(SequenceProperty property)
        {
            switch (property.spawnPositonType)
            {
                case SpawnPositionType.sender: return CreateNewSpawnPoint(GetPositionFromCharacter(this.sender));
                case SpawnPositionType.target: return CreateNewSpawnPoint(GetPositionFromCharacter(this.target));
                case SpawnPositionType.area: return CreateNewSpawnPoint(UnityUtil.GetRandomVector(this.GetComponent<Collider2D>()));
                case SpawnPositionType.randomPoints: return GetRandomPositionFromSpawnPoint(property);
                case SpawnPositionType.spawnPoints: return GetPositionFromSpawnPoint(property.spawnPoints);
                case SpawnPositionType.custom: return CreateNewSpawnPoint(property.position);
            }

            return CreateNewSpawnPoint(Vector2.zero);
        }

        private GameObject CreateNewSpawnPoint(Vector2 position)
        {
            GameObject temp = new GameObject("spawnPoint");
            temp.transform.position = position;
            temp.transform.rotation = Quaternion.identity;
            return temp;
        }

        private Vector2 GetPositionFromCharacter(Character character)
        {
            return character.GetGroundPosition();
        }

        private GameObject GetPositionFromSpawnPoint(List<GameObject> spawnPoints)
        {
            return spawnPoints[this.counter];
        }

        private GameObject GetRandomPositionFromSpawnPoint(SequenceProperty property)
        {
            property.AddSpawnPoints(this.transform);
            List<GameObject> spawnPoints = property.spawnPoints;

            int rng = Random.Range(0, spawnPoints.Count);
            return spawnPoints[rng];
        }

        public Quaternion GetRotation(RotationType type, int rotationfactor, float offset)
        {
            return GetRotation(type, rotationfactor, this.transform.gameObject, offset);
        }

        public Quaternion GetRotation(RotationType type, int rotationfactor, GameObject spawnPoint, float offset)
        {
            switch (type)
            {
                case RotationType.random: return SetRandomRotation(rotationfactor);
                case RotationType.target: return SetRotationOnTarget(spawnPoint.transform.position, offset);
                case RotationType.identity: return Quaternion.identity;
                case RotationType.parent: return this.transform.rotation;
            }

            return spawnPoint.transform.rotation;
        }

        private Quaternion SetRandomRotation(int factor)
        {
            int divisor = 360 / factor;
            int rng = Random.Range(0, (divisor + 1));
            float result = factor * rng;

            return Quaternion.Euler(0, 0, result);
        }

        private Quaternion SetRotationOnTarget(Vector2 origin, float offset)
        {
            Vector2 direction = (this.target.GetGroundPosition() - origin).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + offset;

            return Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
