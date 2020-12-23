using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorPreviewButton : MonoBehaviour
{
    [SerializeField]
    private Vector2Signal signal;

    private Vector2 direction = Vector2.down;
    private List<Vector2> directions = new List<Vector2>() { Vector2.down, Vector2.left, Vector2.up, Vector2.right };
    private int index = 0;

    public void Click(int value)
    {
        this.index += value;

        if (this.index < 0) this.index = this.directions.Count - 1;
        else if (this.index >= this.directions.Count) this.index = 0;

        this.direction = this.directions[this.index];
        this.signal.Raise(this.direction);
    }
}
