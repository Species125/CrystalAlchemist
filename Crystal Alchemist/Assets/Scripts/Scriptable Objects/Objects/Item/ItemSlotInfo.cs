using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(menuName = "Game/Items/Inventory Info")]
public class ItemSlotInfo : ScriptableObject
{
    [BoxGroup("Inventory")]
    [SerializeField]
    [MinValue(-1)]
    private int itemSlot = -1;

    [BoxGroup("Inventory")]
    [SerializeField]
    private SimpleSignal keyItemSignal;


    public void SetSlot(int slot, SimpleSignal signal)
    {
        this.itemSlot = slot;
        this.keyItemSignal = signal;
    }

    public bool isID(int ID)
    {
        if (this.itemSlot == ID) return true;
        else return false;
    }

    public void raiseKeySignal()
    {
        if (this.keyItemSignal != null) this.keyItemSignal.Raise();
    }
}
