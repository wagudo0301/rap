using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;   //Windowsの音声認識で使用
using System.Threading;    //非同期処理で使用
using System.Threading.Tasks;
public class sample_battle_gamemanager : MonoBehaviour
{
    bool recognizer_complete;
    DictationRecognizer dictationRecognizer;
    string ans;
    string fullans;
    float timer;

    float num;
    private Queue<char> _charQueue;

    public float PlayerPointOfRap=0;
    public float OpponentPointOfRap=0;
    private float RateOfTurn;
    private sample_ScoreController MyScoreController;
    private string stringPlayerPointOfRap="0";
    private string stringOpponentPointOfRap="0";
    // SerializeFieldと書くとprivateなパラメーターでも
    // インスペクター上で値を変更できる

    [SerializeField] 
    private AudioSource audioSource;
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
    // Start is called before the first frame update
    void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds  = 40f;
        dictationRecognizer.AutoSilenceTimeoutSeconds  = 40f;

        //熊井のスコアの設定
        kumai_point.text = stringPlayerPointOfRap;
        //先生のスコアの設定
        mashiro_point.text = stringOpponentPointOfRap;

        hpSlider.value = 0.5f;
        StartCoroutine("GameManagerCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        if(recognizer_complete)
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


    /**
    * 敵のテキストを表示する
    */
    private void ReadLine(string text, float captionSpeed)
    {
        string main = text;
        mainText.text = "";
        _charQueue = SeparateString(main);
        // コルーチンを呼び出す
        StartCoroutine(ShowChars(captionSpeed));
    }



    //音声認識してる最中に行う関数
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
        if(recognizer_complete)mainText.text = text;
        else mainText.text = "";

    }



    //DictationComplete：音声認識セッションを終了したときにトリガされるイベント
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause){
        Debug.Log("音声認識完了");
    }



    //音声認識を終了してまた音声認識が使えるようにインスタンスを生成する
    IEnumerator init_recognition()
    {
        dictationRecognizer.Stop();
        yield return new WaitForSeconds(1.5f);
        Debug.Log(fullans);
        float num=GameObject.Find("RapJudger").GetComponent<RapJudger>().JudgeRap(fullans);
        Debug.Log(num+"*"+RateOfTurn+"加算");
        PlayerPointOfRap+=num*RateOfTurn;
        stringPlayerPointOfRap = PlayerPointOfRap.ToString();
        kumai_point.text = stringPlayerPointOfRap;
        hpSlider.value = 1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
        ans = "";
        fullans = "";
        Debug.Log("非同期処理終了");
    }



    //音楽が始まった後の先生のターンの関数
    IEnumerator InitMashiroTurn(string talk, float waittime)
    {
        //先生のターンの表示
        TurnPannel.sprite = mashiro_turn;

        //先生のテキストパネルの表示
        TextPannel.sprite = mashirotextpannel;

        //テキストを緑にする
        mainText.color = new Color(0.0f, 255.0f, 0.0f, 1.0f);

        //熊井の立ち絵と先生の立ち絵の変更
        mashiro.sprite = mashiro_mini_microphone;

        ReadLine(talk, waittime);
        //1秒停止
        yield return new WaitForSeconds(1);

        //透過画像の表示
        TurnPannel.sprite = UIMask;
    }



    //先生のターンに変わる瞬間の関数
    IEnumerator ChangeMashiroTurn(string talk, float waittime)
    {
        //先生のターンの表示
        TurnPannel.sprite = mashiro_turn;

        //1秒停止
        yield return new WaitForSeconds(0.8f);

        //透過画像の表示
        TurnPannel.sprite = UIMask;

        //先生のテキストパネルの表示
        TextPannel.sprite = mashirotextpannel;

        //テキストを緑にする
        mainText.color = new Color(0.0f, 255.0f, 0.0f, 1.0f);

        //熊井の立ち絵と先生の立ち絵の変更
        mashiro.sprite = mashiro_mini_microphone;
        kumai.sprite = kumai_mini;

        ReadLine(talk, waittime);
    }


    //先生のターンの後半の最初で行う関数
    private void Enemy2(string talk, float waittime)
    {
        stringOpponentPointOfRap = OpponentPointOfRap.ToString();
        mashiro_point.text = stringOpponentPointOfRap;
        ReadLine(talk, waittime);
    }

    //熊井のターンに変わる瞬間の関数
    IEnumerator ChangeKumaiTurn()
    {
        //認識した文字をテキストに表示する機能をオンにする
        recognizer_complete = true;
        //先生のポイントの加算
        stringOpponentPointOfRap = OpponentPointOfRap.ToString();

        //先生のポイント表示
        mashiro_point.text = stringOpponentPointOfRap;

        //テキストをリセットする
        mainText.text = "";

        //yourターンの表示
        TurnPannel.sprite = kumai_turn;

        //音声認識開始
        dictationRecognizer.Start();

        //1秒停止
        yield return new WaitForSeconds(1);

        //透過画像の表示
        TurnPannel.sprite = UIMask;

        //熊井のテキストパネルの表示
        TextPannel.sprite = kumaitextpannel;

        //テキストを赤にする
        mainText.color = new Color(255.0f, 0.0f, 0.0f, 1.0f);

        //熊井の立ち絵と先生の立ち絵の変更
        kumai.sprite = kumai_mini_microphone;
        mashiro.sprite = mashiro_mini;
    }


    //最後の点数の処理
    IEnumerator LastPointCalculation()
    {
        //最後の点数計算
        RateOfTurn=2f;
        StartCoroutine("init_recognition");
        //画面上の吹き出しの文字列削除
        mainText.text = "";
        //テキストパネルの削除
        TextPannel.sprite = UIMask;
        //3.5秒停止
        yield return new WaitForSeconds(2f);

        SceneLoad();
    }

    //勝ち負けの結果を表示するシーンをロードする
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



    //ゲームをコルーチンを使って制御する
    IEnumerator GameManagerCoroutine()
    {
        //アニメーションの影響で音楽の開始をずらす
        yield return new WaitForSeconds(0.3f);

        //音楽を再生する
        audioSource.Play();

        //音楽の間の部分の待機時間
        yield return new WaitForSeconds(2.7f);

        //ターン1(先生-1)
        StartCoroutine(InitMashiroTurn("お手並み拝見小テスト　見せてみなさいキミのベスト　聞き届けるはこの担任　キミを育てるそのために", 0.21f));
        yield return new WaitForSeconds(11f);

        //ターン1(先生-2)
        OpponentPointOfRap += 300;
        hpSlider.value = 0.55f;
        Enemy2("対するキミは何者さ　聞かせて見せなYourアンサー　まずは元気にレッツトライ　チャレンジするならザッツオーライ", 0.18f);
        yield return new WaitForSeconds(10.1f);

        //ターン1(熊井-1,2)
        OpponentPointOfRap += 300;
        hpSlider.value = 0.65f;
        StartCoroutine("ChangeKumaiTurn");
        yield return new WaitForSeconds(22.4f);

        //ターン2(先生-1)
        recognizer_complete = false;
        RateOfTurn  = 1.0f;
        StartCoroutine("init_recognition");
        StartCoroutine(ChangeMashiroTurn("夢見てるのなら一つ言わせろ　調子は良いけど経験はゼロ　ラップにおいてはキミは無知　だから打つのよ愛のムチ", 0.17f));
        yield return new WaitForSeconds(11.2f);

        //ターン2(先生-2)
        OpponentPointOfRap += 750;
        hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
        Enemy2("大人の美と　言葉のビート　叩きつけるは　言霊のビーム　おじけづいてないわよね？　＃ComeOn BeatMe", 0.18f);
        yield return new WaitForSeconds(10.4f);

        //ターン2(熊井-1,2)
        OpponentPointOfRap += 750;
        hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
        StartCoroutine("ChangeKumaiTurn");
        yield return new WaitForSeconds(22.4f);

        //ターン3(先生-1)
        recognizer_complete = false;
        RateOfTurn=1.5f;
        StartCoroutine("init_recognition");
        StartCoroutine(ChangeMashiroTurn("確かに少しはやるようね　チェックしとくわYourName-弱音をはかないその威勢　姿勢　すでに　合格点よ", 0.18f));
        yield return new WaitForSeconds(11.3f);

        //ターン3(先生-2)
        OpponentPointOfRap += 500;
        hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
        Enemy2("後はキミの覚悟次第　格上いても倒したい？　覚悟がないなら直ちにカエるか　エイリアンにも立ち向かえるか", 0.2f);
        yield return new WaitForSeconds(10.3f);

        //ターン3(熊井-1)
        recognizer_complete = true;
        OpponentPointOfRap += 500;
        hpSlider.value=1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
        StartCoroutine("ChangeKumaiTurn");
        yield return new WaitForSeconds(23f);

        //結果のシーンをロードする
        recognizer_complete = false;
        StartCoroutine("LastPointCalculation");
        yield return new WaitForSeconds(5f);
    }
}
