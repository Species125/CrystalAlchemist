using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CameraIntro : MonoBehaviour
{
    private enum Mode
    {
        always,
        session,
        oneTime
    }

    [SerializeField]
    private Mode mode;

    [HideIf("mode", Mode.always)]
    [Required]
    [SerializeField]
    private string gameProgressID;

    [HideIf("mode", Mode.always)]
    [Required]
    [SerializeField]
    private PlayerGameProgress playerProgress;

    [SerializeField]
    private UnityEvent onTriggered;

    private bool isPermanent = false;

    private bool CanPlay()
    {
        if (this.mode == Mode.oneTime) isPermanent = true;

        if (this.mode != Mode.always && this.playerProgress.Contains(this.gameProgressID, this.isPermanent)) return false;
        return true;
    }

    public void AddProgress()
    {
        this.playerProgress.AddProgress(this.gameProgressID, this.isPermanent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.CanPlay()) DoCutScene();
    }

    [Button]
    private void DoCutScene() => this.onTriggered?.Invoke();

    private void RaiseSignal(SimpleSignal signal) => signal?.Raise();
}
