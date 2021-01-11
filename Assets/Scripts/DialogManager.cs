using System;
using System.Collections.Generic;
using LitJson;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private string jsonPath;

    private JsonData dialog;
    private int index;
    private int backIndex;
    private string speaker;

    private List<JsonData> layerBackup;
    private bool inDialog;

    #region Unity Methods

    private void Awake()
    {
        DeactivateOptions();
    }

    #endregion

    #region Dialog Methods

    public bool LoadDialog(string eventName)
    {
        if (!inDialog)
        {
            index = 0;
            var jsonTextFile = Resources.Load<TextAsset>(jsonPath + eventName);
            dialog = JsonMapper.ToObject(jsonTextFile.text);
            SetCurrentLayer(dialog);
            inDialog = true;
        }

        return inDialog;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public bool PrintLine()
    {
        if (inDialog)
        {
            var line = GetCurrentLayer()[index];

            foreach (var key in line.Keys)
            {
                speaker = key;
                break;
            }

            switch (speaker)
            {
                case "EOD": // End of dialog
                    inDialog = false;
                    displayText.SetText("");
                    break;
                case "SOO": // Start of options
                    var options = line[0];
                    backIndex = index;
                    displayText.SetText("");

                    for (int optionsNumber = 0; optionsNumber < options.Count; optionsNumber++)
                    {
                        ActivateOption(buttons[optionsNumber], options[optionsNumber]);
                    }

                    break;
                case "EOO": // End of options
                    GoPreviousLayer();
                    index = backIndex + 1;
                    PrintLine();
                    break;
                default:
                    // This method invocation is expensive, only called here because OnUpdate is controlled by OnKeyDown
                    SetDialogTextColor(speaker);
                    displayText.SetText(speaker.ToUpper() + ": " + line[0]);
                    index++;
                    break;
            }
        }

        return inDialog;
    }

    private void SetDialogTextColor(string character)
    {
        displayText.color = GameObject.Find(character).GetComponent<Character>().GetDialogColor();
    }

    #endregion

    #region Options Methods

    private void ActivateOption(GameObject button, JsonData choice)
    {
        button.SetActive(true);
        button.GetComponentInChildren<TMP_Text>().SetText(choice[0][0].ToString());
        button.GetComponent<Button>().onClick.AddListener(delegate { OnClickOption(choice); });
    }

    private void DeactivateOptions()
    {
        foreach (var button in buttons)
        {
            button.SetActive(false);
            button.GetComponentInChildren<TMP_Text>().SetText("");
            button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void OnClickOption(JsonData choice)
    {
        SetCurrentLayer(choice[0]);
        index = 1;
        PrintLine();
        DeactivateOptions();
    }

    #endregion

    #region Layer Methods

    private void SetCurrentLayer(JsonData currentLayer)
    {
        layerBackup ??= new List<JsonData>();
        layerBackup.Add(currentLayer);
    }

    private JsonData GetCurrentLayer()
    {
        return layerBackup[layerBackup.Count - 1];
    }

    private void GoPreviousLayer()
    {
        layerBackup.Remove(layerBackup[layerBackup.Count - 1]);
    }

    #endregion
}