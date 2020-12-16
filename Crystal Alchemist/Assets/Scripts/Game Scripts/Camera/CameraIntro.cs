using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CameraIntro : MonoBehaviour
{
    [HideLabel]
    [SerializeField]
    private ProgressValue progress;

    [SerializeField]
    private UnityEvent onTriggered;

    private bool isPermanent = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.progress.ContainsProgress()) DoCutScene();
    }

    [Button]
    private void DoCutScene() => this.onTriggered?.Invoke();

    private void RaiseSignal(SimpleSignal signal) => signal?.Raise();

    public void AddProgress() => this.progress.AddProgress();    
}
