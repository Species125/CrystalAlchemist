using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace CrystalAlchemist
{
    public static class CharacterUtil
    {
        public static Character Spawn(Character character, Vector2 position, Quaternion rotation)
        {
            if (!NetworkUtil.IsMaster()) return null;
            return PhotonNetwork.Instantiate(character.path, position, rotation).GetComponent<Character>();
        }

        public static void Spawn(AI character, Vector2 position, Quaternion rotation, Character target, bool hasMaxDuration, float maxDuration)
        {
            if (!NetworkUtil.IsMaster()) return;
            AI spawned = PhotonNetwork.Instantiate(character.path, position, rotation).GetComponent<AI>();
            spawned.InitializeAddSpawn(NetworkUtil.GetID(target), hasMaxDuration, maxDuration);
        }

        public static Breakable Spawn(Breakable character, Vector2 position, Quaternion rotation)
        {
            if (!NetworkUtil.IsMaster()) return null;
            return PhotonNetwork.Instantiate(character.path, position, rotation).GetComponent<Breakable>();
        }
    }
}
