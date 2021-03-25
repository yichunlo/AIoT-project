using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class LegoUI : MonoBehaviour
{
    public TMP_Dropdown mode, waveMode;
    public TMP_InputField x, y;
    public Slider fz, amp;
    public Button respawn;
    public Button snake;
    public Button back;
    public LegoManager manager;
    public InputMaker maker;
    public GameObject ball;
    public Canvas cvs;
    //public Text wm, ws, fq, ap;

    //public UnityEvent OnChangeToApp = new UnityEvent();


    private Vector3 initPos;

    private void Awake()
    {
        //mode.gameObject.SetActive(false);
        initPos = ball.transform.position;
        //wm.enabled = false;
        //ws.enabled = false;
        //fq.enabled = false;
        //ap.enabled = false;
        mode.value = 0;
        waveMode.value = 0;
        waveMode.interactable = false;
        waveMode.gameObject.SetActive(false);
        x.interactable = false;
        x.gameObject.SetActive(false);
        y.interactable = false;
        y.gameObject.SetActive(false);
        fz.interactable = false;
        fz.gameObject.SetActive(false);
        amp.interactable = false;
        amp.gameObject.SetActive(false);
    }

    public void InitColor()
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                manager.legoColor[i, j].color = Color.white;
    }

    public void Update()
    {
        if (maker.mode != Mode.Snake)
        {
            snake.interactable = false;
            snake.gameObject.SetActive(false);
        }

        else
        {
            snake.interactable = !maker.startSnake;
            snake.gameObject.SetActive(true);
        }
    }

    public void OnModeChange()
    {
        OnRespawn();
        if (mode.value == 0)
        {
            InitColor();
            maker.mode = Mode.None;
            waveMode.interactable = false;
            waveMode.gameObject.SetActive(false);
            x.interactable = false;
            x.gameObject.SetActive(false);
            y.interactable = false;
            y.gameObject.SetActive(false);
            fz.interactable = false;
            fz.gameObject.SetActive(false);
            amp.interactable = false;
            amp.gameObject.SetActive(false);
            respawn.gameObject.SetActive(true);
            respawn.interactable = true;
        }
        else if (mode.value == 1)
        {
            OnWaveModeChange();
            InitColor();
            waveMode.gameObject.SetActive(true);
            waveMode.interactable = true;
            x.gameObject.SetActive(true);
            x.interactable = true;
            y.gameObject.SetActive(true);
            y.interactable = true;
            fz.gameObject.SetActive(true);
            fz.interactable = true;
            amp.gameObject.SetActive(true);
            amp.interactable = true;
            respawn.gameObject.SetActive(true);
            respawn.interactable = true;
        }
        else if (mode.value == 2)
        {
            InitColor();
            maker.mode = Mode.Sphere;
            waveMode.interactable = false;
            waveMode.gameObject.SetActive(false);
            x.interactable = false;
            x.gameObject.SetActive(false);
            y.interactable = false;
            y.gameObject.SetActive(false);
            fz.interactable = false;
            fz.gameObject.SetActive(false);
            amp.interactable = false;
            amp.gameObject.SetActive(false);
            respawn.gameObject.SetActive(true);
            respawn.interactable = true;
        }
        else if(mode.value == 3)
        {
            maker.mode = Mode.Snake;
            waveMode.interactable = false;
            waveMode.gameObject.SetActive(false);
            x.interactable = false;
            x.gameObject.SetActive(false);
            y.interactable = false;
            y.gameObject.SetActive(false);
            fz.interactable = false;
            fz.gameObject.SetActive(false);
            amp.interactable = false;
            amp.gameObject.SetActive(false);
            respawn.interactable = false;
            respawn.gameObject.SetActive(false);
            snake.gameObject.SetActive(true);
            snake.interactable = true;
            ball.SetActive(false);
        }
        else if(mode.value == 4)
        {
            maker.mode = Mode.AppStore;
            cvs.enabled = true;
            //OnChangeToApp.Invoke();
        }
    }

    public void OnWaveModeChange()
    {
        if (waveMode.value == 0)
            maker.mode = Mode.Sine;
        else if (waveMode.value == 1)
            maker.mode = Mode.Cosine;
        else
            maker.mode = Mode.Tangent;
    }

    public void OnValueChange()
    {
        int xint = (x.text.Length > 0) ? int.Parse(x.text) : 0;
        int yint = (y.text.Length > 0) ? int.Parse(y.text) : 0;
        maker.waveSource = new Vector2(xint, yint);
    }

    public void OnFrequencyChange()
    {
        maker.frequency = fz.value;
    }

    public void OnAmplitueChange()
    {
        maker.amplitude = amp.value;
    }

    public void OnRespawn()
    {
        ball.SetActive(true);
        ball.transform.position = initPos;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void OnStartSnake()
    {
        maker.startSnake = true;
        snake.interactable = false;
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnClick(Canvas canvas)
    {
        canvas.enabled = false;
        mode.value = 0;
    }
}
