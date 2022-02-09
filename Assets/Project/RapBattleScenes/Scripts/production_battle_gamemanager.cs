using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;   //Windowsの音声認識で使用
using System.Threading;    //非同期処理で使用
using System.Threading.Tasks;
using NMeCab.Specialized;
public class production_battle_gamemanager : MonoBehaviour
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
    private string stringPlayerPointOfRap="0";
    private string stringOpponentPointOfRap="0";


    //韻踏みシステムの定義
    private const float PointScale=10.0f;
    private const int JudgeVowelRange=10;
    private const float NumberImportanceScale=0.3f;
    private const float RhymeImportanceScale=1.7f;//累乗される
    private const float ConsonantImportanceScale=1.6f;//0か1かこれ
    
    List<string> ls20= new List<string>() {"ア","カ","キャ","クァ","ガ","ギャ","グァ","サ","シャ","ザ","ジャ","タ","チャ","ダ","ジャ","デャ","ツァ","テャ","ナ","ニャ","ハ","ヒャ","バ","ビャ","パ","ピャ","ファ","フャ","マ","ミャ","ヤ","ラ","リャ","ワ","ヴァ"};
    List<string> ls21= new List<string>() {"イ","キ","キィ","ギ","ギィ","シ","シィ","ジ","ジィ","チ","チィ","ヂ","ヂィ","ディ","ツィ","ティ","ニ","ニィ","ヒ","ヒィ","ビ","ビィ","ピ","ピィ","フィ","ミ","ミィ","リ","リィ","ウィ","ヴィ"};
    List<string> ls22= new List<string>() {"ウ","ク","キュ","グ","ギュ","ス","シュ","ズ","ジュ","ツ","チュ","トゥ","ヅ","ヂュ","ドゥ","デュ","テュ","ヌ","ニュ","フ","ヒュ","ブ","ビュ","プ","ピュ","フュ","ム","ミュ","ユ","ル","リュ","ヴ"};
    List<string> ls23= new List<string>() {"エ","ケ","キェ","ゲ","ギェ","セ","シェ","ゼ","ジェ","テ","チェ","デ","ヂェ","デェ","ツェ","テェ","ネ","ニェ","ヘ","ヒェ","ベ","ビェ","ペ","ピェ","フェ","メ","ミェ","レ","リェ","ウェ","ヴェ"};
    List<string> ls24= new List<string>() {"オ","コ","キョ","ゴ","ギョ","ソ","ショ","ゾ","ジョ","ト","チョ","ド","ヂョ","デョ","ツォ","テョ","ノ","ニョ","ホ","ヒョ","ボ","ビョ","ポ","ピョ","フォ","フョ","モ","ミョ","ヨ","ロ","リョ","ヲ","ヴォ"};

    //↑ここまで


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
    * ↓ここからノベルゲーム式文字列表示の関数
    */


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


    /**
    * ↑ここまでノベルゲーム式文字列表示の関数
    */


    /**
    * ↓ここから韻踏みシステムの関数
    */

    //ラップ判定
    public float JudgeRap(string st00)
    {
        float PointOfRap=0;
        List<(string,string)> ls00= new List<(string,string)>();
        ls00 = SentenceToWords(st00);
        for(var i = 0; i < ls00.Count; i++)
        {
            for(var j = i+1; j < ls00.Count; j++)
            {
                if(Mathf.Abs(i-j)<=JudgeVowelRange)//距離がJudgeVowelRange以内で、自分と違う形態素のみ韻を踏んでるか判断する
                {
                    if(JudgeMaxVowelCombo(ls00[i].Item2,ls00[j].Item2)>=2)
                    {
                        Debug.Log($"{i}{ls00[i].Item2}{j}{ls00[j].Item2}");
                        Debug.Log("韻の数は"+JudgeMaxVowelCombo(ls00[i].Item2,ls00[j].Item2)+"で、点は"+Mathf.Pow(RhymeImportanceScale,JudgeMaxVowelCombo(ls00[i].Item2,ls00[j].Item2)));
                        PointOfRap+=Mathf.Pow(RhymeImportanceScale,JudgeMaxVowelCombo(ls00[i].Item2,ls00[j].Item2));
                    }
                }
            }
        }
        Debug.Log("韻は"+PointOfRap);
        Debug.Log("文字数の得点"+ls00.Count*0.8f);
        PointOfRap+=ls00.Count*0.8f;//形態素の数だけ得点
        Debug.Log("合計"+PointOfRap);
        return(Mathf.Round(PointOfRap)*PointScale);
    }

    //文からカタカナ形態素
    List<(string,string)> SentenceToWords(string st01)
    {
        //重複してないリスト
        List<string> NotDuplicateList = new List<string>();
        //単語と読みセットのリスト
        List<(string,string)> ls01= new List<(string,string)>();

        string m_Path = Application.dataPath;

        //var dicDir = @"Assets/Plugins/NMeCab-0.10.2/dic/ipadic";
        var dicDir = m_Path + "/StreamingAssets/NMeCab-0.10.2/dic/ipadic";

        using (var tagger = MeCabIpaDicTagger.Create(dicDir))
        {
            var nodes = tagger.Parse(st01);

            foreach (var item in nodes)
            {
                if(NotDuplicateWord(NotDuplicateList, item.Surface))
                {
                    ls01.Add((item.Surface,item.Reading));
                    Debug.Log(item.Surface);
                }
            }
        }
        return(ls01);
    }


    //既にリストにある単語と今入ってきた単語を比べて同じかどうか判定する関数

    bool NotDuplicateWord(List<string> NotDuplicateList, string word)
    {
        for(var i = 0; i < NotDuplicateList.Count; i++ )
        {
            if(word == NotDuplicateList[i])
            {
                return false;
            }
        }
        NotDuplicateList.Add(word);
        return true;
    }


    //ライム判定
    float JudgeMaxVowelCombo(string st10,string st11)//韻を踏める最大文字数
    {
        float MaxVowelCombo=0f;
        List<string> ls10= new List<string>();
        List<string> ls11= new List<string>();
        //カタカナ形態素を前処理して音ごとに分ける
        foreach(char ch10 in st10)
        {
            ls10.Add(ch10.ToString());
        }
        foreach(char ch11 in st11)
        {
            ls11.Add(ch11.ToString());
        }
        for(int i = ls10.Count - 1; i >= 0; i--)
        {
            if((ls10[i]=="ッ")||(ls10[i]=="ー")||(ls10[i]=="ン"))
            {
                ls10.RemoveAt(i);
            }
            else if((ls10[i]=="ァ")||(ls10[i]=="ィ")||(ls10[i]=="ゥ")||(ls10[i]=="ェ")||(ls10[i]=="ォ")||(ls10[i]=="ャ")||(ls10[i]=="ュ")||(ls10[i]=="ョ"))
            {
                if(i>=1){ls10[i-1]+=ls10[i];}
                ls10.RemoveAt(i);
            }
        }
        for(int i = ls11.Count - 1; i >= 0; i--)
        {
            if((ls11[i]=="ッ")||(ls11[i]=="ー")||(ls11[i]=="ン"))
            {
                ls11.RemoveAt(i);
            }
            else if((ls11[i]=="ァ")||(ls11[i]=="ィ")||(ls11[i]=="ゥ")||(ls11[i]=="ェ")||(ls11[i]=="ォ")||(ls11[i]=="ャ")||(ls11[i]=="ュ")||(ls11[i]=="ョ"))
            {
                if(i>=1){ls11[i-1]+=ls11[i];}
                ls11.RemoveAt(i);
            }
        }
        //ls10をもって動かす
        for(var i = 0; i < ls10.Count; i++)
        {
            for(var j = 0; j < ls11.Count; j++)
            {
                if(CountVowelCombo(ls10,ls11,i,j)>MaxVowelCombo)
                {
                    MaxVowelCombo=CountVowelCombo(ls10,ls11,i,j);
                }
            }
        }
        //Debug.Log(st10+","+st11+"韻は"+MaxVowelCombo);
        return(MaxVowelCombo);
    }
    float CountVowelCombo(List<string> ls12,List<string> ls13,int num10,int num11)
    {
        float VowelCombo=0;
        int num12=num10;
        int num13=num11;
        while(JudgeVowel(ls12[num12],ls13[num13])>=1)
        {
            VowelCombo+=1;//VowelCombo+=JudgeVowel(ls11[i],ls10[j]);
            if(num12+1<ls12.Count&&num13+1<ls13.Count)
            {
                num12+=1;num13+=1;
            }
            else
            {
                break;
            }
        }
        return(VowelCombo);
    }


    //母音判定
    float JudgeVowel(string st20,string st21)//0,1(,x)を返す
    {
        if((ls20.Contains(st20)&&ls20.Contains(st21))||(ls21.Contains(st20)&&ls21.Contains(st21))||(ls22.Contains(st20)&&ls22.Contains(st21))||(ls23.Contains(st20)&&ls23.Contains(st21))||(ls24.Contains(st20)&&ls24.Contains(st21)))
        {
            return(1);
        }
        else
        {
            return(0);
        }
    }


    /**
    * ↑ここまで韻踏みシステムの関数
    */


    /**
    * ↓ここから音声認識の関数
    */

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
        yield return new WaitForSeconds(3f);
        Debug.Log(fullans);
        float num = JudgeRap(fullans);
        Debug.Log(num+"*"+RateOfTurn+"加算");
        PlayerPointOfRap+=num*RateOfTurn;
        stringPlayerPointOfRap = PlayerPointOfRap.ToString();
        kumai_point.text = stringPlayerPointOfRap;
        hpSlider.value = 1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
        ans = "";
        fullans = "";
        Debug.Log("非同期処理終了");
    }


    /**
    * ↑ここまで音声認識の関数
    */


    /**
    * ↓ここからゲーム自体を制御する関数
    */


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
        //2秒停止
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


    /**
    * ↑ここまでゲーム自体を制御する関数
    */


    //ゲーム本体をコルーチンを使って制御する
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
