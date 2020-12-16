using UnityEngine;

public class AnalyseInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject marker;

    [SerializeField]
    private BoolValue isActive;

    private GameObject target;

    private void Awake()
    {
        this.marker.gameObject.SetActive(this.isActive.GetValue());
    }

    private void LateUpdate()
    {
        this.marker.gameObject.SetActive(this.isActive.GetValue());
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        this.marker.GetComponent<AnalyseUI>()?.setTarget(this.target);
    }
}
