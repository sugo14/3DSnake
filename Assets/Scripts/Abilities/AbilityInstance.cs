using System.Collections.Generic;

public class AbilityInstance
{
    public Ability ability;
    public int applyTimer, appliedTimer;

    public AbilityInstance(Ability ability)
    {
        this.ability = ability;
        applyTimer = ability.cooldown;
    }

    public void OnTick(SnakeManager snakeManager)
    {
        if (appliedTimer > 0) { appliedTimer--; }
        else if (applyTimer > 0) { applyTimer--; }
    }

    public bool Ready() { return applyTimer == 0; }

    public int AppliedTimer() { return appliedTimer; }

    public void Apply(SnakeManager snakeManager)
    {
        applyTimer = ability.cooldown;
        appliedTimer = ability.Effect(snakeManager)[0].turns;
        List<Effect> effects = ability.Effect(snakeManager);
        foreach (Effect effect in effects)
        {
            snakeManager.effectManager.AddEffect(effect);
            if (effect.turns < appliedTimer) { appliedTimer = effect.turns; }
        }
    }
}
