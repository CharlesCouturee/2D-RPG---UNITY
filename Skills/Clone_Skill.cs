using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] float attackMultiplier;
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;

    [Header("Clone Attack")]
    [SerializeField] UI_SkillTreeSlot unlockCloneAttackButton;
    [SerializeField] float cloneAttackMultiplier;
    [SerializeField] bool canAttack;

    [Header("Aggressive Clone")]
    [SerializeField] UI_SkillTreeSlot unlockAggressiveCloneButton;
    [SerializeField] float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple Clone")]
    [SerializeField] UI_SkillTreeSlot unlockMultipleCloneButton;
    [SerializeField] float multiCloneAttackMultiplier;
    [SerializeField] bool canDuplicateClone;
    [SerializeField] float chanceToDuplicate;

    [Header("Crystal Instead Of Clone")]
    [SerializeField] UI_SkillTreeSlot unlockCrystalInsteadButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        unlockCloneAttackButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        unlockAggressiveCloneButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        unlockMultipleCloneButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        unlockCrystalInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockMultiClone();
        UnlockCrystalInstead();
    }

    #region Unlock Region
    void UnlockCloneAttack()
    {
        if (unlockCloneAttackButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    void UnlockAggressiveClone()
    {
        if (unlockAggressiveCloneButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    void UnlockMultiClone()
    {
        if (unlockMultipleCloneButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    void UnlockCrystalInstead()
    {
        if (unlockCrystalInsteadButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }
    #endregion

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrsytal();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetUpClone(
            _clonePosition, 
            cloneDuration, 
            canAttack, 
            _offset, 
            FindClosestEnemy(player.transform), 
            canDuplicateClone, 
            chanceToDuplicate, 
            player,
            attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(1.8f * player.facingDir, 0f)));
    }

    IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
