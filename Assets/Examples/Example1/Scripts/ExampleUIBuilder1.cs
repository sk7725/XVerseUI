using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTown.UI;

public class ExampleUIBuilder1 : MonoBehaviour {
    void Start() {
        Canvas main = FindObjectOfType<Canvas>();

        var one = XButton.New("One", () => Debug.Log("1"));
        one.Get().Up().Left().SetScene(main);

        var two = XButton.New("Two", () => Debug.Log("2"));
        two.Get().Center().FillX().SetScene(main);

        var three = XButton.New("Three", () => Debug.Log("3"));
        three.Get().Right().FillY().SetScene(main);

        Label.New("XTOWN").Get().Left().Down().SetScene(main);

        Label.New("UI").Get().Down().Right().SetScene(main);
    }
}
