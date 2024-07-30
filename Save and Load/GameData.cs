using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionnary<string, bool> skillTree;
    public SerializableDictionnary<string, int> inventory;
    public List<string> equipmentID;

    public SerializableDictionnary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializableDictionnary<string, float> volumeSettings;

    public GameData()
    {
        lostCurrencyX = 0;
        lostCurrencyY = 0;
        lostCurrencyAmount = 0;

        currency = 0;
        skillTree = new SerializableDictionnary<string, bool>();
        inventory = new SerializableDictionnary<string, int>();
        equipmentID = new List<string>();

        checkpoints = new SerializableDictionnary<string, bool>();
        closestCheckpointId = string.Empty;

        volumeSettings = new SerializableDictionnary<string, float>();
    }
}
