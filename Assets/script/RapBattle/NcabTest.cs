using NMeCab.Specialized;
using UnityEngine;

public class NcabTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        string sentence = "Unityで初めての形態素解析をしてつなぎあお";

        // 「dic/ipadicフォルダ」のパスを指定する
        var dicDir = @"Assets/Plugins/NMeCab-0.10.2/dic/ipadic";

        using (var tagger = MeCabIpaDicTagger.Create(dicDir))
        {
            var nodes = tagger.Parse(sentence);

            foreach (var item in nodes)
                Debug.Log($"{item.Surface}, {item.PartsOfSpeech}, {item.PartsOfSpeechSection1}, {item.PartsOfSpeechSection2}, {item.Reading}");
        }
    }
}
