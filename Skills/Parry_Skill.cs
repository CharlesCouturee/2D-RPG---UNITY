using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Restore Health on Parry")]
    [SerializeField] UI_SkillTreeSlot parryRestoreHealthUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] float restorHealthPercentage;
    public bool parryRestoreHealthUnlocked { get; private set; }

    [Header("Parry with a Mirage")]
    [SerializeField] UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (parryRestoreHealthUnlocked)
        {
            int restorAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restorHealthPercentage);
            player.stats.IncreaseHealthBy(restorAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreHealthUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestoreHealth);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestoreHealth();
        UnlockParryWithMirage();
    }

    void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    void UnlockParryRestoreHealth()
    {
        if (parryRestoreHealthUnlockButton.unlocked)
            parryRestoreHealthUnlocked = true;
    }

    void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
