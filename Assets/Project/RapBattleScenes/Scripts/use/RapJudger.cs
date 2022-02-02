using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NMeCab.Specialized;

//韻ふみシステム
public class RapJudger : MonoBehaviour
{
    public string RapSentence;

    const float PointScale=10.0f;
    const int JudgeVowelRange=10;
    const float NumberImportanceScale=0.3f;
    const float RhymeImportanceScale=1.7f;//累乗される
    const float ConsonantImportanceScale=1.6f;//0か1かこれ
    
    List<string> ls20= new List<string>() {"ア","カ","キャ","クァ","ガ","ギャ","グァ","サ","シャ","ザ","ジャ","タ","チャ","ダ","ジャ","デャ","ツァ","テャ","ナ","ニャ","ハ","ヒャ","バ","ビャ","パ","ピャ","ファ","フャ","マ","ミャ","ヤ","ラ","リャ","ワ","ヴァ"};
    List<string> ls21= new List<string>() {"イ","キ","キィ","ギ","ギィ","シ","シィ","ジ","ジィ","チ","チィ","ヂ","ヂィ","ディ","ツィ","ティ","ニ","ニィ","ヒ","ヒィ","ビ","ビィ","ピ","ピィ","フィ","ミ","ミィ","リ","リィ","ウィ","ヴィ"};
    List<string> ls22= new List<string>() {"ウ","ク","キュ","グ","ギュ","ス","シュ","ズ","ジュ","ツ","チュ","トゥ","ヅ","ヂュ","ドゥ","デュ","テュ","ヌ","ニュ","フ","ヒュ","ブ","ビュ","プ","ピュ","フュ","ム","ミュ","ユ","ル","リュ","ヴ"};
    List<string> ls23= new List<string>() {"エ","ケ","キェ","ゲ","ギェ","セ","シェ","ゼ","ジェ","テ","チェ","デ","ヂェ","デェ","ツェ","テェ","ネ","ニェ","ヘ","ヒェ","ベ","ビェ","ペ","ピェ","フェ","メ","ミェ","レ","リェ","ウェ","ヴェ"};
    List<string> ls24= new List<string>() {"オ","コ","キョ","ゴ","ギョ","ソ","ショ","ゾ","ジョ","ト","チョ","ド","ヂョ","デョ","ツォ","テョ","ノ","ニョ","ホ","ヒョ","ボ","ビョ","ポ","ピョ","フォ","フョ","モ","ミョ","ヨ","ロ","リョ","ヲ","ヴォ"};

    void Start()
    {
        /*string str0="Unityで音声認識のゲームをする";
        List<string> lis0=new List<string>();
        lis0=SentenceToHiraganas(str0);
        foreach (string str1 in lis0)
        {
            Debug.Log(str1);
        }*/
        //JudgeRap("昔々あるところにおじいさんとおばあさんがいました");
        //Debug.Log(JudgeRap("宇部の栄でvicolo捨てるbのインテルsoles webosテンの新たりするヒューマンアジュールtoニューサ人気んが米朝右ふりするして集合宇部の栄でvicolo捨てるbのインテルsoles webosテンの新たりするヒューマンアジュールtoニューサ人気んが米朝右ふりするして集合"));
        //Debug.Log(JudgeMaxVowelCombo("テン","ノ"));
        //JudgeMaxVowelCombo("ムカシ","アル");
    }
    void Update()
    {
    }

    //ラップ判定
    public float JudgeRap(string st00)
    {
        float PointOfRap=0;
        List<(string,string)> ls00= new List<(string,string)>();
        ls00=SentenceToWords(st00);
        /*foreach (string str1 in ls00)
        {
            Debug.Log(str1);
        }*/
        for(var i = 0; i < ls00.Count; i++)
        {
            for(var j = i; j < ls00.Count; j++)
            {
                if(Mathf.Abs(i-j)<=JudgeVowelRange&&ls00[i].Item1!=ls00[j].Item1)//距離がJudgeVowelRange以内で、自分と違う形態素のみ韻を踏んでるか判断する
                {
                    //Debug.Log($"{i}{ls00[i].Item2}{j}{ls00[j].Item2}");
                    //Debug.Log("韻の数は"+JudgeMaxVowelCombo(ls00[i].Item2,ls00[j].Item2));
                    //Debug.Log($"{i}{ls00[i].Item2}{j}{ls00[j].Item2}");
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
        List<(string,string)> ls01= new List<(string,string)>();

        var dicDir = @"Assets/Plugins/NMeCab-0.10.2/dic/ipadic";

        using (var tagger = MeCabIpaDicTagger.Create(dicDir))
        {
            var nodes = tagger.Parse(st01);

            foreach (var item in nodes)
            {
                ls01.Add((item.Surface,item.Reading));
            }
        }
        return(ls01);
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
        /*for(var i = 0; i < ls10.Count; i++)
        {
            Debug.Log(ls10[i]);
        }*/
        //Debug.Log(st10+","+st11);
        //ls10をもって動かす
        for(var i = 0; i < ls10.Count; i++)
        {
            for(var j = 0; j < ls11.Count; j++)
            {
                //Debug.Log(CountVowelCombo(ls10,ls11,i,j));
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
        /*if(st20==st21)
        {
            return(2);
        }
        else */
        if((ls20.Contains(st20)&&ls20.Contains(st21))||(ls21.Contains(st20)&&ls21.Contains(st21))||(ls22.Contains(st20)&&ls22.Contains(st21))||(ls23.Contains(st20)&&ls23.Contains(st21))||(ls24.Contains(st20)&&ls24.Contains(st21)))
        {
            return(1);
        }
        else
        {
            return(0);
        }
    }
}