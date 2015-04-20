using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Collections.Generic;




public class SockThread
{
	Socket client;
	public System.Net.IPEndPoint remoteEP;
    
	private string ip;
	private int port;
    
	private byte[] header = new byte[4];
	
	static bool lock1 = false;
	static bool lock2 = false;
	
	public  LinkedList<byte[]> OutQueue = new LinkedList<byte[]> ();
	public  LinkedList<byte[]> InQueue = new LinkedList<byte[]> ();
	
	public  LinkedList<byte[]> tmp1 = new LinkedList<byte[]> ();
	public  LinkedList<byte[]> tmp2 = new LinkedList<byte[]> ();
	
	private System.Object lockobj = new System.Object ();

	
	

	public void Send (byte[] msg)
	{
		tmp1.AddLast (msg);
		try {
			if (Monitor.TryEnter (this)) {
				lock1 = true;
				movetoq ();
			}

		} finally {
			if (lock1) {
				lock1 = false;
				Monitor.Exit (this);
			}
		}
	}
	
	public void Recv ()
	{
		try {
			if (Monitor.TryEnter (lockobj)) {
				lock2 = true;
				movetoq2 ();
			}
			
		} finally {
			if (lock2) {
				lock2 = false;
				Monitor.Exit (lockobj);
			}
		}
	}
	
	private void movetoq ()
	{
		foreach (var a in tmp1) {
			InQueue.AddLast (a);
		}
		tmp1.Clear ();
	}
	
	private void movetoq2 ()
	{
		foreach (var a in tmp2) {
			OutQueue.AddLast (a);
		}
		tmp2.Clear ();
	}
	
	public byte[] NextMsg {
		get {
			if (InQueue.Count <= 0) {
				return null;
			}
			byte[] msg;
			Monitor.Enter (this);
			msg = InQueue.First.Value;
			InQueue.RemoveFirst ();
			Monitor.Exit (this);
			return msg;
		}
	}
	
	public void ReturnMsg (byte[] Msg)
	{
		Monitor.Enter (lockobj);
		tmp2.AddLast (Msg);
		Monitor.Exit (lockobj);
	}
   		
	public void StartWork ()
	{
		ThreadPool.QueueUserWorkItem (new WaitCallback (NetProc), this);
	}
    
	public bool IsAlive ()
	{
		if (client == null) {
			Debug.Log ("client is null");
			return false;
		}
		if ((client.Poll (0, SelectMode.SelectWrite)) && (!client.Poll (0, SelectMode.SelectError))) {
			return true;
		}
		return false;
	}
    
	static void NetProc (System.Object stateInfo)
	{
		SockThread sock = (SockThread)stateInfo;
		Debug.Log ("log in other thread");
        
		sock.Connect ();
		if (sock.client == null) {
			return;
		}
	
  
		ThreadPool.QueueUserWorkItem (new WaitCallback (RecvProc), sock);
		while (sock.IsAlive()) {
			var msg = sock.NextMsg;
			try {
				if (sock.client.Available > 0) {
                  
				}
				if (msg != null) {
					sock.tcpSend (msg);
				}
			} catch (Exception ex) {
				Debug.Log (ex);
				break;
			}
		}
		
		Debug.Log ("send exit ");
	}
	
	static void RecvProc (System.Object stateInfo)
	{
		SockThread sock = (SockThread)stateInfo;
		while (sock.IsAlive()) {
			try {
				if (sock.client.Available > 0) {
					var bytes = sock.tcpRecv ();
					sock.ReturnMsg (bytes);
				}
			} catch (Exception ex) {
				Debug.Log (ex);
				break;
			}
		}	
		Debug.Log ("recv exit");
	}
    
	public SockThread (string ip = "127.0.0.1", int port =  8001)
	{
		this.ip = ip;
		this.port = port;
	}
        
	public void  Close ()
	{
		client.Shutdown (SocketShutdown.Both);
		client.Close ();   
		client = null;
	}
    
	public void Connect ()
	{
		this.client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse (ip);
		remoteEP = new IPEndPoint (ipAdd, port);
		Debug.Log ("try to connect");
		try {
			client.Connect (remoteEP);
		} catch (Exception ex) {
			this.client = null;
			Debug.Log (ex);
			return;
		}
		
		Debug.Log ("Socket connected to : " + client.RemoteEndPoint.ToString ());
        
	}
    
	public void tcpSend (byte[] data)
	{
		int len = data.Length;
		int l = client.Send (data, 0, data.Length, SocketFlags.None);
		while (l < len) {
			l += client.Send (data, l, len - l, SocketFlags.None);
		}
	}
    
	public byte[] tcpRecv ()
	{              
		receivelen (header, header.Length);
		int len = (int)net2host (header);
		byte[] buf = new byte[len];
		receivelen (buf, len);
		return buf;
	}
	
	private void receivelen (byte[] bs, int len)
	{
		int l = client.Receive (bs, 0, len, SocketFlags.None);
		while (l < len) {
			Debug.Log ("still receive");
			l += client.Receive (bs, l, len - l, SocketFlags.None);
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