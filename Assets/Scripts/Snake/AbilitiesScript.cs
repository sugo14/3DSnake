using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public KeyCode qKey = KeyCode.Q;
    public KeyCode eKey = KeyCode.E;

    public SnakeManager snakeManager;
    public GameObject qCube, eCube;

    public Ability qAbility, eAbility;
    bool wantsQ, wantsE;

    public void Reset()
    {
        wantsQ = false;
        wantsE = false;

        qAbility = new FreezeFrame();
        eAbility = new Ghost();

        UpdateButtons();
    }

    void Start() { Reset(); }

    public void OnTick()
    {
        qAbility.OnTick(snakeManager);
        eAbility.OnTick(snakeManager);
        if (wantsQ && qAbility.Ready())
        {
            qAbility.TryApply(snakeManager);
            qCube.GetComponent<AbilityCubeScript>().Activate();
        }
        if (wantsE && eAbility.Ready())
        {
            eAbility.TryApply(snakeManager);
            eCube.GetComponent<AbilityCubeScript>().Activate();
        }
        wantsQ = false;
        wantsE = false;

        UpdateButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(qKey))
        {
            wantsQ = true;
        }
        if (Input.GetKeyDown(eKey))
        {
            wantsE = true;
        }
    }

    void UpdateButtons() {
        for (int i = 0; i < 2; i++)
        {
            Ability ability = i == 0 ? qAbility : eAbility;
            GameObject cube = i == 0 ? qCube : eCube;
            AbilityCubeScript abilityCubeScript = cube.GetComponent<AbilityCubeScript>();

            if (ability.appliedTimer > 0)
            {
                abilityCubeScript.SetText(ability.appliedTimer.ToString());
                abilityCubeScript.SetFillAmount((float)(ability.Effect().turns - ability.appliedTimer) / ability.Effect().turns);
                abilityCubeScript.SetApplied(true);
            }
            else if (ability.applyTimer == 0)
            {
                abilityCubeScript.SetText("");
                abilityCubeScript.SetFillAmount(0);
                abilityCubeScript.SetApplied(true);
            }
            else
            {
                abilityCubeScript.SetText(ability.applyTimer.ToString());
                abilityCubeScript.SetFillAmount((float)ability.applyTimer / ability.cooldown);
                abilityCubeScript.SetApplied(false);
            }
        }
    }
}
