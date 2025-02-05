using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Effect
{
    public virtual void OnTick(SnakeManager snakeManager) { }
    public virtual bool WantsToDetach() { return false; }
    public virtual void Apply(SnakeManager snakeManager) { }
    public virtual void Remove(SnakeManager snakeManager) { }

    public virtual bool hasAppearance { get { return false; } }
    public virtual Color color { get { return Color.white; } }
    public virtual string spritePath { get { return ""; } }
    public virtual void EditAppearance(GameObject gameObject) { }
}

public class SpeedMultiplier : Effect
{
    public float speedMultiplier;

    public SpeedMultiplier(float speedMultiplier) { this.speedMultiplier = speedMultiplier; }

    public override void Apply(SnakeManager snakeManager) { snakeManager.tickTime /= speedMultiplier; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.tickTime *= speedMultiplier; }
}

public class FoodModifier : Effect
{
    public float foodAdd, foodMult;

    public FoodModifier(float foodAdd, float foodMult)
    {
        this.foodAdd = foodAdd;
        this.foodMult = foodMult;
    }

    public override void Apply(SnakeManager snakeManager)
    {
        FoodManager fm = snakeManager.foodManager.GetComponent<FoodManager>();
        fm.foodAdd += foodAdd;
        fm.foodMult *= foodMult;
    }
    public override void Remove(SnakeManager snakeManager)
    {
        FoodManager fm = snakeManager.foodManager.GetComponent<FoodManager>();
        fm.foodAdd -= foodAdd;
        fm.foodMult /= foodMult;
    }
}

public class LengthModifier : Effect
{
    public int lengthAdd;
    public float lengthMult;

    public LengthModifier(int lengthAdd, float lengthMult)
    {
        this.lengthAdd = lengthAdd;
        this.lengthMult = lengthMult;
    }

    public override void Apply(SnakeManager snakeManager)
    {
        snakeManager.snakeMove.lengthMod += lengthAdd;
        snakeManager.snakeMove.lengthMult *= lengthMult;
    }
    public override void Remove(SnakeManager snakeManager)
    {
        snakeManager.snakeMove.lengthMod -= lengthAdd;
        snakeManager.snakeMove.lengthMult /= lengthMult;
    }
}

public abstract class CurseOfAdjacency : Effect
{
    protected int CalculateAdjacencies(SnakeManager snakeManager) {
        int adjacencies = 2;
        HashSet<Vector3> positions = new HashSet<Vector3>();
        List<CubeOrient> orients = new List<CubeOrient>();

        foreach (GameObject bodyPart in snakeManager.snakeMove.snakeBody)
        {
            orients.Add(bodyPart.GetComponent<SnakeBody>().cubeOrient);
            positions.Add(bodyPart.transform.position);
        }
        orients.Add(snakeManager.snakeMove.orient);
        positions.Add(snakeManager.transform.position);

        foreach (CubeOrient co in orients)
        {
            CubeOrient co2 = CubeOrient.Copy(co);
            co2.dir = Vector2Int.up;
            co2.Go();
            if (positions.Contains(co2.WorldPosition())) { adjacencies++; }
            co2 = CubeOrient.Copy(co);
            co2.dir = Vector2Int.down;
            co2.Go();
            if (positions.Contains(co2.WorldPosition())) { adjacencies++; }
            co2 = CubeOrient.Copy(co);
            co2.dir = Vector2Int.left;
            co2.Go();
            if (positions.Contains(co2.WorldPosition())) { adjacencies++; }
            co2 = CubeOrient.Copy(co);
            co2.dir = Vector2Int.right;
            co2.Go();
            if (positions.Contains(co2.WorldPosition())) { adjacencies++; }
            adjacencies -= 2;
        }

        return Math.Max(0, adjacencies);
    }
}

public class COASpeed : CurseOfAdjacency
{
    public float speedMult, lastMult;
    public int lastAdjacencyCount = 0;
    public override bool hasAppearance { get { return true; } }
    public override Color color { get { return Color.white; } }
    public override string spritePath { get { return "Skull-Sprite"; } }

    public COASpeed(float speedMult) { this.speedMult = speedMult; }

    float CalculateSpeedMultiplier(SnakeManager snakeManager)
    {
        lastAdjacencyCount = CalculateAdjacencies(snakeManager);
        return 1 + (lastAdjacencyCount * speedMult / (float)Math.Sqrt(snakeManager.snakeMove.currLength));
    }

    public override void OnTick(SnakeManager snakeManager)
    {
        lastMult = CalculateSpeedMultiplier(snakeManager);
        snakeManager.effectManager.AddEffect(new SpeedChange(1, lastMult));
    }

    public override void EditAppearance(GameObject gameObject)
    {
        TMP_Text text = gameObject.GetComponentInChildren<TMP_Text>();
        text.text = lastAdjacencyCount.ToString();
        // change color from white to red based on speedMult
        text.color = new Color(1, 2 - lastMult, 2 - lastMult);
    }
}
