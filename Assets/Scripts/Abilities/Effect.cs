public abstract class Effect {
    public int turns;

    public Effect(int turns) { this.turns = turns; }

    public virtual float SpeedMult() { return 1; }
    public virtual bool IsInvincible() { return false; }
    public virtual int SnipLength() { return 0; }
    public virtual int PointMult() { return 1; }
    public virtual int PointAdd() { return 0; }

    public abstract void Apply(SnakeManager snakeManager);
    public abstract void Remove(SnakeManager snakeManager);
}

public class SpeedChange : Effect {
    public float speedMultiplier;

    public SpeedChange(int turns, float speedMultiplier) : base(turns)
    {
        this.speedMultiplier = speedMultiplier;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.tickTime /= speedMultiplier; }

    public override void Remove(SnakeManager snakeManager) { snakeManager.tickTime *= speedMultiplier; }
}

public class Invincible : Effect {
    public Invincible(int turns) : base(turns) { }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.isInvincible = true; }

    public override void Remove(SnakeManager snakeManager) { snakeManager.snakeMove.isInvincible = false; }
}
