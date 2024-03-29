﻿using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(PhotonView))]
    public class BossMechanic : NetworkBehaviour
    {
        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private Character sender;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private Character target;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private List<Character> targets = new List<Character>();

        private List<BossMechanicProperty> properties = new List<BossMechanicProperty>();

        [Button]
        private void AddCharacters()
        {
            this.sender = FindObjectOfType<AI>();
            this.target = FindObjectOfType<Player>();
            
            this.gameObject.SetActive(false);
        }

        private void Awake()
        {
            foreach (BossMechanicProperty property in this.GetComponentsInChildren<BossMechanicProperty>(true)) this.properties.Add(property);
        }

        private void Start()
        {
            foreach (BossMechanicProperty property in this.properties) property.Initialize(this.sender, this.target, this.targets);
            InvokeRepeating("Updating", 0.1f, 10f);
        }

        private void Updating()
        {
            int counter = 0;
            foreach (BossMechanicProperty property in this.properties) if (!property.enabled || !property.gameObject.activeInHierarchy) counter++;

            if (counter >= this.properties.Count) Destroy(this.gameObject, 10f);
        }

        public void Initialize(Character sender, Character target, List<Character> targets)
        {
            this.sender = sender;
            this.target = target;
            this.targets = targets;
        }
    }
}
