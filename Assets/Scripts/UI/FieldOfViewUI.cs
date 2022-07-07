using UnityEngine;
using UnityEngine.UI;

public class FieldOfViewUI : MonoBehaviour {

    public Image prefabUI;
    private Image uiUse;
    private bool existingUI;
    public StateController tankState;
    public bool chaseTargetIsActive;

    void Start() 
    {
        existingUI = false;
        tankState = GetComponent<StateController>();
        chaseTargetIsActive = false;
    }

    void Update()
    {
        // check if enemy has entered chasesequence
            // not implemented successfully

        //chaseTargetIsActive = state.currentState;
            //if (chaseTargetIsActive & !existingUI){
            //} else if (chaseTargetIsActive & existingUI){
        if (!existingUI)
        {
            uiUse = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
            existingUI = true;
        }
        uiUse.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        
    }
}