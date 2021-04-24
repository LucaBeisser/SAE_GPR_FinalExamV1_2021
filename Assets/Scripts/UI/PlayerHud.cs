using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private SpellCastingController spellCastingController;
    [SerializeField] private DropCollector dropCollector;

    [SerializeField] private Transform ability;
    [SerializeField] private Image spellIcon;
    [SerializeField] private GameObject spellOutline;
    [SerializeField] private TMPro.TMP_Text spellCooldownText;
    [SerializeField] private GameObject collectUIObject;

    [Header("Special Ability")]

    [SerializeField] private Transform specialAbility;
    [SerializeField] private Image specialAbilityIcon;
    [SerializeField] private TMPro.TMP_Text specialAbilityCooldownText;
    [SerializeField] private GameObject specialAbilityOutline;

    private void Start()
    {
        Debug.Assert(spellCastingController != null, "SpellCastingController reference is null");
        Debug.Assert(dropCollector != null, "DropCollector reference is null");

        spellIcon.sprite = spellCastingController.SimpleAttackSpellDescription.SpellIcon;

        specialAbilityCooldownText.text = "";

        dropCollector.DropsInRangeChanged += OnDropsInRangeChanged;
    }

    private void OnDropsInRangeChanged()
    {
        collectUIObject.SetActive(dropCollector.DropsInRangeCount > 0);
    }


    public void ActivateAbility(AbilityUIs abilityUI)
    {
        spellCooldownText.text = spellCastingController.GetSimpleAttackCooldown().ToString("0.0");
        spellIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
        spellOutline.SetActive(true);
        ability.localScale *= abilityUI.GrowMultiplicator;

        StartCoroutine(ShrinkAbilityCoroutine(abilityUI.TransitionTime));
    }
    public void ActivateSpecialAbility(AbilityUIs abilityUI)
    {
        specialAbilityCooldownText.text = spellCastingController.GetspecialAttackCooldown().ToString("0.0");
        specialAbilityIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
        specialAbilityOutline.SetActive(true);
        specialAbility.localScale *= abilityUI.GrowMultiplicator;

        StartCoroutine(ShrinkSpecialAbilityCoroutine(abilityUI.TransitionTime));
    }


    private IEnumerator ShrinkAbilityCoroutine(float shrinkTime)
    {
        float value = (ability.localScale.x - 1f) / shrinkTime;

        while (ability.localScale.x > 1f)
        {
            spellCooldownText.text = spellCastingController.GetSimpleAttackCooldown().ToString("0.0");
            ability.localScale -= Vector3.one * Time.deltaTime * value;
            yield return null;
        }

        ability.localScale = Vector3.one;
        spellCooldownText.text = "";
        spellIcon.color = Color.white;
        spellOutline.SetActive(false);
    }

    private IEnumerator ShrinkSpecialAbilityCoroutine(float shrinkTime)
    {
        float value = (specialAbility.localScale.x - 1f) / shrinkTime;

        while (specialAbility.localScale.x > 1f)
        {
            specialAbilityCooldownText.text = spellCastingController.GetspecialAttackCooldown().ToString("0.0");
            specialAbility.localScale -= Vector3.one * Time.deltaTime * value;
            yield return null;
        }

        specialAbility.localScale = Vector3.one;
        specialAbilityCooldownText.text = "";
        specialAbilityIcon.color = Color.white;
        specialAbilityOutline.SetActive(false);
    }

    public void SetSpecialAbilitySlot(ProjectileSpellDescription projectileSpellDescription)
    {
        specialAbilityIcon.sprite = projectileSpellDescription.SpellIcon;
        specialAbility.localScale = Vector3.one;
        specialAbilityCooldownText.text = "";
        specialAbilityIcon.color = Color.white;
        specialAbilityOutline.SetActive(false);
    }
}

[System.Serializable]
public struct AbilityUIs
{
    [Range(1f, 5f)] public float GrowMultiplicator;
    [Range(0.001f, 5f)] public float TransitionTime;
}
