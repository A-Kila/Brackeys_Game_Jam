using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class OptionsManager : MonoBehaviour {
    
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;

    private GameObject[] keybindButtons;
    private string bindToChange = string.Empty;

    void Start() {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        foreach (GameObject keybind in keybindButtons) {
            TextMeshProUGUI text = keybind.GetComponentInChildren<TextMeshProUGUI>();
            GameObject button = keybind.transform.GetChild(1).gameObject;
            button.GetComponentInChildren<TextMeshProUGUI>().text = GetKeybind(text.text).ToString();
        }

        float musicVolume, sfxVolume;
        audioMixer.GetFloat("MusicVolume", out musicVolume);
        audioMixer.GetFloat("SfxVolume", out sfxVolume);

        musicSlider.value = Mathf.Pow(10f, musicVolume / 20f);
        sfxSlider.value = Mathf.Pow(10f, sfxVolume / 20f);
    }

    public void MusicSlider(float volume) {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);    
    }

    public void SFXSlider(float volume) {
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
    }

    public void Select() { bindToChange  = "Select"; }
    public void Move() { bindToChange  = "Move"; }
    public void StartShooting() { bindToChange  = "Start Shooting"; }
    public void StopShooting() { bindToChange  = "Stop Shooting"; }
    public void CombineGroups() { bindToChange  = "Combine Groups"; }
    public void DivideGroups() { bindToChange  = "Divide Groups"; }
    public void TargetSelection() { bindToChange  = "Target Selection"; }
    public void Upgrade() { bindToChange  = "Upgrade"; }

    /* Options pricate methods */
    private KeyCode GetKeybind(string action) {
        KeyCode result = KeyCode.None;
        switch (action) {
            case "Select": 
                result = MyInput.select;
                break;
            case "Move": 
                result = MyInput.move;
                break;
            case "Start Shooting": 
                result = MyInput.startShoot;
                break;
            case "Stop Shooting": 
                result = MyInput.stopShoot;
                break;
            case "Combine Groups": 
                result = MyInput.combineGroups;
                break;
            case "Divide Groups": 
                result = MyInput.divideGroups;
                break;
            case "Target Selection": 
                result = MyInput.targetSelect;
                break;
            case "Upgrade": 
                result = MyInput.upgrade;
                break;
        }
        return result;
    }

    private void ChangeKeybind(string action, KeyCode newKey) {
        switch (action) {
            case "Select": 
                MyInput.select = newKey;
                break;
            case "Move": 
                MyInput.move = newKey;
                break;
            case "Start Shooting": 
                MyInput.startShoot = newKey;
                break;
            case "Stop Shooting": 
                MyInput.stopShoot = newKey;
                break;
            case "Combine Groups": 
                MyInput.combineGroups = newKey;
                break;
            case "Divide Groups": 
                MyInput.divideGroups = newKey;
                break;
            case "Target Selection": 
                MyInput.targetSelect = newKey;
                break;
            case "Upgrade": 
                MyInput.upgrade = newKey;
                break;
        }
    }

    void OnGUI() {
        if (bindToChange != string.Empty) { 
            Event e = Event.current;

            if (e.isKey) {
                AssignNewKey(bindToChange, e.keyCode);
                bindToChange = string.Empty;
            }
        }
    }

    void AssignNewKey(string name, KeyCode newKey) {
        ChangeKeybind(name, newKey);

        foreach (GameObject keybind in keybindButtons) {
            TextMeshProUGUI text = keybind.GetComponentInChildren<TextMeshProUGUI>();
            if (text.text == name) { 
                GameObject button = keybind.transform.GetChild(1).gameObject;
                button.GetComponentInChildren<TextMeshProUGUI>().text = newKey.ToString();
            }
        }
    }

}
