using DG.Tweening;
using UnityEngine;

public class CutsceneCharacterMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 position;

    [SerializeField]
    private float duration;

    private Rigidbody2D myRigidbody;

    private void Start() => GameEvents.current.OnPlayerSpawned += AddPlayer;    

    private void OnDestroy() => GameEvents.current.OnPlayerSpawned -= AddPlayer;

    private void AddPlayer(GameObject gameObject)
    {
        this.myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Play()
    {
        this.myRigidbody?.DOMove(position, this.duration);
    }
}
