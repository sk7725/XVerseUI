using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XVerse.UI;

public class ExampleUIBuilder2 : MonoBehaviour {
    public void Build(Canvas main) {
        Background.New().Get().Fill().SetScene(main);
    }
}
