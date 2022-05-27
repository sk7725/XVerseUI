using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XTown.UI;

public class ExampleUIBuilder1 : MonoBehaviour {
    void Start() {
        Canvas main = FindObjectOfType<Canvas>();

        var one = XButton.New("One", () => Debug.Log("1"));
        one.Get().Up().Left().SetScene(main);

        var two = XButton.New("Two", () => Debug.Log("2"));
        two.Get().Center().Width(300).SetScene(main);

        var three = XButton.New("Three", () => Debug.Log("3"));
        three.Get().Right().FillY().SetScene(main);

        Label.New("XTOWN").Get().Left().Down().SetScene(main);

        Label.New("UI").Get().Down().Right().SetScene(main);

        string[] names = new string[] { "Taegyu", "Hanjun", "SeongH" };
        var ver = LayoutUtils.NewLayout<VerticalLayoutGroup>(200f, 600f);
        for (int i = 0; i < names.Length; i++) {
            Label l = Label.New(names[i]);
            l.Get().Center().FillX();
            ver.Add(l);
        }
        ver.Get().Left().SetScene(main);
    }
}
