using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Text;
using UnityEngine;



public class State
{
    public Socket client = null;
    public bool Done = false;
    public Exception error = null;
    public byte[] buf;
    public int len;
    public int index;
    
    public void Reset ()
    {
        this.Done = false;
        this.error = null;
        this.index = 0;
    }
}


public class SockAsync
{

    public byte[] ReceiveData;
    Socket client;
    public System.Net.IPEndPoint remoteEP;
    
    public State connState;
    public State sendState;
    public State recvState;

    private byte[] header = new byte[4];
    
    public SockAsync ()
    {
        connState = new State{};
        sendState = new State{};
        recvState = new State{};
    }
    
    public bool Islive ()
    {   
        return !(!(client != null) || (client.Poll (1000, SelectMode.SelectRead) && (client.Available == 0)) || !client.Connected);
    }

    public static bool Islive (Socket client)
    {   
        return !(!(client != null) || (client.Poll (1000, SelectMode.SelectRead) && (client.Available == 0)) || !client.Connected);
    }
    
    public void  Close ()
    {
        client.Close ();   
    }

    public IEnumerator Connect (string ip = "127.0.0.1", int port =  8888)
    {
        this.client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        connState.client = this.client;
        sendState.client = this.client;
        recvState.client = this.client;
        
        this.connState.Reset ();
        
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse (ip);
        remoteEP = new IPEndPoint (ipAdd, port);
        client.BeginConnect (remoteEP, new AsyncCallback (ConnectCallback), this.connState);
        
        while (!this.connState.Done) {
            yield return null;
        }
        
    }
    
    public IEnumerator Send (byte[] data)
    {
        this.sendState.Reset ();
        if (!Islive ()) {
            this.sendState.Done = true;
            this.sendState.error = new SocketException ((int)SocketError.NotConnected);
        }

        try {
            client.BeginSend (data, 0, data.Length, SocketFlags.None,
                              new AsyncCallback (SendCallback), this.sendState);
            
        } catch (SocketException ex) {
            this.sendState.error = ex;
            this.sendState.Done = true;
        }
        
        while (!this.sendState.Done) {
            yield return null;
        }
    }
    
    public IEnumerator Recv ()
    {              
    
        this.recvState.Reset ();
        this.ReceiveData = null;
        this.recvState.buf = header;
        this.recvState.len = 4;
        
        try {
            client.BeginReceive (header, 0, 4, 0, new AsyncCallback (RecvCallback), this.recvState);
        } catch (SocketException ex) {
            Debug.Log (ex);
            this.recvState.error = ex;
            this.recvState.Done = true;            
        }

        while (!this.recvState.Done) {
            yield return null;
        }
        
        if (this.recvState.error != null) {
            yield break;
        }
            
        this.recvState.Reset ();

        try {
            int len = (int)net2host (this.recvState.buf);
            Debug.Log ("body len: " + len);
            this.recvState.buf = new byte[len];
            this.recvState.len = len;
            this.ReceiveData = null;
            client.BeginReceive (this.recvState.buf, 0, len, 0, new AsyncCallback (RecvCallback), this.recvState);
        } catch (SocketException ex) {
            Debug.Log (ex);
            this.recvState.error = ex;
            this.recvState.Done = true;            
        }
            
            
        while (!this.recvState.Done) {
            yield return null;
        }
        
        this.ReceiveData = this.recvState.buf;
            
    }
    
    private static void ConnectCallback (IAsyncResult ar)
    {
        State cs = (State)ar.AsyncState;
        try {
            Debug.Log ("Socket connected to : " + cs.client.RemoteEndPoint.ToString ());
            cs.client.EndConnect (ar);
            cs.Done = true;
        } catch (Exception e) {
            Debug.Log (e.ToString ());
            cs.error = e;
            cs.Done = true;
        }
    }
    
    private static void SendCallback (IAsyncResult ar)
    {
        State state = ar.AsyncState as State;
        try {
            state.client.EndSend (ar);
            //int bytesSent = state.client.EndSend (ar);
            //Debug.Log ("Sent bytes to server." + bytesSent);
            state.Done = true;
        } catch (Exception e) {
            Debug.Log (e.ToString ());
            state.error = e;
            state.Done = true;
        }
    } 
    
    private static void RecvCallback (IAsyncResult ar)
    {
        State state = ar.AsyncState as State;
        try {
            int recvlen = state.client.EndReceive (ar);
            if (recvlen < state.len) {
                state.index += recvlen;
                state.len -= recvlen;
                if(!Islive(state.client)){
                    state.error = new SocketException((int)SocketError.NotConnected);
                    state.Done = true; 
                   return;
                }
                state.client.BeginReceive (state.buf, state.index, state.len, 0, new AsyncCallback (RecvCallback), state);
                Debug.Log("still receive");
            } else {
                Debug.Log ("receive done ");
                state.Done = true;
            }
            
            //Debug.Log ("recv bytes to server." + recvlen);
        } catch (Exception e) {
            Debug.Log (e.ToString ());
            state.error = e;
            state.Done = true;
        }
    }
    
    public static byte[] host2net (UInt32 value)
    {
        return BitConverter.GetBytes (ReverseBytes (value));
    }
    
    public static UInt32 net2host (byte[] value)
    {
        return ReverseBytes ((UInt32)BitConverter.ToInt32 (value, 0));
    }
    
    public static UInt16 ReverseBytes (UInt16 value)
    {
        return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
    }

    public static UInt32 ReverseBytes (UInt32 value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
            (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }

    public static UInt64 ReverseBytes (UInt64 value)
    {
        return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
            (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
            (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
            (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
    }
}