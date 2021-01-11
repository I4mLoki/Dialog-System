using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private string eventName;

    private GameObject player;
    private bool inTrigger;
    private bool dialogLoaded;

    #region Unity Methods

    private void Start()
    {
        player = GameObject.Find("Player");

        if (dialogManager == null)
            dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
    }

    private void Update()
    {
        RunDialog(Input.GetKeyDown(KeyCode.Space));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
            inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
            inTrigger = false;
    }

    #endregion

    #region Dialog Methods

    private void RunDialog(bool keyTrigger)
    {
        if (keyTrigger)
        {
            if (inTrigger && !dialogLoaded)
                dialogLoaded = dialogManager.LoadDialog(eventName);

            if (dialogLoaded)
                dialogLoaded = dialogManager.PrintLine();
        }
    }

    #endregion
}