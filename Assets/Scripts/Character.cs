using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Color dialogColor;

    public Color GetDialogColor()
    {
        return dialogColor;
    }
}