using UnityEngine.UIElements;

public class UIController
{
    protected VisualElement root;
    
    public UIController(VisualElement rootElement)
    {
        root = rootElement;
        CollectElements();
        RegisterCallbacks();
    }
    
    protected virtual void CollectElements() {}
    protected virtual void RegisterCallbacks() {}
}