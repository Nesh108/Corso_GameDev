using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainView;
    [SerializeField] private GameObject SettingsView;
    [SerializeField] private TMP_Dropdown ResolutionDropdown;

    private Resolution[] _resolutions;
    private List<string> _resolutionOptions = new List<string>();
    private int _currentResolutionIndex = 0;

    private void Start()
    {
        _resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            _resolutionOptions.Add($"{_resolutions[i].width}x{_resolutions[i].height}"); // Formatta la resoluzione: ex. '1920x1080'

            if (_resolutions[i].width.Equals(Screen.currentResolution.width) &&
                _resolutions[i].height.Equals(Screen.currentResolution.height))
            {
                _currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(_resolutionOptions);
        ResolutionDropdown.value = _currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    // Mostra la schermata delle impostazioni
    public void ShowSettingsView()
    {
        MainView.SetActive(false);
        SettingsView.SetActive(true);
    }

    // Mostra la schermata iniziale
    public void ShowMainView()
    {
        MainView.SetActive(true);
        SettingsView.SetActive(false);
    }

    // Esci dal gioco
    public void ExitGame()
    {
        Application.Quit();
    }

    // Imposta la nuova resoluzione
    public void OnResolutionChanged()
    {
        string newSelection = ResolutionDropdown.options[ResolutionDropdown.value].text;
        Debug.Log("New Resolution: " + newSelection);

        // Converti l'opzione da string a 2 integer (width, heigth)
        string[] newResolution = newSelection.Split('x');
        int newWidth = int.Parse(newResolution[0]);
        int newHeight = int.Parse(newResolution[1]);

        Screen.SetResolution(newWidth, newHeight, true);
    }

    // Salva le impostazioni
    public void SaveSettings()
    {

    }
}
