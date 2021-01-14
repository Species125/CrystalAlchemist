
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class TitleScreenPatchNotes : MonoBehaviour
    {
        [SerializeField]
        private PatchNoteInfo infos;

        [SerializeField]
        private GameObject template;

        private void OnEnable()
        {
            foreach (PatchNote note in this.infos.GetPatchNotes())
            {
                GameObject line = Instantiate(template, this.transform);
                line.GetComponent<TextMeshProUGUI>().text = note.GetText();
                line.GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;
            }

            this.template.SetActive(false);
        }

        private void OnDisable()
        {
            for (int i = 1; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }

            this.template.SetActive(true);
        }
    }
}
