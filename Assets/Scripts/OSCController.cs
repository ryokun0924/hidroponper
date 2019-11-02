using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;
using UniRx;
using System;
using UnityEngine.SceneManagement;

//参考　https://qiita.com/yohki/items/496d9ad345fdd1c9b779


public class OSCController : SingletonMonoBehaviour<OSCController>
{

    private Queue receiveQueue;

    private Subject<int> showSubject = new Subject<int>();
    private Subject<Vector2> itemSubject = new Subject<Vector2>();

    private Subject<int> scoreSubject = new Subject<int>();

    private Subject<int> rankSubject = new Subject<int>();

    private Target activeTarget;
    private ItemController activeItem;


    public IObservable<int> OnShowSignal
    {
        get { return showSubject; }
    }

    public IObservable<Vector2> OnItemShowSignal
    {
        get { return itemSubject; }
    }

    public IObservable<int> OnScoreShowSignal
    {
        get { return scoreSubject; }
    }

    public IObservable<int> OnRankShowSignal
    {
        get { return rankSubject; }
    }

    public void sendHitTarget(int _duration){
         OSCHandler.Instance.SendMessageToClient("tabletSender","/hit",1000);
    }

    public void sendHitItem(int _kind){
         OSCHandler.Instance.SendMessageToClient("tabletSender","/hititem",_kind);
    }


    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        receiveQueue = new Queue();
        receiveQueue = Queue.Synchronized(receiveQueue);
        OSCHandler.Instance.Init();
        OSCHandler.Instance.PacketReceiveEvent += OnPacketReceived;
    }


    void Update()
    {

        while (0 < receiveQueue.Count)
        {
            OSCPacket packet = receiveQueue.Dequeue() as OSCPacket;
            if (packet.IsBundle())
            {
                // OSCBundleの場合
                OSCBundle bundle = packet as OSCBundle;
                foreach (OSCMessage msg in bundle.Data)
                {
                    // メッセージの中身にあわせた処理
                    string address = msg.Address;
                    string data = "   (";
                    for( int i = 0; i < msg.Data.Count; i ++){
                        data += msg.Data[i].ToString();
                        data += ",";
                    }
                    data += ")";
                    print(address + data);
                  
                    switch (address)
                    {
                        //開始
                        case "/start":
                            SceneManager.LoadScene("countdown");
                            break;

                        //的出現
                        case "/up":


                            int targetDuration = (int)msg.Data[0];
                            showSubject.OnNext(targetDuration);


                            break;

                        //アイテム出現
                        case "/item":
                            int itemdurationInt = (int)msg.Data[0];
                            int kindInt = (int)msg.Data[1];

                            float kind = (float)kindInt;
                            float itemduration = (float)itemdurationInt;
                            itemSubject.OnNext(new Vector2(kind, itemduration));
                            break;

                        //難易度変更
                        case "/level":
                            break;

                        //終了
                        case "/end":
                            SceneManager.LoadScene("result");
                            break;

                        //点数表示
                        case "/result":
                            scoreSubject.OnNext((int)msg.Data[0]);
                            break;

                        //ランク表示
                        case "/resultother":
                            rankSubject.OnNext((int)msg.Data[0]);
                            break;

                        //初期化
                        case "/init":
                            SceneManager.LoadScene("init");
                            break;

                    }

                }
            }
            else
            {
                // OSCMessageの場合はそのまま変換
                OSCMessage msg = packet as OSCMessage;
                // メッセージの中身にあわせた処理

            }
        }


        if (Input.GetKeyDown("space"))
        {
            showSubject.OnNext(3000);
            
            OSCHandler.Instance.SendMessageToClient("tabletSender","/hit",1111);

        }

  

    }

    void OnPacketReceived(OSCServer server, OSCPacket packet)
    {
        receiveQueue.Enqueue(packet);
    }

}
