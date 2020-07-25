using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FloatValue timePlayed;

    private void OnDestroy()
    {
        this.timePlayed.SetValue(this.timePlayed.GetValue()+Time.timeSinceLevelLoad);
    }
}
