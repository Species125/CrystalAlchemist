using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Cleaner : MonoBehaviour
    {
        [Button]
        public void CleanUp()
        {
            /*
        List<GameObject> test = new List<GameObject>();

        List<SimpleSignalListener> result = new List<SimpleSignalListener>();
        List<StringSignalListener> result2 = new List<StringSignalListener>();
        List<BoolSignalListener> result3 = new List<BoolSignalListener>();

        UnityUtil.GetChildren(this.transform, test);

        foreach (GameObject temp in test)
        {
            SimpleSignalListener blub = temp.GetComponent<SimpleSignalListener>();
            if (blub != null && blub.signal == null) result.Add(blub);

            StringSignalListener blub2 = temp.GetComponent<StringSignalListener>();
            if (blub2 != null && blub2.signal == null) result2.Add(blub2);

            BoolSignalListener blub3 = temp.GetComponent<BoolSignalListener>();
            if (blub3 != null && blub3.signal == null) result3.Add(blub3);
        }

        foreach (SimpleSignalListener temp in result)
        {
            Debug.Log(temp.gameObject + " - " + temp.transform.parent.gameObject);
            DestroyImmediate(temp);
        }

        foreach (StringSignalListener temp in result2)
        {
            Debug.Log(temp.gameObject + " - " + temp.transform.parent.gameObject);
            DestroyImmediate(temp);
        }

        foreach (BoolSignalListener temp in result3)
        {
            Debug.Log(temp.gameObject + " - " + temp.transform.parent.gameObject);
            DestroyImmediate(temp);
        }*/
        }


    }
}
