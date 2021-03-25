using System.Collections.Generic;
using UnityEngine;


public enum Mode
{
    Sine,
    Cosine,
    Tangent,
    Sphere,
    Snake,
    AppStore,
    None
}

public class InputMaker : MonoBehaviour
{
    public Mode mode = Mode.None;
    [Range(-3,3)]
    public float frequency;
    [Range(0,2)]
    public float amplitude;
    public GameObject sphere;
    public LegoManager legoManager;
    public Vector2 waveSource = Vector2.zero;
    private float timer = 0;

    public bool startSnake = false;
    private bool previousSnake = false;
    private Queue<Vector2Int> snakeQueue = new Queue<Vector2Int>();
    private Vector2Int fruit;
    private Vector2Int[] allPos;
    private Vector2Int head;
    private int fruitIndex = 0;
    private int direction;  //0 for up
                            //1 for down
                            //2 for left
                            //3 for right
    private readonly int framePerTick = 8;
    private int directionInLastFrame;
    private int frame = 0, previous_frame = 0;

    private void Start()
    {
        allPos = new Vector2Int[100];
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                allPos[i * 10 + j] = new Vector2Int(i, j);
        //None();
    }

    private void Update()
    {
        if (     Input.GetKey(KeyCode.W) && directionInLastFrame != 1)
            direction = 0;
        else if (Input.GetKey(KeyCode.S) && directionInLastFrame != 0)
            direction = 1;
        else if (Input.GetKey(KeyCode.A) && directionInLastFrame != 3)
            direction = 2;
        else if (Input.GetKey(KeyCode.D) && directionInLastFrame != 2)
            direction = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (previousSnake == false && startSnake == true)
            snakeInit();
        switch(mode)
        {
            case Mode.None:
                startSnake = false;
                None();
                break;
            case Mode.Sphere:
                startSnake = false;
                Sphere();
                break;
            case Mode.Snake:
                if (startSnake)
                    Snake();
                else
                    None();
                break;
            default:
                startSnake = false;
                Wave();
                break;
        }
        previousSnake = startSnake;
    }

    void None()
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                legoManager.legoInput[i, j] *= 0.8f;
        //
    }

    void Sphere()
    {
        Vector2 sPos = new Vector2(sphere.transform.position.x, sphere.transform.position.z);
        float xdelta = 0;
        float ydelta = 0;
        if (Input.GetKey(KeyCode.W))
            ydelta += 1;
        if (Input.GetKey(KeyCode.S))
            ydelta -= 1;
        if (Input.GetKey(KeyCode.A))
            xdelta -= 1;
        if (Input.GetKey(KeyCode.D))
            xdelta += 1;
        if (xdelta != 0 || ydelta != 0)
        {
            sPos += new Vector2(xdelta, ydelta) * 40 * Time.fixedDeltaTime;
            sPos = new Vector2(Mathf.Clamp(sPos.x, -1.5f, 1.5f), Mathf.Clamp(sPos.y, -1.5f, 1.5f));
        }
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                legoManager.legoInput[i, j] = Mathf.Lerp(legoManager.legoInput[i, j], IndexToHeight(sPos, i, j), 0.075f);
    }

    void Wave()
    {
        float value = 0;
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                Vector2 legoPos = new Vector2(legoManager.legoArray[i, j].transform.position.x, legoManager.legoArray[i, j].transform.position.z);
                switch (mode)
                {
                    case Mode.Sine:
                        value = Mathf.Sin((timer + (legoPos - waveSource).magnitude / 5f) * 3.14159f * frequency) * amplitude;
                        break;
                    case Mode.Cosine:
                        value = Mathf.Cos((timer + (legoPos - waveSource).magnitude / 5f) * 3.14159f * frequency) * amplitude;
                        break;
                    case Mode.Tangent:
                        value = Mathf.Tan((timer + (legoPos - waveSource).magnitude / 5f) * 3.14159f * frequency) * amplitude;
                        break;
                }
                legoManager.legoInput[i, j] = value;
            }
        timer += Time.fixedDeltaTime;
    }

    void Snake()
    {
        frame++;
        if (frame % framePerTick != 0)
            return;

        Vector2Int nextPos = head;
        directionInLastFrame = direction;
        switch (direction)
        {
            case 0:
                nextPos += new Vector2Int(0, -1);
                break;
            case 1:
                nextPos += new Vector2Int(0, 1);
                break;
            case 2:
                nextPos += new Vector2Int(1, 0);
                break;
            case 3:
                nextPos += new Vector2Int(-1, 0);
                break;
        }
        if (nextPos.x < 0)
            nextPos = new Vector2Int(9, nextPos.y);
        if (nextPos.x > 9)
            nextPos = new Vector2Int(0, nextPos.y);
        if (nextPos.y < 0)
            nextPos = new Vector2Int(nextPos.x, 9);
        if (nextPos.y > 9)
            nextPos = new Vector2Int(nextPos.x, 0);

        if (snakeQueue.Contains(nextPos))
        {
            //print("Hello!!!");
            
            None();
            startSnake = false;
            return;
        }
        snakeQueue.Enqueue(nextPos);
        head = nextPos;

        if (nextPos == fruit)
            newFruit();
        else
            snakeQueue.Dequeue();

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (snakeQueue.Contains(new Vector2Int(i, j)))
                {
                    legoManager.legoInput[i, j] = 0.15f * Mathf.Sin(timer + i + j + frame) + 0.3f;
                    legoManager.legoColor[i, j].SetColor("_Color", Color.green);
                }
                else if (new Vector2Int(i, j) == fruit)
                {
                    if (frame % 10 == 0)
                    {
                        legoManager.legoInput[i, j] = 0.8f;
                        previous_frame = frame;
                    }
                    else if (previous_frame == frame + 2)
                        legoManager.legoInput[i, j] = 0.6f;
                    else
                        legoManager.legoInput[i, j] = 0.5f;
                    legoManager.legoColor[i, j].SetColor("_Color", Color.red);
                }
                else
                {
                    legoManager.legoInput[i, j] = 0;
                    legoManager.legoColor[i, j].SetColor("_Color", Color.white);
                }
            }
        }
        timer += Time.fixedDeltaTime;
    }

    private void snakeInit()
    {
        frame = 0;
        snakeQueue = new Queue<Vector2Int>();
        snakeQueue.Enqueue(new Vector2Int(0, 0));
        snakeQueue.Enqueue(new Vector2Int(0, 1));
        snakeQueue.Enqueue(new Vector2Int(0, 2));
        head = new Vector2Int(0, 2);
        for(int i = 0; i < 100; i ++)
        {
            int rand = Random.Range(0, 100);
            Vector2Int temp = allPos[i];
            allPos[i] = allPos[rand];
            allPos[rand] = temp;
        }
        fruitIndex = 0;
        direction = 1;
        directionInLastFrame = 1;
        newFruit();
    }

    private void newFruit()
    {
        int counter = 0;
        while(snakeQueue.Contains(allPos[fruitIndex]))
        {
            fruitIndex = (fruitIndex + 1) % 100;
            counter++;
            if (counter >= 100)
            {
                startSnake = false;
                None();
                return;
            }
        }
        fruit = allPos[fruitIndex];
    }

    private float IndexToHeight(Vector2 spherePos, int i, int j)
    {
        return (new Vector2(legoManager.legoArray[i, j].transform.position.x, legoManager.legoArray[i, j].transform.position.z) - spherePos).magnitude;
    }
}


