using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public abstract int cooldown { get; }
    public int applyTimer, appliedTimer;

    public Ability() { applyTimer = cooldown; }

    public abstract Effect Effect();

    public bool Ready() { return applyTimer == 0; }
    public bool Finished() { return appliedTimer == 0; }

    public void OnTick(SnakeManager snakeManager)
    {
        if (appliedTimer > 0)
        {
            appliedTimer--;
            if (Finished()) { Remove(snakeManager); }
        }
        else if (applyTimer > 0) { applyTimer--; }
    }

    public void TryApply(SnakeManager snakeManager)
    {
        if (Ready()) { Apply(snakeManager); }
    }

    void Apply(SnakeManager snakeManager)
    {
        applyTimer = cooldown;
        appliedTimer = Effect().turns;
        Effect().Apply(snakeManager);
    }
    void Remove(SnakeManager snakeManager) { Effect().Remove(snakeManager); }
}

[CreateAssetMenu(fileName = "FreezeFrame", menuName = "Abilities/FreezeFrame")]
public class FreezeFrame : Ability
{
    [SerializeField]
    private int _cooldown = 20;

    public override int cooldown => _cooldown;

    public override Effect Effect() => new SpeedChange(4, 0.4f);
}

[CreateAssetMenu(fileName = "Ghost", menuName = "Abilities/Ghost")]
public class Ghost : Ability
{
    [SerializeField]
    private int _cooldown = 40;

    public override int cooldown => _cooldown;

    public override Effect Effect() => new Invincible(3);
}
