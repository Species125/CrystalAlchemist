using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributeStats : MonoBehaviour
{
    [SerializeField]
    private attributeType type;

    [SerializeField]
    private CharacterAttributeMenu mainMenu;

    public Image icon;

    [SerializeField]
    private List<GameObject> pointsList = new List<GameObject>();

    private void Start() => updateUI();
   
    public string GetName()
    {
        return FormatUtil.GetLocalisedText(this.gameObject.name+"_Name", LocalisationFileType.menues);
    }

    public string GetDescription()
    {
        return FormatUtil.GetLocalisedText(this.gameObject.name + "_Description", LocalisationFileType.menues);
    }

    public void setAttributes(int value)
    {
        int points = this.mainMenu.attributes.GetPoints(this.type);
        int pointsLeft = this.mainMenu.GetPointsLeft();

        if (points > 0 && points == value) value--;
        else if (value - points > pointsLeft) value = points + pointsLeft;

        this.mainMenu.attributes.SetPoints(this.type, value);
        this.mainMenu.attributes.SetValues();
        updateUI();
        this.mainMenu.UpdatePoints();
    }

    private void updateUI()
    {
        for(int i = 0; i< this.pointsList.Count; i++)
        {
            this.pointsList[i].SetActive(false);
            if(i+1 <= this.mainMenu.attributes.GetPoints(this.type)) this.pointsList[i].SetActive(true);
        }
    }
}
