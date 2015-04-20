using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class UserToken
{
	public Socket sock;
	public int offset;
	public byte[] buf;
	public bool isBody;
	
	public int Len {
		get {
			return buf.Length - offset;
		}
	}
}


public class SockIOCP : MonoBehaviour
{

	Socket sock; 
    
	IPEndPoint remoteIPeP;
	
	void Start ()
	{
		Security.PrefetchSocketPolicy ("127.0.0.1", 8000);
		Debug.Log (Thread.CurrentThread.ManagedThreadId);
		ThreadPool.SetMaxThreads (10, 10);
  
	}
    
	// Update is called once per frame
	void Update ()
	{
//		if (!IsAlive ()) {
//			Debug.Log ("===============");
//		}
	}
	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, 100, 20), "connect")) {
			Connect ("127.0.0.1", 8001);
		}
        
		if (GUI.Button (new Rect (0, 20, 100, 20), "send")) {
			Send ();
		}
		if (GUI.Button (new Rect (0, 40, 100, 20), "down")) {
			Close ();
		}
            
	}
	
	public bool IsAlive ()
	{
		if (sock == null) {
			return false;
		}
		if ((sock.Poll (0, SelectMode.SelectWrite)) && (!sock.Poll (0, SelectMode.SelectError))) {
			return true;
		}
		return false;
	}
    
	void Connect (string ip, int port)
	{
		if (IsAlive ()) {
			return;
		}
		sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		SocketAsyncEventArgs e = new SocketAsyncEventArgs ();
		IPEndPoint ipEnd = new IPEndPoint (IPAddress.Parse (ip), port);
		remoteIPeP = ipEnd;
		e.RemoteEndPoint = ipEnd;
		e.UserToken = sock;
		e.Completed += new EventHandler<SocketAsyncEventArgs> (IO_Completed);
		sock.ConnectAsync (e);
	}
	
	void Send ()
	{
		SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs ();
		socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs> (IO_Completed);
		socketEventArg.RemoteEndPoint = remoteIPeP;
		socketEventArg.UserToken = sock;
		
		var buffer = new Msg ("test12345").ToBytes ();
		socketEventArg.SetBuffer (buffer, 0, buffer.Length);
		
		bool willRaiseEvent = sock.SendAsync (socketEventArg);

		if (!willRaiseEvent) {
			Debug.Log ("will raise send");
			ProcessSend (socketEventArg);
		}
		
		Debug.Log ("send done");
	
	}
	
	void Close ()
	{
		if (!IsAlive ()) {
			return;
		}
		
		sock.Shutdown (SocketShutdown.Both);
		sock = null;
	}
	
	static void IO_Completed (object sender, SocketAsyncEventArgs e)
	{
		switch (e.LastOperation) {
		case SocketAsyncOperation.Connect:
			ProcessConnect (e);
			break;
            
		case SocketAsyncOperation.Receive:
			ProcessReceive (e);
			break;
            
		case SocketAsyncOperation.Send:
			ProcessSend (e);
			break;	
		default:
			Debug.Log ("error: " + e.LastOperation);
			throw new Exception ("Invalid operation completed");
		}
	}
	
	private static void ProcessConnect (SocketAsyncEventArgs e)
	{
		if (e.SocketError == SocketError.Success) {
			// Successfully connected to the server
			Debug.Log ("connect success");
			
			var sock = e.UserToken as Socket;
			e.UserToken = new UserToken{
				sock = sock,
				offset = 0,
				isBody = false,
				buf = new byte[4]
			};
			
			var ut = (UserToken)e.UserToken;
			e.SetBuffer (ut.buf, ut.offset, ut.Len);
			bool willRaiseEvent = ut.sock.ReceiveAsync (e);
			if (!willRaiseEvent) {
				ProcessReceive (e);
			}
            
		} else {
			throw new SocketException ((int)e.SocketError);
		}
	}
	
	private static void ProcessReceive (SocketAsyncEventArgs e)
	{
		Debug.Log ("ProcessReceive: " + Thread.CurrentThread.ManagedThreadId);
		if (e.BytesTransferred <= 0) {
			var ut = (UserToken)e.UserToken;
			ut.sock.Close ();
			Debug.Log ("lost connet");
			return;
		}
	
		if (e.SocketError == SocketError.Success) {
			var ut = (UserToken)e.UserToken;
			ut.offset += e.BytesTransferred;
			
			if (ut.Len > 0) {
				e.SetBuffer (ut.buf, ut.offset, ut.Len);
				if (!ut.sock.ReceiveAsync (e)) {
					ProcessReceive (e);
				}
				return;
			}
			
			if (!ut.isBody) {
				int len = (int)net2host (ut.buf);
				ut.buf = new byte[len];
				ut.offset = 0;
				ut.isBody = true;
				e.SetBuffer (ut.buf, ut.offset, ut.Len);
				
				
				if (!ut.sock.ReceiveAsync (e)) {
					ProcessReceive (e);
				}
				return;
			}
			
			Debug.Log (Encoding.UTF8.GetString (ut.buf));
			
			ut.buf = new byte[4];
			ut.offset = 0;
			ut.isBody = false;
			e.SetBuffer (ut.buf, ut.offset, ut.Len);
			
			if (!ut.sock.ReceiveAsync (e)) {
				ProcessReceive (e);
			}
			
		} else {
			throw new SocketException ((int)e.SocketError);
		}
	}
	static void ProcessSend (SocketAsyncEventArgs e)
	{
		if (e.SocketError == SocketError.Success) {
			Debug.Log ("send succ");
		} else {
			Debug.Log ("send error");
			
			throw new SocketException ((int)e.SocketError);
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