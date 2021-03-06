using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
public class PrologueGameManager : MonoBehaviour
{
    private string _text = "";
    private Queue<char> _charQueue;

    private const char SEPARATE_PAGE = '&';
    private const char SEPARATE_COMMAND = '!';
    private const char COMMAND_SEPARATE_PARAM = '=';
    private const string COMMAND_BACKGROUND = "background";
    private const string COMMAND_SPRITE = "_sprite";
    private const string COMMAND_COLOR = "_color";
    private Queue<string> _pageQueue;
    private const string COMMAND_ACTIVE = "_active";
    private const string COMMAND_DELETE = "_delete";
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
    private const string COMMAND_CHARACTER_IMAGE = "charaimg";
    private const string COMMAND_SIZE = "_size";
    private const string COMMAND_POSITION = "_pos";
    private const string COMMAND_ROTATION = "_rotate";
    private const char COMMAND_SEPARATE_ANIM = '%';
    private const string COMMAND_ANIM = "_anim";
    private const string COMMAND_WAIT_TIME = "wait";
    private const string COMMAND_CHANGE_SCENE = "scene";
    private float _waitTime = 0;

    private const string SE_AUDIOSOURCE_PREFAB = "SEAudioSource";
    [SerializeField]
    private Text mainText;
    [SerializeField]
    private string textFile = "Texts/Scenario";
    [SerializeField]
    private float captionSpeed = 0.2f;
    [SerializeField]
    private GameObject nextPageIcon;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private string spritesDirectory = "Sprites/";
    [SerializeField]
    private string prefabsDirectory = "Prefabs/";
    private List<Image> _charaImageList = new List<Image>();
    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private GameObject seAudioSources;
    [SerializeField]
    private string audioClipsDirectory = "AudioClips/";
    private List<AudioSource> _seList = new List<AudioSource>();
    [SerializeField]
    private Image foregroundImage;
    [SerializeField]
    private string animationsDirectory = "Animations/";
    [SerializeField]
    private string overrideAnimationClipName = "Clip";

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // ???(=0)????????????????????????OnClick???????????????????????????
        if (Input.GetMouseButtonDown(0)) OnClick();
    }

    /**
    * ???????????????
    */
    private void Init()
    {
        _text = LoadTextFile(textFile);
        _pageQueue = SeparateString(_text, SEPARATE_PAGE);
        ShowNextPage();
    }
    /**
    * ???????????????????????????????????????
    */
    private string LoadTextFile(string fname)
    {
        TextAsset textasset = Resources.Load<TextAsset>(fname);
        return textasset.text.Replace("\n", "").Replace("\r", "");
    }

    /**
    * ??????????????????????????????
    */
    private bool ShowNextPage()
    {
        if (_pageQueue.Count <= 0) return false;
        nextPageIcon.SetActive(false);
        ReadLine(_pageQueue.Dequeue());
        return true;
    }

    /**
    * ?????????????????????????????????
    */
    private void OnClick()
    {
        if (_charQueue.Count > 0) OutputAllChar();
        else
        {
            if (!ShowNextPage())
                return;
                // Unity???????????????Play????????????????????????
                //UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    /**
    * 1??????????????????
    */
    private void ReadLine(string text)
    {
        if (text[0].Equals(SEPARATE_COMMAND))
        {
            ReadCommand(text);
            if (_waitTime > 0)
            {
                StartCoroutine(WaitForCommand());
                return;
            }
            ShowNextPage();
            return;
        }
        mainText.text = "";
        _charQueue = SeparateString(text);
        StartCoroutine(ShowChars(captionSpeed));
    }

    /**
    * ????????????????????????bool???????????????
    */
    private bool ParameterToBool(string parameter)
    {
        string p = parameter.Replace(" ", "");
        return p.Equals("true") || p.Equals("TRUE");
    }

    /**
    * ??????1??????????????????????????????????????????????????????????????????
    */
    private Queue<char> SeparateString(string str)
    {
        // ????????????char????????????????????? = 1????????????????????????
        char[] chars = str.ToCharArray();
        Queue<char> charQueue = new Queue<char>();
        // foreach????????????chars?????????????????????????????????????????????
        // ?????????????????????
        foreach (char c in chars) charQueue.Enqueue(c);
        return charQueue;
    }

    /**
    * 1?????????????????????
    */
    private bool OutputChar()
    {
        // ????????????????????????????????????????????????false?????????
        if (_charQueue.Count <= 0)
        {
            nextPageIcon.SetActive(true);
            return false;
        }
        // ?????????????????????????????????????????????????????????????????????
        mainText.text += _charQueue.Dequeue();
        return true;
    }

    /**
    * ?????????????????????????????????
    */
    private IEnumerator ShowChars(float wait)
    {
        // OutputChar???????????????false?????????(=????????????????????????)?????????????????????
        while (OutputChar())
            // wait???????????????
            yield return new WaitForSeconds(wait);
        // ??????????????????????????????
        yield break;
    }

    /**
    * ?????????????????????
    */
    private void OutputAllChar()
    {
        // ??????????????????????????????
        StopCoroutine(ShowChars(captionSpeed));
        // ????????????????????????????????????
        while (OutputChar()) ;
        _waitTime = 0;
        nextPageIcon.SetActive(true);
    }

    /**
    * ???????????????????????????????????????????????????????????????????????????????????????????????????
    */
    private Queue<string> SeparateString(string str, char sep)
    {
        string[] strs = str.Split(sep);
        Queue<string> queue = new Queue<string>();
        foreach (string l in strs) queue.Enqueue(l);
        return queue;
    }

    /**
    * ???????????????
    */
    private void SetBackgroundImage(string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_BACKGROUND, "");
        SetImage(cmd, parameter, backgroundImage);
    }

    /**
    * ???????????????
    */
    private void SetForegroundImage(string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_FOREGROUND, "");
        SetImage(cmd, parameter, foregroundImage);
    }

    /**
    * ??????????????????????????????????????????????????????????????????????????????
    */
    private Sprite LoadSprite(string name)
    {
        return Instantiate(Resources.Load<Sprite>(spritesDirectory + name));
    }

    /**
    * ??????????????????????????????????????????
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
    * ???????????????
    */
    private void SetImage(string cmd, string parameter, Image image)
    {
        cmd = cmd.Replace(" ", "");
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        switch (cmd)
        {
            case COMMAND_SPRITE:
                image.sprite = LoadSprite(parameter);
                break;
            case COMMAND_COLOR:
                image.color = ParameterToColor(parameter);
                break;
            case COMMAND_ANIM:
                ImageSetAnimation(image, parameter);
                break;
        }
    }

    /**
    * ???????????????????????????????????????????????????
    */
    private Vector3 ParameterToVector3(string parameter)
    {
        string[] ps = parameter.Replace(" ", "").Split(',');
        return new Vector3(float.Parse(ps[0]), float.Parse(ps[1]), float.Parse(ps[2]));
    }

    /**
    * ????????????????????????????????????????????????????????????????????????
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
    * ?????????????????????????????????????????????
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
    * BGM?????????
    */
    private void SetBackgroundMusic(string cmd, string parameter)
    {
        cmd = cmd.Replace(COMMAND_BGM, "");
        SetAudioSource(cmd, parameter, bgmAudioSource);
    }

    /**
    * ??????????????????
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
    * ???????????????
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
    * ???????????????????????????????????????????????????????????????
    */
    private AudioClip LoadAudioClip(string name)
    {
        return Instantiate(Resources.Load<AudioClip>(audioClipsDirectory + name));
    }

    /**
    * ??????????????????????????????????????????
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
    * ?????????????????????????????????
    */
    private void FadeSound(string parameter, AudioSource audio)
    {
        string[] ps = parameter.Replace(" ", "").Split(',');
        StartCoroutine(FadeSound(audio, int.Parse(ps[0]), int.Parse(ps[1])));
    }

    /**
    * ???????????????????????????
    */
    private void SetWaitTime(string parameter)
    {
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        _waitTime = float.Parse(parameter);
    }

    /**
    * ????????????????????????????????????????????????
    */
    private IEnumerator WaitForCommand()
    {
        yield return new WaitForSeconds(_waitTime);
        _waitTime = 0;
        ShowNextPage();
        yield break;
    }

    /**
    * ???????????????????????????????????????
    */
    private void ChangeNextScene(string parameter)
    {
        parameter = parameter.Substring(parameter.IndexOf('"') + 1, parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
        SceneManager.LoadSceneAsync(parameter);
    }

    /**
    * ???????????????????????????
    */
    private void ReadCommand(string cmdLine)
    {
        // ????????????!??????????????????
        cmdLine = cmdLine.Remove(0, 1);
        Queue<string> cmdQueue = SeparateString(cmdLine, SEPARATE_COMMAND);
        foreach (string cmd in cmdQueue)
        {
            // ???=???????????????
            string[] cmds = cmd.Split(COMMAND_SEPARATE_PARAM);
            // ????????????????????????????????????????????????????????????
            if (cmds[0].Contains(COMMAND_BACKGROUND))
                SetBackgroundImage(cmds[0], cmds[1]);
            if (cmds[0].Contains(COMMAND_FOREGROUND))
                SetForegroundImage(cmds[0], cmds[1]);
            if (cmds[0].Contains(COMMAND_BGM))
                SetBackgroundMusic(cmds[0], cmds[1]);
            if (cmds[0].Contains(COMMAND_SE))
                SetSoundEffect(cmds[1], cmds[0], cmds[2]);
            if (cmds[0].Contains(COMMAND_WAIT_TIME))
                SetWaitTime(cmds[1]);
            if (cmds[0].Contains(COMMAND_CHANGE_SCENE))
                ChangeNextScene(cmds[1]);
        }
    }
}
