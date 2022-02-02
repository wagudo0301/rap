using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;   //Windowsの音声認識で使用
using System.Threading;    //非同期処理で使用
using System.Threading.Tasks;

public class battle_gamemanager : MonoBehaviour
{
    DictationRecognizer dictationRecognizer;
    string ans;
    string fullans;
    float timer;

    float num;
    private Queue<char> _charQueue;
    private int count = 0;
    private float starttime = 8f;
    private float voice_lag = 0f;

    public float PlayerPointOfRap=0;
    public float OpponentPointOfRap=0;
    private float RateOfTurn=1.0f;
    private ScoreController MyScoreController;
    private string stringPlayerPointOfRap="0";
    private string stringOpponentPointOfRap="0";
    // SerializeFieldと書くとprivateなパラメーターでも
    // インスペクター上で値を変更できる
    [SerializeField]
    private Text mainText;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Text kumai_point;
    [SerializeField]
    private Text mashiro_point;

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

        MyScoreController=GameObject.Find("ScoreController").GetComponent<ScoreController>();
        //float maxHp = 100f;
        //float nowHp = 50f;
 
 
        //スライダーの最大値の設定
        //hpSlider.maxValue = maxHp;
 
        //スライダーの現在値の設定
        //hpSlider.value = nowHp;

        //熊井のスコアの設定
        kumai_point.text = stringPlayerPointOfRap;
        //先生のスコアの設定
        mashiro_point.text = stringOpponentPointOfRap;

        hpSlider.value = 0.5f;
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
                    starttime = 0;
                    break;
                case 1:
                    OpponentPointOfRap+=75;
                    MyScoreController.OpponentPointOfRap+=75;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value = 0.55f;
                    ReadLine("対するキミは何者さ　聞かせて見せなYourアンサー　まずは元気にレッツトライ　チャレンジするならザッツオーライ", 0.18f);
                    voice_lag = 0.8f;
                    break;
                case 2:
                    OpponentPointOfRap+=75;
                    MyScoreController.OpponentPointOfRap+=75;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value = 0.6f;
                    //ReadLine("僕の名前は熊井だよ　イカしたビートつなぎあお　ラップは初心者からスタート　経験つんで頑張った後", 0.21f);
                    mainText.text = "";
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    Debug.Log("音声認識開始");
                    //hpSlider.value += -10f;
                    voice_lag = -1f;
                    break;
                case 3:
                    //ReadLine("スターと　なって皆を夢中に　宇宙にまで届けるビート　かなで行こうぜ高鳴る世界と　Rappy Box楽しまないと", 0.18f);
                    //hpSlider.value += -10f;
                    voice_lag = 1f;
                    break;
                case 4:
                    fullans += mainText.text;
                    TextPannel.sprite = mashirotextpannel;
                    mainText.color = new Color(0.0f, 255.0f, 0.0f, 1.0f);
                    dictationRecognizer.Stop();

                    RateOfTurn=1.0f;
                    MyScoreController.RateOfTurn=1.0f;
                    init_recognition();

                    ReadLine("夢見てるのなら一つ言わせろ　調子は良いけど経験はゼロ　ラップにおいてはキミは無知　だから打つのよ愛のムチ", 0.18f);
                    voice_lag = 0;
                    break;
                case 5:
                    OpponentPointOfRap+=100;
                    MyScoreController.OpponentPointOfRap+=100;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    ReadLine("大人の美と　言葉のビート　叩きつけるは　言霊のビーム　おじけづいてないわよね？　＃ComeOn BeatMe", 0.18f);
                    voice_lag = 0.8f;
                    break;
                case 6:
                    OpponentPointOfRap+=100;
                    MyScoreController.OpponentPointOfRap+=100;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    //ReadLine("現状だけのディスは嫌い　どうせ描くなら輝く未来　わけぇしこれからレッツトライ　チャレンジするなら結果オーライ", 0.18f);
                    mainText.text = "";
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    Debug.Log("音声認識開始");
                    //hpSlider.value += -10f;
                    voice_lag = -1f;
                    break;
                case 7:
                    //ReadLine("そうだろ　だって初心者の僕に　現状がどうこうって元も子もない話じゃん　描いた未来を引き寄せる　。。。#それがラッピーだろ", 0.17f);
                    //hpSlider.value += -15f;
                    voice_lag = 1f;
                    break;
                case 8:
                    fullans += mainText.text;
                    TextPannel.sprite = mashirotextpannel;
                    mainText.color = new Color(0.0f, 255.0f, 0.0f, 1.0f);
                    dictationRecognizer.Stop();

                    RateOfTurn=1.5f;
                    MyScoreController.RateOfTurn=1.5f;
                    init_recognition();

                    ReadLine("確かに少しはやるようね　チェックしとくわYourName-弱音をはかないその威勢　姿勢　すでに　合格点よ", 0.18f);
                    //hpSlider.value += 20f;
                    voice_lag = 0f;
                    break;
                case 9:
                    OpponentPointOfRap+=125;
                    MyScoreController.OpponentPointOfRap+=125;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    ReadLine("後はキミの覚悟次第　格上いても倒したい？　覚悟がないなら直ちにカエるか　エイリアンにも立ち向かえるか", 0.2f);
                    voice_lag = 0.8f;
                    break;
                case 10:
                    OpponentPointOfRap+=125;
                    MyScoreController.OpponentPointOfRap+=125;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    mainText.text = "";
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    //hpSlider.value += -20f;
                    voice_lag = -1f;
                    break;
                case 11:
                    //hpSlider.value += -40f;
                    voice_lag = 1f;
                    break;
                case 12:
                    fullans += mainText.text;
                    TextPannel.sprite = mashirotextpannel;
                    mainText.color = new Color(0.0f, 255.0f, 0.0f, 1.0f);
                    dictationRecognizer.Stop();

                    RateOfTurn=1.5f;
                    MyScoreController.RateOfTurn=1.5f;
                    init_recognition();
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
        Debug.Log("aaa");
        MyScoreController.Rap=st;
        MyScoreController.WillChangeScore=true;
        /*float num=GameObject.Find("RapJudger").GetComponent<RapJudger>().JudgeRap(st);
        PlayerPointOfRap+=num;
        stringPlayerPointOfRap = PlayerPointOfRap.ToString();
        kumai_point.text = stringPlayerPointOfRap;
        hpSlider.value = 1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));*/
        dictationRecognizer.Dispose(); 
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds  = 24f;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 24f;
        ans = "";
        fullans = "";
        Debug.Log("非同期処理終了");
    }
}
