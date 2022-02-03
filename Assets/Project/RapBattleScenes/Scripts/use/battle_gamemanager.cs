using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private float RateOfTurn;
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
    private Image kumai;
    [SerializeField]
    private Sprite kumai_mini;
    [SerializeField]
    private Sprite kumai_mini_microphone;
    [SerializeField]
    private Image mashiro;
    [SerializeField]
    private Sprite mashiro_mini;
    [SerializeField]
    private Sprite mashiro_mini_microphone;

    [SerializeField]
    private Image TextPannel;

    [SerializeField]
    private Sprite kumaitextpannel;
    [SerializeField]
    private Sprite mashirotextpannel;
    [SerializeField]
    private Image TurnPannel;
    [SerializeField]
    private Sprite kumai_turn;
    [SerializeField]
    private Sprite mashiro_turn;
    [SerializeField]
    private Sprite UIMask;
    [SerializeField]
    private Image ForeGroundPannel;
    float alfa;
    // Start is called before the first frame update
    void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds  = 240f;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 240f;

        MyScoreController=GameObject.Find("ScoreController").GetComponent<ScoreController>();

        //熊井のスコアの設定
        kumai_point.text = stringPlayerPointOfRap;
        //先生のスコアの設定
        mashiro_point.text = stringOpponentPointOfRap;

        hpSlider.value = 0.5f;

        alfa = ForeGroundPannel.color.a;
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
                    //ターン1(先生-1)
                    mashiro.sprite = mashiro_mini_microphone;
                    StartCoroutine("ChangeMashiroTurn");
                    TextPannel.sprite = mashirotextpannel;
                    ReadLine("お手並み拝見小テスト　見せてみなさいキミのベスト　聞き届けるはこの担任　キミを育てるそのために", 0.21f);
                    starttime = 0;
                    break;
                case 1:
                    //ターン1(先生-2)
                    OpponentPointOfRap+=300;
                    MyScoreController.OpponentPointOfRap+=300;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value = 0.55f;
                    ReadLine("対するキミは何者さ　聞かせて見せなYourアンサー　まずは元気にレッツトライ　チャレンジするならザッツオーライ", 0.18f);
                    voice_lag = 0.8f;
                    break;
                case 2:
                    //ターン1(熊井-1)
                    OpponentPointOfRap+=300;
                    MyScoreController.OpponentPointOfRap+=300;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value = 0.6f;
                    mainText.text = "";
                    kumai.sprite = kumai_mini_microphone;
                    mashiro.sprite = mashiro_mini;
                    StartCoroutine("ChangeKumaiTurn");
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    Debug.Log("音声認識開始");
                    voice_lag = -1f;
                    break;
                case 3:
                    //ターン1(熊井-2)
                    voice_lag = 1f;
                    break;
                case 4:
                    //ターン2(先生-1)
                    mashiro.sprite = mashiro_mini_microphone;
                    kumai.sprite = kumai_mini;
                    StartCoroutine("ChangeMashiroTurn");
                    MyScoreController.RateOfTurn=1.0f;
                    init_recognition();
                    ReadLine("夢見てるのなら一つ言わせろ　調子は良いけど経験はゼロ　ラップにおいてはキミは無知　だから打つのよ愛のムチ", 0.18f);
                    voice_lag = 0;
                    break;
                case 5:
                    //ターン2(先生-2)
                    OpponentPointOfRap+=750;
                    MyScoreController.OpponentPointOfRap+=750;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    ReadLine("大人の美と　言葉のビート　叩きつけるは　言霊のビーム　おじけづいてないわよね？　＃ComeOn BeatMe", 0.18f);
                    voice_lag = 0.8f;
                    break;
                case 6:
                    //ターン2(熊井-1)
                    OpponentPointOfRap+=750;
                    MyScoreController.OpponentPointOfRap+=750;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    mainText.text = "";
                    kumai.sprite = kumai_mini_microphone;
                    mashiro.sprite = mashiro_mini;
                    StartCoroutine("ChangeKumaiTurn");
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    Debug.Log("音声認識開始");
                    voice_lag = -1f;
                    break;
                case 7:
                    //ターン2(熊井-2)
                    voice_lag = 1f;
                    break;
                case 8:
                    //ターン3(先生-1)
                    mashiro.sprite = mashiro_mini_microphone;
                    kumai.sprite = kumai_mini;
                    StartCoroutine("ChangeMashiroTurn");
                    MyScoreController.RateOfTurn=1.5f;
                    init_recognition();
                    ReadLine("確かに少しはやるようね　チェックしとくわYourName-弱音をはかないその威勢　姿勢　すでに　合格点よ", 0.18f);
                    voice_lag = 0f;
                    break;
                case 9:
                    //ターン3(先生-2)
                    OpponentPointOfRap+=500;
                    MyScoreController.OpponentPointOfRap+=500;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    ReadLine("後はキミの覚悟次第　格上いても倒したい？　覚悟がないなら直ちにカエるか　エイリアンにも立ち向かえるか", 0.2f);
                    voice_lag = 0.8f;
                    break;
                case 10:
                    //ターン3(熊井-1)
                    OpponentPointOfRap+=500;
                    MyScoreController.OpponentPointOfRap+=500;
                    stringOpponentPointOfRap = OpponentPointOfRap.ToString();
                    mashiro_point.text = stringOpponentPointOfRap;
                    hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
                    mainText.text = "";
                    kumai.sprite = kumai_mini_microphone;
                    mashiro.sprite = mashiro_mini;
                    StartCoroutine("ChangeKumaiTurn");
                    TextPannel.sprite = kumaitextpannel;
                    mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
                    dictationRecognizer.Start();
                    voice_lag = -1f;
                    break;
                case 11:
                    //ターン3(熊井-2)
                    voice_lag = 1f;
                    break;
                case 12:
                    StartCoroutine("LastPointCalculation");
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
        MyScoreController.Rap=st;
        MyScoreController.WillChangeScore=true;

        /*
            ここで外部の関数で寺杣君韻踏みシステムで計算をしてバーを動かすところまでしてある。
        */

        //音声認識で使ったやつを消して新しく音声認識のインスタンスを生成する。
        dictationRecognizer.Dispose(); 
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds  = 240f;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 240f;
        ans = "";
        fullans = "";
        Debug.Log("非同期処理終了");
    }

    //yourturnの表示
    IEnumerator ChangeKumaiTurn()
    {
        //yourターンの表示
        TurnPannel.sprite = kumai_turn;

        //1秒停止
        yield return new WaitForSeconds(1);

        //投下画像の表示
        TurnPannel.sprite = UIMask;
    }


    //mashiroturnの表示
    IEnumerator ChangeMashiroTurn()
    {
        //先生のターンの表示
        TurnPannel.sprite = mashiro_turn;

        //1秒停止
        yield return new WaitForSeconds(1);

        //透過画像の表示
        TurnPannel.sprite = UIMask;
    }

    //最後の点数の処理とフェードアウトの実装
    IEnumerator LastPointCalculation()
    {
        //最後の点数計算
        MyScoreController.RateOfTurn=2f;
        init_recognition();
        //画面上の吹き出しの文字列削除
        mainText.text = "";
        //テキストパネルの削除
        TextPannel.sprite = UIMask;
        //1秒停止
        yield return new WaitForSeconds(2);

        SceneLoad();
    }
    private void SceneLoad()
    {
        PublicStaticStatus.KumaiResultPoint = PlayerPointOfRap;
        PublicStaticStatus.MashiroResultPoint = OpponentPointOfRap;
        if(PlayerPointOfRap >= OpponentPointOfRap)
        {
            SceneManager.LoadScene("WinScenes");
        } 
        else if(PlayerPointOfRap < OpponentPointOfRap)
        {
            SceneManager.LoadScene("LoseScenes");
        }
    }
}
