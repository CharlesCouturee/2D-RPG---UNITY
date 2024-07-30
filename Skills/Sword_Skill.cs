using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] UI_SkillTreeSlot unlockBounceButton;
    [SerializeField] int bounceAmount;
    [SerializeField] float bounceGravity;
    [SerializeField] float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] UI_SkillTreeSlot unlockPierceButton;
    [SerializeField] int pierceAmount;
    [SerializeField] float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] UI_SkillTreeSlot unlockSpinButton;
    [SerializeField] float hitCooldown = 0.35f;
    [SerializeField] float maxTravelDistance = 7f;
    [SerializeField] float spinDuration = 2f;
    [SerializeField] float spinGravity = 1f;

    [Header("Skill Info")]
    [SerializeField] UI_SkillTreeSlot unlockSwordButton;
    public bool swordUnlocked {  get; private set; }
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchForce;
    [SerializeField] float swordGravity;
    [SerializeField] float freezeTimeDuration;
    [SerializeField] float returnSpeed;
    float defaultGravity;

    [Header("Passive skills")]
    [SerializeField] UI_SkillTreeSlot unlockTimeStopButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] UI_SkillTreeSlot unlockVulnerableButton;
    public bool vulnerableUnlocked { get; private set; }    

    Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] int numberOfDots;
    [SerializeField] float spaceBteweenDots;
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] Transform dotsParent;

    GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        defaultGravity = swordGravity;
        //SetUpGravity();

        unlockSwordButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        unlockBounceButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        unlockPierceButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        unlockSpinButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        unlockVulnerableButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
        unlockTimeStopButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
    }

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounceSword();
        UnlockSpinSword();
        UnlockPierceSword();
        UnlockTimeStop();
        UnlockVulnerable();
    }

    protected override void Update()
    {
        SetUpGravity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBteweenDots);
            }
        }
    }

    void SetUpGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
        else
            swordGravity = defaultGravity;
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetUpBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetUpPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetUpSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        newSwordScript.SetUpSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Unlock Region
    void UnlockTimeStop()
    {
        if (unlockTimeStopButton.unlocked)
            timeStopUnlocked = true;
    }

    void UnlockVulnerable()
    {
        if (unlockVulnerableButton.unlocked)
            vulnerableUnlocked = true;
    }

    void UnlockSword()
    {
        if (unlockSwordButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    void UnlockBounceSword()
    {
        if (unlockBounceButton.unlocked)
            swordType = SwordType.Bounce;
    }

    void UnlockPierceSword()
    {
        if (unlockPierceButton.unlocked)
            swordType = SwordType.Pierce;
    }

    void UnlockSpinSword()
    {
        if (unlockSpinButton.unlocked)
            swordType = SwordType.Spin;
    }

    #endregion

    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x, 
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
