﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiniGameState
{
    play,
    win,
    lose
}

public class MiniGameTrys : MonoBehaviour
{
    [SerializeField]
    private List<MiniGameTrySlot> slots = new List<MiniGameTrySlot>();

    public int needed;
    public int max;
    public int successCounter;

    private int index;

    private void Start()
    {
        reset();
    }

    public void reset()
    {
        setSlots();
    }

    public MiniGameState canStartNewRound()
    {
        if (this.successCounter >= this.needed)
            return MiniGameState.win; //win
        else if (this.max - (this.index) < (this.needed-this.successCounter))
            return MiniGameState.lose; //lose
        else if (this.index < this.max)
            return MiniGameState.play;
        else
            return MiniGameState.lose; //lose
    }

    public void updateSlots(bool success)
    {
        if (success) this.successCounter++;
        this.slots[this.index].setMark(success);
        this.index++;
        if (!success) updateNeccessary(); 
    }

    public void setValues(int needed, int max)
    {
        this.needed = needed;
        this.max = max;
    }

    private void setSlots()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if ((i + 1) <= max) slots[i].gameObject.SetActive(true);
            else slots[i].gameObject.SetActive(false);
        }

        updateNeccessary();
    }

    private void updateNeccessary()
    {
        int neededSlotsLeft = this.needed - this.successCounter;
        int i = this.index;

        do
        {
            this.slots[i].setAsNeccessary();
            neededSlotsLeft--;
            i++;
        }
        while (i < this.slots.Count && neededSlotsLeft > 0);
    }



}
