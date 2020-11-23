using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {
    int damage;
    public float timeONScreen = 0.25f;
    int frames = 50;
    float verticalRise = 36; // In pixels

    public void SetDamage(int damage) {
        this.damage = damage;
        StartCoroutine("FadeAway");
    }

    IEnumerator FadeAway() {
        GetComponent<Text>().text = "-" + damage;
        for (int i = 0; i < frames; i++) {
            //Become more transparent
            Color c = GetComponent<Text>().color;
            c.a = (float)(frames - i) / frames;
            GetComponent<Text>().color = c;


            //Rise a little
            transform.position += new Vector3(0, (verticalRise / frames), 0);

            //Wait
            yield return new WaitForSeconds(timeONScreen / frames);
        }

        //Go away
        Destroy(gameObject);
    }
}
