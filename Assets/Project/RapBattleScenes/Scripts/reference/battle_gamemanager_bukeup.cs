using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;   //Windowsの音声認識で使用

public class battle_gamemanager_buckup : MonoBehaviour
{
    DictationRecognizer dictationRecognizer;
    string ans;
    string fullans;
    float timer;
    private Queue<char> _charQueue;
    private int count = 0;
    private float starttime = 8f;
    private float voice_lag = 0f;
    // SerializeFieldと書くとprivateなパラメーターでも
    // インスペクター上で値を変更できる
    [SerializeField]
    private Text mainText;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Image TextPannel;

    [SerializeField]
    private Sprite kumaitextpannel;
    [SerializeField]
    private Sprite mashirotextpannel;
    // Start is called before the first frame update
    void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds  = 24f;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 24f;

        float maxHp = 100f;
        float nowHp = 50f;
 
 
        //スライダーの最大値の設定
        hpSlider.maxValue = maxHp;
 
        //スライダーの現在値の設定
        hpSlider.value = nowHp;
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if(timer > 11f - starttime - voice_lag)
        {
            switch(count)
            {
                case 0:
                    ReadLine("お手並み拝見小テスト　見せてみなさいキミのベスト　聞き届けるはこの担任　キミを育てるそのために", 0.21f);
                    hpSlider.value += 5f;
                    starttime = 0;
                    break;
                case 1:
                    ReadLine("対するキミは何者さ　聞かせて見せなYourアンサー　まずは元気にレッツトライ　チャレンジするならザッツオーライ", 0.18f);
                    hpSlider.value += 10f;
                    voice_lag = 0.8f;
                    break;
                case 2:
                    //ReadLine("僕の名前は熊井だよ　イカしたビートつなぎあお　ラップは初心者からスタート　経験つんで頑張った後", 0.21f);
                    mainText.text = "";
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    Debug.Log("音声認識開始");
                    hpSlider.value += -10f;
                    voice_lag = -1f;
                    break;
                case 3:
                    //ReadLine("スターと　なって皆を夢中に　宇宙にまで届けるビート　かなで行こうぜ高鳴る世界と　Rappy Box楽しまないと", 0.18f);
                    hpSlider.value += -10f;
                    voice_lag = 1f;
                    break;
                case 4:
                    init_recognition();
                    ReadLine("夢見てるのなら一つ言わせろ　調子は良いけど経験はゼロ　ラップにおいてはキミは無知　だから打つのよ愛のムチ", 0.18f);
                    hpSlider.value += 10f;
                    voice_lag = 0;
                    break;
                case 5:
                    ReadLine("大人の美と　言葉のビート　叩きつけるは　言霊のビーム　おじけづいてないわよね？　＃ComeOn BeatMe", 0.18f);
                    hpSlider.value += 20f;
                    voice_lag = 0.8f;
                    break;
                case 6:
                    //ReadLine("現状だけのディスは嫌い　どうせ描くなら輝く未来　わけぇしこれからレッツトライ　チャレンジするなら結果オーライ", 0.18f);
                    mainText.text = "";
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    Debug.Log("音声認識開始");
                    hpSlider.value += -10f;
                    voice_lag = -1f;
                    break;
                case 7:
                    //ReadLine("そうだろ　だって初心者の僕に　現状がどうこうって元も子もない話じゃん　描いた未来を引き寄せる　。。。#それがラッピーだろ", 0.17f);
                    hpSlider.value += -15f;
                    voice_lag = 1f;
                    break;
                case 8:
                    init_recognition();
                    ReadLine("確かに少しはやるようね　チェックしとくわYourName-弱音をはかないその威勢　姿勢　すでに　合格点よ", 0.18f);
                    hpSlider.value += 20f;
                    voice_lag = 0f;
                    break;
                case 9:
                    ReadLine("後はキミの覚悟次第　格上いても倒したい？　覚悟がないなら直ちにカエるか　エイリアンにも立ち向かえるか", 0.2f);
                    hpSlider.value += 20f;
                    voice_lag = 0.8f;
                    break;
                case 10:
                    mainText.text = "";
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    hpSlider.value += -20f;
                    voice_lag = -1f;
                    break;
                case 11:
                    hpSlider.value += -40f;
                    voice_lag = 1f;
                    break;
                case 12:
                    init_recognition();
                    ans = "";
                    fullans = "";
                    break;
            }
            timer = 0;
            Debug.Log(count);
            count++;
        }
        if(count%4 > 1)
        {
            voice_recognition();
        }
}

    /**
    * 文を1文字ごとに区切り、キューに格納したものを返す
    */
    private Queue<char> SeparateString(string str)
    {
        // 文字列をchar型の配列にする = 1文字ごとに区切る
        char[] chars = str.ToCharArray();
        Queue<char> charQueue = new Queue<char>();
        // foreach文で配列charsに格納された文字を全て取り出し
        // キューに加える
        foreach (char c in chars) charQueue.Enqueue(c);
        return charQueue;
    }

    /**
    * 1文字を出力する
    */
    private bool OutputChar()
    {
        // キューに何も格納されていなければfalseを返す
        if (_charQueue.Count <= 0) return false;
        // キューから値を取り出し、キュー内からは削除する
        mainText.text += _charQueue.Dequeue();
        return true;
    }

    /**
    * 文字送りするコルーチン
    */
    private IEnumerator ShowChars(float wait)
    {
        // OutputCharメソッドがfalseを返す(=キューが空になる)までループする
        while (OutputChar())
            // wait秒だけ待機
            yield return new WaitForSeconds(wait);
        // コルーチンを抜け出す
        yield break;
    }

    private void ReadLine(string text, float captionSpeed)
    {
        string main = text;
        mainText.text = "";
        _charQueue = SeparateString(main);
        // コルーチンを呼び出す
        StartCoroutine(ShowChars(captionSpeed));
    }

    private void voice_recognition()
    {
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;//DictationRecognizer_DictationResult処理を行う
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;//DictationRecognizer_DictationHypothesis処理を行う
    }

    //DictationResult：音声が特定の認識精度で認識されたときに発生するイベント
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence){
        if(ans != text)
        {
            fullans += text;
            ans = text;
            mainText.text = "";
        }
    }

    //DictationHypothesis：音声入力中に発生するイベント
    private void DictationRecognizer_DictationHypothesis(string text){
        mainText.text = text;

    }

    //DictationComplete：音声認識セッションを終了したときにトリガされるイベント
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause){
        Debug.Log("音声認識完了");
    }

    //音声認識を終了してまた音声認識が使えるようにインスタンスを生成する
    private void init_recognition()
    {
        fullans += mainText.text;
        TextPannel.sprite = mashirotextpannel;
        mainText.color = new Color(0.0f, 255.0f, 0.0f, 1.0f);
        dictationRecognizer.Stop();
        Debug.Log(fullans);
        string st = fullans;
        float num=GameObject.Find("RapJudger").GetComponent<RapJudger>().JudgeRap(st);
        dictationRecognizer.Dispose(); 
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds  = 24f;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 24f;
        ans = "";
        fullans = "";
    }
}
