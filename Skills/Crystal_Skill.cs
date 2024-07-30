using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] float crystalDuration;
    [SerializeField] GameObject crystalPrefab;
    GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] UI_SkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] bool cloneInsteadOfCrystal;

    [Header("Crystal Simple")]
    [SerializeField] UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Explode Crystal")]
    [SerializeField] UI_SkillTreeSlot unlockExplosiveCrystalButton;
    [SerializeField] float explosiveCooldown;
    [SerializeField] bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] bool canMoveToEnemy;
    [SerializeField] float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] bool canUseMultiStack;
    [SerializeField] int amountOfStacks;
    [SerializeField] float multiStackCooldown;
    [SerializeField] float useTimeWindow;
    [SerializeField] List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStackCrystal);
    }

    #region Unlock Skill Region

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiStackCrystal();
    }

    void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveCrystalButton.unlocked)
        {
            canExplode = true;
            cooldown = explosiveCooldown;
        }
    }

    void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }

    void UnlockMultiStackCrystal()
    {
        if (unlockMultiStackButton.unlocked)
            canUseMultiStack = true;
    }
    #endregion

    public override void UseSkill()
    {
        base.UseSkill();

        if (canUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrsytal();
        }

        else
        {

            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrsytal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    bool canUseMultiCrystal()
    {
        if (canUseMultiStack)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0f;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }

                return true;
            }
        }

        return false;
    }

    void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    void ResetAbility()
    {
        if (cooldownTimer > 0f)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
