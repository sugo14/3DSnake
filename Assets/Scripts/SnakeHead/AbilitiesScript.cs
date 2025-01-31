using UnityEngine;

public class Abilities : MonoBehaviour
{
    public Ability qAbility, eAbility;

    public KeyCode qKey = KeyCode.Q, eKey = KeyCode.E;

    public SnakeManager snakeManager;
    public GameObject qCube, eCube;

    AbilityInstance qInstance, eInstance;
    bool wantsQ, wantsE;

    public void Reset()
    {
        wantsQ = false;
        wantsE = false;

        qCube.GetComponent<AbilityCubeScript>().SetColor(snakeManager.snakeSpecies.snakeSpecies.bodyMaterials[0]);
        eCube.GetComponent<AbilityCubeScript>().SetColor(snakeManager.snakeSpecies.snakeSpecies.bodyMaterials[0]);

        if (snakeManager.snakeSpecies.snakeSpecies.qAbilityName == "") { qAbility = null; }
        else { qAbility = AbilityRegistry.GetAbility(snakeManager.snakeSpecies.snakeSpecies.qAbilityName); }

        if (snakeManager.snakeSpecies.snakeSpecies.eAbilityName == "") { eAbility = null; }
        else { eAbility = AbilityRegistry.GetAbility(snakeManager.snakeSpecies.snakeSpecies.eAbilityName); }

        if (qAbility != null)
        {
            qInstance = qAbility.Instantiate();
            qCube.GetComponent<AbilityCubeScript>().SetSprite(Resources.Load<Sprite>(qAbility.spritePath));
        }
        else { qCube.GetComponent<AbilityCubeScript>().SetSprite(Resources.Load<Sprite>("Square")); }

        if (eAbility != null)
        {
            eInstance = eAbility.Instantiate();
            eCube.GetComponent<AbilityCubeScript>().SetSprite(Resources.Load<Sprite>(eAbility.spritePath));
        }
        else { eCube.GetComponent<AbilityCubeScript>().SetSprite(Resources.Load<Sprite>("Square")); }

        UpdateButtons();
    }

    void Start() { Reset(); }

    public void OnTick()
    {
        if (qAbility != null)
        {
            qInstance.OnTick(snakeManager);
            if (wantsQ && qInstance.Ready())
            {
                qInstance.Apply(snakeManager);
                qCube.GetComponent<AbilityCubeScript>().Activate();
            }
        }

        if (eAbility != null)
        {
            eInstance.OnTick(snakeManager);
            if (wantsE && eInstance.Ready())
            {
                eInstance.Apply(snakeManager);
                eCube.GetComponent<AbilityCubeScript>().Activate();
            }
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
        UpdateButtons();
    }

    void UpdateButtons() {
        for (int i = 0; i < 2; i++)
        {
            Ability originalAbility = i == 0 ? qAbility : eAbility;
            AbilityInstance ability = i == 0 ? qInstance : eInstance;
            GameObject cube = i == 0 ? qCube : eCube;
            AbilityCubeScript abilityCubeScript = cube.GetComponent<AbilityCubeScript>();

            if (originalAbility == null)
            {
                abilityCubeScript.SetText("");
                abilityCubeScript.SetFillAmount(1);
                abilityCubeScript.SetApplied(false);
                continue;
            }

            if (ability.AppliedTimer() > 0)
            {
                abilityCubeScript.SetText(ability.AppliedTimer().ToString());
                abilityCubeScript.SetFillAmount((float)(ability.ability.GetDuration(snakeManager) - ability.AppliedTimer()) / ability.ability.GetDuration(snakeManager));
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
                abilityCubeScript.SetFillAmount((float)ability.applyTimer / ability.ability.cooldown);
                abilityCubeScript.SetApplied(false);
            }
        }
    }
}
