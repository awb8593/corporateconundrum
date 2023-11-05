using UnityEngine;

public class PopUp : MonoBehaviour
{
    public string[] variations; // Prefab variations
    public string type;
    public Vector2 location;
    public bool enlarged = false;

    // Constructor
    void Start()
    {
        int randNum = Random.Range(0, variations.Length);
        // this.type = variations[randNum];
        this.location = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
    }

    // Called when a popup is clicked
    public int PopUpClicked()
    {
        if (this.type == "ad")
        {
            return -200;
        }
        else
        {
            return 200;
        }
    }

    // Destroy the popup GameObject
    public void DestroyPopUp()
    {
        Destroy(gameObject);
    }
}