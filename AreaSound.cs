using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] int sfxSoundIndex;
    [SerializeField] bool isSFX;
    [SerializeField] int bgmSoundIndex;
    [SerializeField] bool isBGM;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && isSFX)
            AudioManager.instance.PlaySFX(sfxSoundIndex, null);

        else if (collision.GetComponent<Player>() != null && isBGM)
            AudioManager.instance.PlaySFX(bgmSoundIndex, null);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && isSFX)
            AudioManager.instance.StopSFXWithTime(sfxSoundIndex);

        if (collision.GetComponent<Player>() != null && isBGM)
            AudioManager.instance.StopSFXWithTime(bgmSoundIndex);
    }
}
