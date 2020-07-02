﻿using UnityEngine;

public class NonPlayer : Character
{
    public override void Start()
    {
        if (this.IsSummoned)
        {
            SetCharacterSprites(false);
            SpawnOut();
        }
        
        base.Start();

        if (this.stats.showAnalyse)
        {
            AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.GetHeadPosition(), Quaternion.identity, this.transform);
            analyse.SetTarget(this.gameObject);
        }

        if (this.IsSummoned)
        {
            SetCharacterSprites(true);
            PlayRespawnAnimation();
            SpawnIn();
        }
    }
}
