using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private const char SEPARATE_MAIN_START = '「';
    private const char SEPARATE_MAIN_END = '」';
    private Queue<char> _charQueue;
    private const char SEPARATE_PAGE = '&';
    private Queue<string> _pageQueue;
    private const char SEPARATE_COMMAND = '!';
    private const char COMMAND_SEPARATE_PARAM = '=';
    private const string COMMAND_BACKGROUND = "background";
    private const string COMMAND_SPRITE = "_sprite";
    private const string COMMAND_COLOR = "_color";
    private const string COMMAND_CHARACTER_IMAGE = "charaimg";
    private const string COMMAND_SIZE = "_size";
    private const string COMMAND_POSITION = "_pos";
    private const string COMMAND_ROTATION = "_rotate";
    private const string COMMAND_ACTIVE = "_active";
    private const string COMMAND_DELETE = "_delete";
    private const char COMMAND_SEPARATE_ANIM = '%';
    private const string COMMAND_ANIM = "_anim";
    private const char SEPARATE_SUBSCENE = '#';
    private Dictionary<string, Queue<string>> _subScenes =
        new Dictionary<string, Queue<string>>();
    private const string COMMAND_JUMP = "jump_to";
    private const string COMMAND_TOP = "top";
    private const string COMMAND_BOTTOM = "bottom";
    private const string COMMAND_TEXT = "_text";
    private const string SELECT_BUTTON_PREFAB = "SelectButton";
    private const string COMMAND_WAIT_TIME = "wait";
    private float _waitTime = 0;
    private const string COMMAND_BGM = "bgm";
    private const string COMMAND_SE = "se";
    private const string COMMAND_PLAY = "_play";
    private const string COMMAND_MUTE = "_mute";
    private const string COMMAND_SOUND = "_sound";
    private const string COMMAND_VOLUME = "_volume";
    private const string COMMAND_PRIORITY = "_priority";
    private const string COMMAND_LOOP = "_loop";
    private const string COMMAND_FADE = "_fade";
    private const string COMMAND_FOREGROUND = "foreground";
    private const string COMMAND_CHANGE_SCENE = "scene";
    private const string SE_AUDIOSOURCE_PREFAB = "SEAudioSource";
    private const string CHARACTER_IMAGE_PREFAB = "CharacterImage";
    private bool wait = false;
    // SerializeFieldと書くとprivateなパラメーターでも
    // インスペクター上で値を変更できる
    [SerializeField]
    private Text mainText;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private float captionSpeed = 0.2f;
    [SerializeField]
    private GameObject nextPageIcon;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image foregroundImage;
    [SerializeField]
    private GameObject characterImages;
    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private GameObject seAudioSources;
    [SerializeField]
    private string spritesDirectory = "Sprites/";
    [SerializeField]
    private string prefabsDirectory = "Prefabs/";
    [SerializeField]
    private string textFile = "Texts/Scenario";
    [SerializeField]
    private string animationsDirectory = "Animations/";
    [SerializeField]
    private GameObject topButtons;
    [SerializeField]
    private GameObject bottomButtons;

    public List<Button> _selectButtonList = new List<Button>();
    [SerializeField]
    private string overrideAnimationClipName = "Clip";
    private List<Image> _charaImageList = new List<Image>();
    [SerializeField]
    private string audioClipsDirectory = "AudioClips/";
    private List<AudioSource> _seList = new List<AudioSource>();

    private string _text = "";
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 左(=0)クリックされたらOnClickメソッドを呼び出し
        if (Input.GetMouseButtonDown(0) && !wait) OnClick();
    }

    // ここから下は作った関数

    /**
    * 初期化する
    */
    private void Init()
    {
        _text = LoadTextFile(textFile);
        Queue<string> subScenes = SeparateString(_text, SEPARATE_SUBSCENE);
        foreach (string subScene in subScenes)
        {
            if (subScene.Equals("")) continue;
            Queue<string> pages = SeparateString(subScene, SEPARATE_PAGE);
            _subScenes[pages.Dequeue()] = pages;
        }
        _pageQueue = _subScenes.First().Value;
        ShowNextPage();
    }

    /**
    * 次のページを表示する
    */
    private bool ShowNextPage()
    {
        if (_pageQueue.Count <= 0) return false;
        // オブジェクトの表示/非表示を設定する
        nextPageIcon.SetActive(false);

        ReadLine(_pageQueue.Dequeue());
        return true;
    }

    /**
    * クリックしたときの処理
    */
    private void OnClick()
    {
        if (_charQueue.Count > 0) OutputAllChar();
        else
        {
            if (_selectButtonList.Count > 0) return;
        if (!ShowNextPage())
                return;
                // UnityエディタのPlayモードを終了する
                //UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    /**
    * 1行を読み出す
    */
    private void ReadLine(string text)
    {
        // 最初が「!」だったら
        if (text[0].Equals(SEPARATE_COMMAND))
        {
            ReadCommand(text);
            if (_selectButtonList.Count > 0) return;
            if (_waitTime > 0)
            {
                StartCoroutine(WaitForCommand());
                return;
            }
            ShowNextPage();
            return;
        }
        string[] ts = text.Split(SEPARATE_MAIN_START);
        string name = ts[0];
        string main = ts[1].Remove(ts[1].LastIndexOf(SEPARATE_MAIN_END));
        if (name.Equals("")) nameText.transform.parent.gameObject.SetActive(false);
        else
        {
            nameText.text = name;
            nameText.transform.parent.gameObject.SetActive(true);
        }
        mainText.text = "";
        _charQueue = SeparateString(main);
        StartCoroutine(ShowChars(captionSpeed));
    }

    /**
    * コマンドの読み出し
    */
    private void ReadCommand(string cmdLine)
    {
        // 最初の「!」を削除する
        cmdLine = cmdLine.Remove(0, 1);
        Queue<string> cmdQueue = SeparateString(cmdLine, SEPARATE_COMMAND);
        foreach (string cmd in cmdQueue)
        {
            // 「=」で分ける
            string[] cmds = cmd.Split(COMMAND_SEPARATE_PARAM);
            // もし背景コマンドの文字列が含まれていたら
            if (cmds[0].Contains(COMMAND_BACKGROUND))
                SetBackgroundImage(cmds[0], cmds[1]);
            if (cmds[0].Contains(COMMAND_FOREGROUND))
                SetForegroundImage(cmds[0], cmds[1]);
            if (cmds[0].Contains(COMMAND_CHARACTER_IMAGE))
                SetCharacterImage(cmds[1], cmds[0], cmds[2]);
            if (cmds[0].Contains(COMMAND_JUMP))
                JumpTo(cmds[1]);
            if (cmds[0].Contains(COMMAND_TOP))
                SetTopButton(cmds[1], cmds[0], cmds[2]);
            if (cmds[0].Contains(COMMAND_BOTTOM))
                SetBottomButton(cmds[1], cmds[0], cmds[2]);
            if (cmds[0].Contains(COMMAND_WAIT_TIME))
                SetWaitTime(cmds[1]);
            if (cmds[0].Contains(COMMAND_BGM))
                SetBackgroundMusic(cmds[0], cmds[1]);
            if (cmds[0].Contains(COMMAND_SE))
                SetSoundEffect(cmds[1], cmds[0], cmds[2]);
            if (cmds[0].Contains(COMMAND_CHANGE_SCENE))
                ChangeNextScene(cmds[1]);
            
        }
    }

    /**
    * 待機時間を設定する
    */
    private void SetWaitTime(string parameter)
    {
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        _waitTime = float.Parse(parameter);
    }

    /**
    * 次の読み込みを待機するコルーチン
    */
    private IEnumerator WaitForCommand()
    {
        wait = true;
        yield return new WaitForSeconds(_waitTime);
        _waitTime = 0;
        wait = false;
        ShowNextPage();
        yield break;
    }
    private string LoadTextFile(string fname)
    {
        TextAsset textasset = Resources.Load<TextAsset>(fname);
        return textasset.text.Replace("\n", "").Replace("\r", "");
    }

    /**
    * 選択肢がクリックされた
    */
    private void SelectButtonOnClick(string label)
    {
        foreach (Button button in _selectButtonList) Destroy(button.gameObject);
        _selectButtonList.Clear();
        JumpTo('"' + label + '"');
        ShowNextPage();
    }

    /**
    * 選択肢の設定
    */
    private void SetTopButton(string name, string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_TOP, "");
        name = name.Substring(name.IndexOf('"') + 1, name.LastIndexOf('"') - name.IndexOf('"') - 1);
        Button button = _selectButtonList.Find(n => n.name == name);
        if (button == null)
        {
            button = Instantiate(Resources.Load<Button>(prefabsDirectory + SELECT_BUTTON_PREFAB),topButtons.transform);
            button.name = name;
            button.onClick.AddListener(() => SelectButtonOnClick(name));
            _selectButtonList.Add(button);
        }
        SetImage(cmd, parameter, button.image);
    }
    private void SetBottomButton(string name, string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_BOTTOM, "");
        name = name.Substring(name.IndexOf('"') + 1, name.LastIndexOf('"') - name.IndexOf('"') - 1);
        Button button = _selectButtonList.Find(n => n.name == name);
        if (button == null)
        {
            button = Instantiate(Resources.Load<Button>(prefabsDirectory + SELECT_BUTTON_PREFAB),bottomButtons.transform);
            button.name = name;
            button.onClick.AddListener(() => SelectButtonOnClick(name));
            _selectButtonList.Add(button);
        }
        SetImage(cmd, parameter, button.image);
    }

    /**
    * BGMの設定
    */
    private void SetBackgroundMusic(string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_BGM, "");
        SetAudioSource(cmd, parameter, bgmAudioSource);
    }

    /**
    * 効果音の設定
    */
    private void SetSoundEffect(string name, string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_SE, "");
        name = name.Substring(name.IndexOf('"') + 1, name.LastIndexOf('"') - name.IndexOf('"') - 1);
        AudioSource audio = _seList.Find(n => n.name == name);
        if (audio == null)
        {
            audio = Instantiate(Resources.Load<AudioSource>(prefabsDirectory + SE_AUDIOSOURCE_PREFAB), seAudioSources.transform);
            audio.name = name;
            _seList.Add(audio);
        }
        SetAudioSource(cmd, parameter, audio);
    }

    /**
    * 音声の設定
    */
    private void SetAudioSource(string cmd, string parameter, AudioSource audio)
    {
        cmd = cmd.Replace(" ", "");
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        switch (cmd)
        {
            case COMMAND_PLAY:
                audio.Play();
                break;
            case COMMAND_MUTE:
                audio.mute = ParameterToBool(parameter);
                break;
            case COMMAND_SOUND:
                audio.clip = LoadAudioClip(parameter);
                break;
            case COMMAND_VOLUME:
                audio.volume = float.Parse(parameter);
                break;
            case COMMAND_PRIORITY:
                audio.priority = int.Parse(parameter);
                break;
            case COMMAND_LOOP:
                audio.loop = ParameterToBool(parameter);
                break;
            case COMMAND_FADE:
                FadeSound(parameter, audio);
                break;
            case COMMAND_ACTIVE:
                audio.gameObject.SetActive(ParameterToBool(parameter));
                break;
            case COMMAND_DELETE:
                _seList.Remove(audio);
                Destroy(audio.gameObject);
                break;
        }
    }

    /**
    * 音声ファイルを読み出し、インスタンス化する
    */
    private AudioClip LoadAudioClip(string name)
    {
        return Instantiate(Resources.Load<AudioClip>(audioClipsDirectory + name));
    }

    /**
    * 音のフェードを行うコルーチン
    */
    private IEnumerator FadeSound(AudioSource audio, float time, float volume)
    {
        float vo = (volume - audio.volume) / (time / Time.deltaTime);
        bool isOut = audio.volume > volume;
        while ((!isOut && audio.volume < volume) || (isOut && audio.volume > volume))
        {
            audio.volume += vo;
            yield return null;
        }
        audio.volume = volume;
    }

    /**
    * 音声にフェードをかける
    */
    private void FadeSound(string parameter, AudioSource audio)
    {
        string[] ps = parameter.Replace(" ", "").Split(',');
        StartCoroutine(FadeSound(audio, int.Parse(ps[0]), int.Parse(ps[1])));
    }

    /**
    * 背景の設定
    */
    private void SetBackgroundImage(string cmd, string parameter)
    {
        // 空白を削除し、背景コマンドの文字列も削除する
        cmd = cmd.Replace(COMMAND_BACKGROUND, "");
        SetImage(cmd, parameter, backgroundImage);
    }

    /**
    * 前景の設定
    */
    private void SetForegroundImage(string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_FOREGROUND, "");
        SetImage(cmd, parameter, foregroundImage);
    }

    /**
    * 立ち絵の設定
    */
    private void SetCharacterImage(string name, string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_CHARACTER_IMAGE, "");
        name = name.Substring(name.IndexOf('"') + 1, name.LastIndexOf('"') - name.IndexOf('"') - 1);
        Image image = _charaImageList.Find(n => n.name == name);
        if (image == null)
        {
            image = Instantiate(Resources.Load<Image>(prefabsDirectory + CHARACTER_IMAGE_PREFAB), characterImages.transform);
            image.name = name;
            _charaImageList.Add(image);
        }
        SetImage(cmd, parameter, image);
    }

    /**
    * パラメーターからboolを取得する
    */
    private bool ParameterToBool(string parameter)
    {
        string p = parameter.Replace(" ", "");
        return p.Equals("true") || p.Equals("TRUE");
    }
    /**
    * パラメーターからベクトルを取得する
    */
    private Vector3 ParameterToVector3(string parameter)
    {
        string[] ps = parameter.Replace(" ", "").Split(',');
        return new Vector3(float.Parse(ps[0]), float.Parse(ps[1]), float.Parse(ps[2]));
    }

    /**
    * 対応するラベルまでジャンプする
    */
    private void JumpTo(string parameter)
    {
        nameText.text = "";
        mainText.text = "";
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        _pageQueue = _subScenes[parameter];
    }

    /**
    * スプライトをファイルから読み出し、インスタンス化する
    */
    private Sprite LoadSprite(string name)
    {
        return Instantiate(Resources.Load<Sprite>(spritesDirectory + name));
    }

    /**
    * パラメーターから色を作成する
    */
    private Color ParameterToColor(string parameter)
    {
        string[] ps = parameter.Replace(" ", "").Split(',');
        if (ps.Length > 3)
            return new Color32(byte.Parse(ps[0]), byte.Parse(ps[1]),
                                            byte.Parse(ps[2]), byte.Parse(ps[3]));
        else
            return new Color32(byte.Parse(ps[0]), byte.Parse(ps[1]),
                                            byte.Parse(ps[2]), 255);
    }

    /**
    * 画像の設定
    */
    private void SetImage(string cmd, string parameter, Image image)
    {
        cmd = cmd.Replace(" ", "");
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        switch (cmd)
        {
            case COMMAND_TEXT:
                image.GetComponentInChildren<Text>().text = parameter;
                break;
            case COMMAND_SPRITE:
                image.sprite = LoadSprite(parameter);
                break;
            case COMMAND_COLOR:
                image.color = ParameterToColor(parameter);
                break;
            case COMMAND_SIZE:
                image.GetComponent<RectTransform>().sizeDelta = ParameterToVector3(parameter);
                break;
            case COMMAND_POSITION:
                image.GetComponent<RectTransform>().anchoredPosition = ParameterToVector3(parameter);
                break;
            case COMMAND_ROTATION:
                image.GetComponent<RectTransform>().eulerAngles = ParameterToVector3(parameter);
                break;
            case COMMAND_ACTIVE:
                image.gameObject.SetActive(ParameterToBool(parameter));
                break;
            case COMMAND_DELETE:
                _charaImageList.Remove(image);
                Destroy(image.gameObject);
                break;
            case COMMAND_ANIM:
                ImageSetAnimation(image, parameter);
                break;
        }
    }

    /**
    * パラメーターからアニメーションクリップを生成する
    */
    private AnimationClip ParameterToAnimationClip(Image image, string[] parameters)
    {
        string[] ps = parameters[0].Replace(" ", "").Split(',');
        string path = animationsDirectory + SceneManager.GetActiveScene().name + "/" + image.name;
        AnimationClip prevAnimation = Resources.Load<AnimationClip>(path + "/" + ps[0]);
        AnimationClip animation;
        #if UNITY_EDITOR
            if (ps[3].Equals("Replay") && prevAnimation != null)
                return Instantiate(prevAnimation);
            animation = new AnimationClip();
            Color startcolor = image.color;
            Vector3[] start = new Vector3[3];
            start[0] = image.GetComponent<RectTransform>().sizeDelta;
            start[1] = image.GetComponent<RectTransform>().anchoredPosition;
            Color endcolor = startcolor;
            if (parameters[1] != "") endcolor = ParameterToColor(parameters[1]);
            Vector3[] end = new Vector3[3];
            for (int i = 0; i < 2; i++)
            {
                if (parameters[i + 2] != "")
                    end[i] = ParameterToVector3(parameters[i + 2]);
                else end[i] = start[i];
            }
            AnimationCurve[,] curves = new AnimationCurve[4, 4];
            if (ps[3].Equals("EaseInOut"))
            {
                curves[0, 0] = AnimationCurve.EaseInOut(float.Parse(ps[1]), startcolor.r, float.Parse(ps[2]), endcolor.r);
                curves[0, 1] = AnimationCurve.EaseInOut(float.Parse(ps[1]), startcolor.g, float.Parse(ps[2]), endcolor.g);
                curves[0, 2] = AnimationCurve.EaseInOut(float.Parse(ps[1]), startcolor.b, float.Parse(ps[2]), endcolor.b);
                curves[0, 3] = AnimationCurve.EaseInOut(float.Parse(ps[1]), startcolor.a, float.Parse(ps[2]), endcolor.a);
                for (int i = 0; i < 2; i++)
                {
                    curves[i + 1, 0] = AnimationCurve.EaseInOut(float.Parse(ps[1]), start[i].x, float.Parse(ps[2]), end[i].x);
                    curves[i + 1, 1] = AnimationCurve.EaseInOut(float.Parse(ps[1]), start[i].y, float.Parse(ps[2]), end[i].y);
                    curves[i + 1, 2] = AnimationCurve.EaseInOut(float.Parse(ps[1]), start[i].z, float.Parse(ps[2]), end[i].z);
                }
            }
            else
            {
                curves[0, 0] = AnimationCurve.Linear(float.Parse(ps[1]), startcolor.r, float.Parse(ps[2]), endcolor.r);
                curves[0, 1] = AnimationCurve.Linear(float.Parse(ps[1]), startcolor.g, float.Parse(ps[2]), endcolor.g);
                curves[0, 2] = AnimationCurve.Linear(float.Parse(ps[1]), startcolor.b, float.Parse(ps[2]), endcolor.b);
                curves[0, 3] = AnimationCurve.Linear(float.Parse(ps[1]), startcolor.a, float.Parse(ps[2]), endcolor.a);
                for (int i = 0; i < 2; i++)
                {
                    curves[i + 1, 0] = AnimationCurve.Linear(float.Parse(ps[1]), start[i].x, float.Parse(ps[2]), end[i].x);
                    curves[i + 1, 1] = AnimationCurve.Linear(float.Parse(ps[1]), start[i].y, float.Parse(ps[2]), end[i].y);
                    curves[i + 1, 2] = AnimationCurve.Linear(float.Parse(ps[1]), start[i].z, float.Parse(ps[2]), end[i].z);
                }
            }
            string[] b1 = { "r", "g", "b", "a" };
            for (int i = 0; i < 4; i++)
            {
                AnimationUtility.SetEditorCurve(
                    animation,
                    EditorCurveBinding.FloatCurve("", typeof(Image), "m_Color." + b1[i]),
                    curves[0, i]
                );
            }
            string[] a = { "m_SizeDelta", "m_AnchoredPosition" };
            string[] b2 = { "x", "y", "z" };
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    AnimationUtility.SetEditorCurve(
                        animation,
                        EditorCurveBinding.FloatCurve("", typeof(RectTransform), a[i] + "." + b2[j]),
                        curves[i + 1, j]
                    );
                }   
            }
            if (!Directory.Exists("Assets/Resources/" + path))
                Directory.CreateDirectory("Assets/Resources/" + path);
            AssetDatabase.CreateAsset(animation, "Assets/Resources/" + path + "/" + ps[0] + ".anim");
            AssetDatabase.ImportAsset("Assets/Resources/" + path + "/" + ps[0] + ".anim");
        #elif UNITY_STANDALONE
            animation = prevAnimation;
        #endif
        return Instantiate(animation);
    }
    /**
    * アニメーションを画像に設定する
    */
    private void ImageSetAnimation(Image image, string parameter)
    {
        Animator animator = image.GetComponent<Animator>();
        AnimationClip clip = ParameterToAnimationClip(image, parameter.Split(COMMAND_SEPARATE_ANIM));
        AnimatorOverrideController overrideController;
        if (animator.runtimeAnimatorController is AnimatorOverrideController)
            overrideController = (AnimatorOverrideController)animator.runtimeAnimatorController;
        else
        {
            overrideController = new AnimatorOverrideController();
            overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
            animator.runtimeAnimatorController = overrideController;
        }
        overrideController[overrideAnimationClipName] = clip;
        animator.Update(0.0f);
        animator.Play(overrideAnimationClipName, 0);
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
        if (_charQueue.Count <= 0)
        {
            nextPageIcon.SetActive(true);
            return false;
        }
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
    * 全文を表示する
    */
    private void OutputAllChar()
    {
        // コルーチンをストップ
        StopCoroutine(ShowChars(captionSpeed));
        // キューが空になるまで表示
        while (OutputChar()) ;
        _waitTime = 0;
        nextPageIcon.SetActive(true);
    }

    /**
    * 文字列を指定した区切り文字ごとに区切り、キューに格納したものを返す
    */
    private Queue<string> SeparateString(string str, char sep)
    {
        string[] strs = str.Split(sep);
        Queue<string> queue = new Queue<string>();
        foreach (string l in strs) queue.Enqueue(l);
        return queue;
    }
    /**
    * 対応するシーンに切り替える
    */
    private void ChangeNextScene(string parameter)
    {
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        SceneManager.LoadSceneAsync(parameter);
    }
}
