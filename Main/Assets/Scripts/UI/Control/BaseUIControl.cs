using UnityEngine;
public class BaseUIControl {
    protected string panelName;
    public BaseUI view;
    public virtual void ShowPanel(System.Object data) {
        GameObject obj=MonoBehaviour.Instantiate(Resources.Load("UI/Panel" + panelName)) as GameObject;
        view = obj.GetComponent<BaseUI>();
        view.OnShow();
    }
    public virtual void HidePanel() {
        view.OnHide();
        MonoBehaviour.Destroy(view.gameObject);
    }
}
