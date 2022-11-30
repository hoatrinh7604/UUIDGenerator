using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;

    [SerializeField] TextMeshProUGUI uuidText;

    [SerializeField] Button copyButton;
    [SerializeField] Button saveButton;

    [SerializeField] Button genButton;

    [SerializeField] DataSaveController saveController;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();

        copyButton.onClick.AddListener(delegate { CopyToClibboard(); });
        saveButton.onClick.AddListener(delegate { SaveToFile(); });

        SaveFileBtn.onClick.AddListener(delegate { SaveToDisk(); });
        fileName_Input.onDeselect.AddListener(delegate { CheckFileName(); });

    }

    private void DropdownitemSelected()
    {
        
    }


    public void OnValueChanged()
    {
        //if(CheckValidate())
        //{
        //    genButton.interactable = true;
        //}
        //else
        //{
        //    genButton.interactable= false;
        //}
    }

    private bool CheckValidate()
    {
        //return text.All(char.IsDigit);
        return true;
    }
    public void Process()
    {
        GennerateUUID();
        EnableFunctionButtons(true);
    }

    public void EnableFunctionButtons(bool isEnable)
    {
        copyButton.interactable = isEnable;
        saveButton.interactable = isEnable;
    }

    private void GennerateUUID()
    {
        Guid result = Guid.NewGuid();
        string resultStr = result.ToString();

        uuidText.text = resultStr;
    }

    private void CopyToClibboard()
    {
        GUIUtility.systemCopyBuffer = uuidText.text;
    }

    [SerializeField] GameObject savePopup;
    [SerializeField] TMP_InputField fileName_Input;
    [SerializeField] Button SaveFileBtn;
    private void SaveToFile()
    {
        savePopup.SetActive(true);
        
    }

    private void CheckFileName()
    {
        if(fileName_Input.text != "")
        {
            SaveFileBtn.interactable = true;
        }
    }

    [SerializeField] ToastController toastController;
    private void SaveToDisk()
    {
        SaveFileBtn.interactable = false;
        savePopup.SetActive(false);
        string path = "";
        var check = saveController.WriteFile(uuidText.text, fileName_Input.text, ref path);

        if (check)
        {
            toastController.SetText(path, 2.5f);
        }
        else
        {
            toastController.SetText("There is something error when save the file!", 1.5f);
        }
        toastController.Toast();
    }

    public void Clear()
    {
        uuidText.text = "";

        SaveFileBtn.interactable = false;
        savePopup.SetActive(false);
        EnableFunctionButtons(false);
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
