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
    public Button qButton, eButton;

    public Ability qAbility, eAbility;
    bool wantsQ, wantsE;

    List<Effect> currEffects = new List<Effect>();

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
        if (wantsQ) { qAbility.TryApply(snakeManager); }
        if (wantsE) { eAbility.TryApply(snakeManager); }
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
            Button button = i == 0 ? qButton : eButton;

            if (ability.appliedTimer > 0)
            {
                button.GetComponentInChildren<TMP_Text>().text = ability.appliedTimer.ToString();
                button.GetComponentsInChildren<Image>()[1].fillAmount = (float)(ability.Effect().turns - ability.appliedTimer) / ability.Effect().turns;
                button.GetComponentsInChildren<Image>()[2].enabled = false;
            }
            else if (ability.applyTimer == 0)
            {
                button.GetComponentInChildren<TMP_Text>().text = "";
                button.GetComponentsInChildren<Image>()[1].fillAmount = 0;
                button.GetComponentsInChildren<Image>()[2].enabled = false;
            }
            else
            {
                button.GetComponentInChildren<TMP_Text>().text = ability.applyTimer == 0 ? "" : ability.applyTimer.ToString();
                button.GetComponentsInChildren<Image>()[1].fillAmount = (float)ability.applyTimer / ability.cooldown;
                button.GetComponentsInChildren<Image>()[2].enabled = true;
            }
        }
    }
}
