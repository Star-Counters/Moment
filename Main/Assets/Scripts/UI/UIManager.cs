using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIManager{
    public static UIManager Instance {
        get {
            if (instance == null)
                instance = new UIManager();
            return instance;
        }
    }
    private static UIManager instance;
    public UIManager() {
        uiControlList = new List<BaseUIControl>();
        uiDictionary = new Dictionary<string, BaseUIControl>();
        uiDictionary.Add("Combat", new CombatPanelControl());
        uiDictionary.Add("Chat", new ChatPanelControl());
    }
    List<BaseUIControl> uiControlList;
    Dictionary<string, BaseUIControl> uiDictionary;
    public BaseUIControl GetUIControlByName(string uiControlName) {
        return uiDictionary[uiControlName];
    }
    public void ShowPanel(string panelName,System.Object data) {
        uiDictionary[panelName].ShowPanel(data);
    }
    public void HidePanel(string panelName) {
        uiDictionary[panelName].HidePanel();
    }
}
