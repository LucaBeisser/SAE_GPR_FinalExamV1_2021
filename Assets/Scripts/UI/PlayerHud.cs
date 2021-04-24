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

    private void Start()
    {
        Debug.Assert(spellCastingController != null, "SpellCastingController reference is null");
        Debug.Assert(dropCollector != null, "DropCollector reference is null");

        spellIcon.sprite = spellCastingController.SimpleAttackSpellDescription.SpellIcon;

        dropCollector.DropsInRangeChanged += OnDropsInRangeChanged;
    }

    private void OnDropsInRangeChanged()
    {
        collectUIObject.SetActive(dropCollector.DropsInRangeCount > 0);
    }

    private void Update()
    {
        float cooldown = spellCastingController.GetSimpleAttackCooldown();
        if (cooldown > 0)
        {
            spellCooldownText.text = cooldown.ToString("0.0");
            spellIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
            spellOutline.SetActive(true);
        }
        else
        {
            spellCooldownText.text = "";
            spellIcon.color = Color.white;
            spellOutline.SetActive(false);
        }
    }

    public void ActivateAbility(AbilityUIs abilityUI)
    {
        ability.localScale *= abilityUI.GrowMultiplicator;
        StartCoroutine(ShrinkCoroutine(abilityUI.TransitionTime));
    }
    private IEnumerator ShrinkCoroutine(float shrinkTime)
    {
        float value = (ability.localScale.x - 1f) / shrinkTime;

        while (ability.localScale.x > 1f)
        {
            ability.localScale -= Vector3.one * Time.deltaTime * value;
            yield return null;
        }
        ability.localScale = Vector3.one;
    }
}

[System.Serializable]
public struct AbilityUIs
{
    [Range(1f, 5f)] public float GrowMultiplicator;
    [Range(0.001f, 5f)] public float TransitionTime;
}
