using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;   //Windowsの音声認識で使用

public class voice_recognition : MonoBehaviour
{
    DictationRecognizer dictationRecognizer;
    float time;

    string ans;
    string fullans;

    float count = 0;

	void Start () {
		dictationRecognizer = new DictationRecognizer();
	}
	
	void Update () {
		
        time += Time.deltaTime;
        if(time > 3.0f && time < 7.0f){
            //ディクテーションを開始
            if(count == 0) {
                dictationRecognizer.Start();
                Debug.Log("音声認識開始");
                count = 1;
            }
            dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;//DictationRecognizer_DictationResult処理を行う
            dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;//DictationRecognizer_DictationHypothesis処理を行う
            dictationRecognizer.DictationError += DictationRecognizer_DictationError;//DictationRecognizer_DictationError処理を行う
        }else if(time > 10.0f) {
            if(count == 1){
                Debug.Log(fullans);
                dictationRecognizer.Stop();
                dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;//DictationRecognizer_DictationComplete処理を行う
                count = 2;
            }
        }
	}

    //DictationResult：音声が特定の認識精度で認識されたときに発生するイベント
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence){
        if(ans != text)
        {
            fullans += text;
            ans = text;
            Debug.Log(fullans);
        }
    }
    
    //DictationHypothesis：音声入力中に発生するイベント
    private void DictationRecognizer_DictationHypothesis(string text){
        //Debug.Log("音声認識中："+　text);
    }

    //DictationComplete：音声認識セッションを終了したときにトリガされるイベント
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause){
        Debug.Log("音声認識完了");
    }

    //DictationError：音声認識セッションにエラーが発生したときにトリガされるイベント
    private void DictationRecognizer_DictationError(string error, int hresult){
        //Debug.Log("音声認識エラー");
    }
}
