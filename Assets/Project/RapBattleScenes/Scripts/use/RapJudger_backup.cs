using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//韻ふみシステム
public class RapJudger_buckup : MonoBehaviour
{
    const float RhymeImportanceScale=1.6f;//累乗される
    const float MoreThanVowelImportanceScale=1.6f;//0か1かこれ
    List<char> ls20= new List<char>() {'あ','か','さ','た','な','は','ま','や','ら','わ'};
    List<char> ls21= new List<char>() {'い','き','し','ち','に','ひ','み','り'};
    List<char> ls22= new List<char>() {'う','く','す','つ','ぬ','ふ','む','ゆ','る'};
    List<char> ls23= new List<char>() {'え','け','せ','て','ね','へ','め','れ'};
    List<char> ls24= new List<char>() {'お','こ','そ','と','の','ほ','も','よ','ろ','を'};

    void Start()
    {
        //Debug.Log(JudgeVowel('ふ','て'));
        float num=JudgeRhyme("あいうええおおお","あえお");
    }
    void Update()
    {
    }
    void JudgeRap(List<string> ls00)
    {
        // for ( int i  = 0 ; i < ls00.Count ; i++ )
        // {
        //     JudgeRhyme("ad","ad");
        // }
    }
    float JudgeRhyme(string st10,string st11)//1.短い方の単位が 2.飛ばしながらでも韻を踏める最大文字数 3.乗するRhymeImportanceScaleを（未実装）
    {
        string st12;
        string st13;
        int MaxRhyme=0;
        //1
        if(st10.Length>=st11.Length)
        {
            st12=st10;//長い方
            st13=st11;//短い方
        }
        else
        {
            st12=st11;
            st13=st10;
        }
        //2.飛ばしながらでも韻を踏める最大文字数
        for(int i=0;i<st13.Length;i++)
        {
            int num10_pt=0;
            Debug.Log(st13[i]+"から踏む なら");
            string st13_cut=st13.Substring(i);
            (string,string,int) Evaluateing=(st12,st13_cut,num10_pt);
            (string,string,int) LastEvaluateing=("","",-1);
            while(Evaluateing.Item1!=""&&Evaluateing.Item2!=""&&LastEvaluateing.Item3!=Evaluateing.Item3)
            {
                Debug.Log("やります");
                LastEvaluateing=Evaluateing;
                Evaluateing=FindSameVowel(Evaluateing.Item1,Evaluateing.Item2,Evaluateing.Item3);
            }
            Debug.Log(st13[i]+"から踏んだ結果");
            Debug.Log(Evaluateing.Item3+"点");
            if(MaxRhyme<Evaluateing.Item3)
            {
                MaxRhyme=Evaluateing.Item3;
            }
        }
        Debug.Log(MaxRhyme+"が最終");
        return(MaxRhyme);
    }
    (string,string,int) FindSameVowel(string st14,string st15,int num11_pt)//母音が同じ字を消してポイント加算する
    {
        string st16=st14;
        string st17=st15;
        int num12_pt=num11_pt;
        for(int j=0;j<st14.Length;j++)
        {
            int num15_rh=JudgeVowel(st15[0],st14[j]);
            if(num15_rh>=1)
            {
                //文字切る
                st16=st16.Substring(j+1);
                st17=st17.Substring(1);
                num12_pt+=1;
                //Debug.Log(st16);
                //Debug.Log(st17);
                break;
            }
        }
        Debug.Log("踏んだので次は");
        Debug.Log(st16);
        Debug.Log(st17);
        return(st16,st17,num12_pt);
    }


    int JudgeVowel(char ch20,char ch21)//0,1,2を返す
    {
        if(ch20==ch21)
        {
            return(2);
        }
        else if((ls20.Contains(ch20)&&ls20.Contains(ch21))||(ls21.Contains(ch20)&&ls21.Contains(ch21))||(ls22.Contains(ch20)&&ls22.Contains(ch21))||(ls23.Contains(ch20)&&ls23.Contains(ch21))||(ls24.Contains(ch20)&&ls24.Contains(ch21)))
        {
            return(1);
        }
        else
        {
            return(0);
        }
    }
}
