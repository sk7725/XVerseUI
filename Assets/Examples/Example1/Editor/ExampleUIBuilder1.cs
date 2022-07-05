using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XVerse.UI;

public class ExampleUIBuilder1 : MonoBehaviour, ICompileUI {
    public Sprite backSprite;
    public void Build(Canvas main) {

        var one = XButton.New("One", () => Debug.Log("1"));
        one.Get().Up().Left().SetScene(main);

        var two = XButton.New("Two", () => Debug.Log("2"));
        two.Get().Center().Width(300).SetScene(main);

        var three = XButton.New("Three", () => Debug.Log("3"));
        three.Get().Right().FillY().SetScene(main);

        Label.New("XTOWN").Color(Color.yellow).Get().Left().Down().SetScene(main);

        Label.New("UI").Get().Down().Right().SetScene(main);

        string[] names = new string[] { "Taegu", "Hanjun", "OverflowwwwwwwwwwwTest" };
        string[] roles = new string[] { "Leader", "Dev", "Dev" };
        var ver = Layouts.Vertical(true);
        ver.Horizontal(t => {
            for (int i = 0; i < names.Length; i++) {
                t.Add(names[i]).Get().Center().Width(170);
            }
        }).Height(60f).Color(Color.blue);
        ver.RawImage().Color(Color.cyan).Get().Height(8).GrowX();
        ver.Horizontal(t => {
            for (int i = 0; i < names.Length; i++) {
                int ii = i;
                t.Button(roles[i], backSprite.texture, 32, () => Debug.Log(names[ii]+"!")).Get().Center().Width(170);
            }
        }).Height(160f);

        ver.Get().Left().SetScene(main);
    }
}