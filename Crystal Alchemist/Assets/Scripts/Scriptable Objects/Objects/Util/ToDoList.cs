using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class ToDoVersion
    {
        public string version = "0.0.0";
        public List<ToDoCategory> categories = new List<ToDoCategory>();
    }

    [System.Serializable]
    public class ToDoCategory
    {
        public enum Category
        {
            Feature,
            Bug,
            Change,
            Rework
        }

        public enum Type
        {
            Player,
            Character,
            Objects,
            Skills,
            Items,
            Graphics,
            Networking,
            Audio,
            System,
            Content
        }

        public Category category;
        public Type type;

        [TextArea]
        public string description;
    }

    [CreateAssetMenu(menuName = "Game/Editor/To Do List")]
    public class ToDoList : ScriptableObject
    {
        public List<ToDoVersion> roadmap = new List<ToDoVersion>();
    }
}