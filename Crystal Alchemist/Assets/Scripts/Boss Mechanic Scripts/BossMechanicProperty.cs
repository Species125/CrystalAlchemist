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

        [HideInInspector]
        public Character sender;
        [HideInInspector]
        public Character target;
        [HideInInspector]
        public List<Character> targets = new List<Character>();
        [HideInInspector]
        public int counter;


        public void Initialize(Character sender, Character target, List<Character> targets)
        {
            this.sender = sender;
            this.target = target;
            this.targets = targets;
        }

        public List<GameObject> GetSpawnPosition(SequenceProperty property)
        {
            switch (property.spawnPositonType)
            {
                case SpawnPositionType.sender: return CreateNewSpawnPoint(GetPositionFromCharacter(this.sender));
                case SpawnPositionType.mainTarget: return CreateNewSpawnPoint(GetPositionFromCharacter(this.target));
                case SpawnPositionType.allTargets: return CreateNewSpawnPoint(GetPositionFromCharacters(this.targets, SpawnPositionType.allTargets));
                case SpawnPositionType.noMainTarget: return CreateNewSpawnPoint(GetPositionFromCharacters(this.targets, SpawnPositionType.noMainTarget));
                case SpawnPositionType.area: return CreateNewSpawnPoint(UnityUtil.GetRandomVector(this.GetComponent<Collider2D>()));
                case SpawnPositionType.randomPoints: return CreateNewSpawnPoints(GetRandomPositionFromSpawnPoint(property));
                case SpawnPositionType.spawnPoints: return CreateNewSpawnPoints(GetPositionFromSpawnPoint(property.spawnPoints));
                case SpawnPositionType.custom: return CreateNewSpawnPoint(property.position);
            }

            return CreateNewSpawnPoint(Vector2.zero);
        }

        private List<GameObject> CreateNewSpawnPoint(Vector2 position)
        {
            List<Vector2> positions = new List<Vector2>();
            positions.Add(position);
            return CreateNewSpawnPoint(positions);
        }

        private List<GameObject> CreateNewSpawnPoint(List<Vector2> positions)
        {
            List<GameObject> spawnPoints = new List<GameObject>();

            foreach (Vector2 position in positions)
            {
                GameObject temp = new GameObject("spawnPoint");
                temp.transform.position = position;
                temp.transform.rotation = Quaternion.identity;
                spawnPoints.Add(temp);
            }

            return spawnPoints;
        }

        private List<GameObject> CreateNewSpawnPoints(GameObject spawn)
        {
            List<GameObject> spawnPoints = new List<GameObject>();
            spawnPoints.Add(spawn);

            return spawnPoints;
        }

        private Vector2 GetPositionFromCharacter(Character character)
        {
            return character.GetGroundPosition();
        }

        private List<Vector2> GetPositionFromCharacters(List<Character> characters, SpawnPositionType type)
        {
            List<Vector2> positions = new List<Vector2>();

            foreach (Character character in characters)
            {
                if (type == SpawnPositionType.noMainTarget && character == this.target) continue;
                positions.Add(character.GetGroundPosition());
            }

            return positions;
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
                case RotationType.mainTarget: return SetRotationOnTarget(spawnPoint.transform.position, offset);
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
