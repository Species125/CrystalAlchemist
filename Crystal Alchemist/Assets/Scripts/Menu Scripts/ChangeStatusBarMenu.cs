﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStatusBarMenu : TitleScreenMenues
{
    [SerializeField]
    private CostType resourceType;

    private void OnEnable()
    {
        getLayout();
    }

    private void getLayout()
    {
        if(this.resourceType == CostType.life) setLayout(GlobalValues.healthBar);
        else if (this.resourceType == CostType.mana) setLayout(GlobalValues.manaBar);
    }

    private void setLayout(bool value)
    {
        if (value) this.switchButtons(this.secondButton, this.firstButton);
        else this.switchButtons(this.firstButton, this.secondButton);
    }

    public void changeLayout(bool useBar)
    {
        if (this.resourceType == CostType.life) GlobalValues.healthBar = useBar;
        else if (this.resourceType == CostType.mana) GlobalValues.manaBar = useBar;

        getLayout();

        this.switchSignal.Raise();
    }
}
