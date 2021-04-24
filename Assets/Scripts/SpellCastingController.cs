using System.Collections;
using UnityEngine;

public interface IPlayerAction
{
    bool IsInAction();
}

public class SpellCastingController : MonoBehaviour, IPlayerAction
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform castLocationTransform;
    [SerializeField] private ProjectileSpellDescription simpleAttackSpell;
    [SerializeField] private PlayerHud playerHud;
    [SerializeField] private DropCollector dropCollector;

    private ProjectileSpellDescription specialAttackSpell;

    private bool inAction;
    private float lastSimpleAttackTimestamp = -100;
    private float lastSpecialAttackTimestamp = -100;

    public SpellDescription SimpleAttackSpellDescription { get => simpleAttackSpell; }

    private void Start()
    {
        Debug.Assert(simpleAttackSpell, "No spell assigned to SpellCastingController.");
        dropCollector.DropCollected += OnCollectDrop;
    }

    void Update()
    {
        bool simpleAttack = Input.GetButtonDown("Fire1");
        bool specialAttack = Input.GetButtonDown("Fire2");

        if (!inAction)
        {
            if (simpleAttack && GetSimpleAttackCooldown() == 0)
            {
                StartCoroutine(SimpleAttackRoutine());
            }
            else if (specialAttack)
            {
                StartCoroutine(SpecialAttackRoutine());
                Debug.Log("Trigger special attack");
            }
        }
    }
    

    private void OnCollectDrop(Drop drop)
    {
        if (drop.SpecialAbilityDrop == null) return;

        specialAttackSpell = drop.SpecialAbilityDrop;
        playerHud.SetSpecialAbilitySlot(specialAttackSpell);
    }

    private IEnumerator SimpleAttackRoutine()
    {
        inAction = true;
        animator.SetTrigger(simpleAttackSpell.AnimationVariableName);

        yield return new WaitForSeconds(simpleAttackSpell.ProjectileSpawnDelay);

        playerHud.ActivateAbility(simpleAttackSpell.AbilityUI);
        Instantiate(simpleAttackSpell.ProjectilePrefab, castLocationTransform.position, castLocationTransform.rotation);

        yield return new WaitForSeconds(simpleAttackSpell.Duration - simpleAttackSpell.ProjectileSpawnDelay);

        lastSimpleAttackTimestamp = Time.time;
        inAction = false;
    }

    private IEnumerator SpecialAttackRoutine()
    {
        if (specialAttackSpell != null)
        {
            inAction = true;
            animator.SetTrigger(specialAttackSpell.AnimationVariableName);

            yield return new WaitForSeconds(specialAttackSpell.ProjectileSpawnDelay);

            playerHud.ActivateSpecialAbility(specialAttackSpell.AbilityUI);
            Instantiate(specialAttackSpell.ProjectilePrefab, castLocationTransform.position, castLocationTransform.rotation);

            yield return new WaitForSeconds(specialAttackSpell.Duration - specialAttackSpell.ProjectileSpawnDelay);

            lastSpecialAttackTimestamp = Time.time;
            inAction = false;
        }
    }

    public bool IsInAction()
    {
        return inAction;
    }

    public float GetSimpleAttackCooldown()
    {
        return Mathf.Max(0, lastSimpleAttackTimestamp + simpleAttackSpell.Cooldown - Time.time);
    }
    public float GetspecialAttackCooldown()
    {
        if (specialAttackSpell == null) return 0f;

        return Mathf.Max(0, lastSpecialAttackTimestamp + specialAttackSpell.Cooldown - Time.time);
    }
}
