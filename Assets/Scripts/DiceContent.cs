using System.Collections.Generic;
using UnityEngine;

public class DiceContent {
    List<EffectHolder> effects = new List<EffectHolder>();

    public void AddEffectHolder(EffectHolder newEffectHolder)
    {
        effects.Add(newEffectHolder);
    }

    public Effect RandomEffect(int randNb)
    {
        int sommeNb = 0;
        for (int i = 0; i < effects.Count;++i)
        {
            sommeNb += effects[i].GetChance();
        }
        int resultRand = Random.Range(0, sommeNb);

        int j = 0;
        int currentNb = 0;
        while (currentNb < effects[j].GetChance())
        {
            currentNb = effects[j].GetChance();
        }

        Debug.Log(effects[j].GetEffect());
        return new EffectSpawnZombi(1);
    }
}

public class EffectHolder
{
    Effect effect;
    int chance;

    EffectHolder (Effect newEffet, int newChance)
    {
        this.effect = newEffet;
        this.chance = newChance;
    }

    public void ChangeChance(int newChance)
    {
        chance = newChance;
    }

    public int GetChance()
    {
        return chance;
    }

    public Effect GetEffect()
    {
        return effect;
    }
}
