using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public Image satellite;
    public Image faction;
    public Image unit1;
    public Image unit2;
    public Image unit3;

    public TextMeshProUGUI blurb;
    public TextMeshProUGUI win;

    void Start() {
        UnitCollection unitCollection = UnitCollection.GetSingleton();

        switch (PlayerPrefs.GetString("Winner")) {
            case "Insect":
                unit1.sprite = unitCollection.insectFaction[0].GetComponent<Character>().sprite;
                unit2.sprite = unitCollection.insectFaction[1].GetComponent<Character>().sprite;
                unit3.sprite = unitCollection.insectFaction[2].GetComponent<Character>().sprite;

                satellite.sprite = Resources.Load<Sprite>("Sprites/insectcapwin");

                blurb.text = "The Insect faction have successfully taken over this Psionic Satellite and now have access to the vital resources the ruined city brings. The road to surviving the apocalypse's unnatural selection is still long and perilous, but for now...";
                win.text = $"The Insects win!";
                break;
            case "Mech":
                unit1.sprite = unitCollection.mechFaction[0].GetComponent<Character>().sprite;
                unit2.sprite = unitCollection.mechFaction[1].GetComponent<Character>().sprite;
                unit3.sprite = unitCollection.mechFaction[2].GetComponent<Character>().sprite;

                satellite.sprite = Resources.Load<Sprite>("Sprites/mechcapwin");

                blurb.text = "The Mech faction have successfully taken over this Psionic Satellite and now have access to the vital resources the ruined city brings. The road to surviving the apocalypse's unnatural selection is still long and perilous, but for now...";
                win.text = $"The Mechs win!";
                break;
            case "Rock":
                unit1.sprite = unitCollection.rockFaction[0].GetComponent<Character>().sprite;
                unit2.sprite = unitCollection.rockFaction[1].GetComponent<Character>().sprite;
                unit3.sprite = unitCollection.rockFaction[2].GetComponent<Character>().sprite;

                satellite.sprite = Resources.Load<Sprite>("Sprites/rockcapwin");

                blurb.text = "The Rock faction have successfully taken over this Psionic Satellite and now have access to the vital resources the ruined city brings. The road to surviving the apocalypse's unnatural selection is still long and perilous, but for now...";
                win.text = $"The Rocks win!";
                break;
            case "Shadow":
                unit1.sprite = unitCollection.shadowFaction[0].GetComponent<Character>().sprite;
                unit2.sprite = unitCollection.shadowFaction[1].GetComponent<Character>().sprite;
                unit3.sprite = unitCollection.shadowFaction[2].GetComponent<Character>().sprite;

                satellite.sprite = Resources.Load<Sprite>("Sprites/shadowcapwin");

                blurb.text = "The Shadow faction have successfully taken over this Psionic Satellite and now have access to the vital resources the ruined city brings. The road to surviving the apocalypse's unnatural selection is still long and perilous, but for now...";
                win.text = $"The Shadows win!";
                break;
        }
    }

    public void ResetGame() {
        if (UnitCollection.GetSingleton()) {
            Destroy(UnitCollection.GetSingleton().gameObject);
        }
        SceneManager.LoadScene(0);
    }
}
