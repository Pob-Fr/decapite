using System.Collections.Generic;
using UnityEngine;

public class DiceContent {
    List<EffectHolder> effects = new List<EffectHolder>();

    public void AddEffectHolder(EffectHolder newEffectHolder)
    {
        effects.Add(newEffectHolder);
    }

    public Effect RandomEffect()
    {
        int maxRange = 0;
        for (int i = 0; i < effects.Count; ++i)
        {
            maxRange += effects[i].GetChance();
        }
        int resultRand = Random.Range(0, maxRange);

        int index = 0;
        int sommePrevious = 0;
        while (index < effects.Count)
        {
            if ((effects[index].GetChance() + sommePrevious - resultRand) > 0)
                return effects[index].GetEffect();
            sommePrevious += effects[index].GetChance();
            ++index;
        }
        return effects[0].GetEffect();
    }
}

public class EffectHolder
{
    Effect effect;
    int chance;

    public EffectHolder (Effect newEffet, int newChance)
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
