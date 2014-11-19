using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;

public class GameManager {
    public GameNet gameNet = null;

    public void Start(){
        gameNet = new GameNet();
        gameNet.start();
        gameNet.send("hello");

        try{

            gameNet.synctest();
            } catch (System.Net.Sockets.SocketException  e){
                Console.WriteLine("catch: {0}", e);
            }
        // in u3d
        //StartCoroutine(conrecv());
    }

    public void Update(){
        // gameNet.recv();
    }
}


public class StateObject {
    // Client socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 256;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}


public class GameNet{
    //public Socket sock;
    public static ManualResetEvent connectDone = new ManualResetEvent(false);
    public static ManualResetEvent sendDone = new ManualResetEvent(false);
    public static ManualResetEvent receiveDone = new ManualResetEvent(false);

    public byte[] buffer = new byte[1024];
    Socket m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);

    public void start(){
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("127.0.0.1");
        System.Net.IPEndPoint remoteEP = new IPEndPoint (ipAdd, 8888);
        //m_socClient.Connect(remoteEP);
        Connect(remoteEP, m_socClient);
        //m_socClient.Close();
    }

    public void recv(){
        Receive(m_socClient);
    }

    public void send(string data){
        Send(m_socClient, data);
    }

    public static void Connect(EndPoint remoteEP, Socket client) {
        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client );
        connectDone.WaitOne();
        Console.WriteLine("Connect done!!!");
        //client.IOControl(IOControlCode.KeepAliveValues, KeepAliveSetting(1, 5000, 5000), null);
    }

    private static byte[] KeepAliveSetting(uint a, uint b, uint c){
        byte[] inOptionValues = new byte[sizeof(uint) * 3];
        BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
        BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, sizeof(uint));//多长时间开始第一次探测
        BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, sizeof(uint) * 2);
        return inOptionValues;
    }

    private static void ConnectCallback(IAsyncResult ar) {
        try {
        // Retrieve the socket from the state object.
        Socket client = (Socket) ar.AsyncState;

        // Complete the connection.
        client.EndConnect(ar);

        Console.WriteLine("Socket connected to {0}",
            client.RemoteEndPoint.ToString());

        // Signal that the connection has been made.
        connectDone.Set();
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Send(Socket client, String data) {
        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        client.BeginSend(byteData, 0, byteData.Length, SocketFlags.None,
            new AsyncCallback(SendCallback), client);
    }

    private static void SendCallback(IAsyncResult ar) {
        try {
        // Retrieve the socket from the state object.
        Socket client = (Socket) ar.AsyncState;

        // Complete sending the data to the remote device.
        int bytesSent = client.EndSend(ar);
        Console.WriteLine("Sent {0} bytes to server.", bytesSent);

        // Signal that all bytes have been sent.
        sendDone.Set();
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Receive(Socket client) {
        try {
        // Create the state object.
        StateObject state = new StateObject();
        state.workSocket = client;

        // Begin receiving the data from the remote device.
        client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReceiveCallback), state);

        receiveDone.WaitOne();
        Console.WriteLine("response: {0}", state.sb.ToString());

        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ReceiveCallback( IAsyncResult ar ) {
        try {
        // Retrieve the state object and the client socket 
        // from the asynchronous state object.
        StateObject state = (StateObject) ar.AsyncState;
        Socket client = state.workSocket;
        // Read data from the remote device.
        int bytesRead = client.EndReceive(ar);
        if (bytesRead > 0) {
            // There might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));
                //  Get the rest of the data.
                client.BeginReceive(state.buffer,0,StateObject.BufferSize,0,
                    new AsyncCallback(ReceiveCallback), state);
                } else {
            // All the data has arrived; put it in response.
            // Signal that all bytes have been received.
            receiveDone.Set();
        }
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private IEnumerator conrecv(){
        //float delaySeconds = 5.0f;
        var bytes = new byte[1024];
        string data = null;

        while(true){
            //print("tick " + Time.time);
            int len = m_socClient.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, len);
            yield return data;
            data = null;
        }
    }

    public  void synctest(){
        int i = 0;
        var bytes = new byte[256];
        string data = null;

        while(i < 10){
            int len = m_socClient.Receive(bytes);
            data += System.Text.Encoding.Default.GetString(bytes, 0, len);
            Console.WriteLine("synctest: {0}, {1}", i, data);
            data = null;
            m_socClient.Send(System.Text.Encoding.Default.GetBytes(i.ToString()));
            i++;
        }
    }

}

class Program{
    static public void Main(){
        GameManager gameManager = new GameManager();
        gameManager.Start();
        gameManager.gameNet.send("hello");
        gameManager.Update();

        Console.WriteLine("ok");
    }
}