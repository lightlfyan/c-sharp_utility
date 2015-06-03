package main

import (
	"io"
	"log"
	"net"
	// "time"
)

var policy = `
<?xml version="1.0"?>
<!DOCTYPE cross-domain-policy SYSTEM
"http://www.adobe.com/xml/dtds/cross-domain-policy.dtd">
<cross-domain-policy>
<site-control permitted-cross-domain-policies="master-only"/>
<allow-access-from domain="*" to-ports="*"/>
</cross-domain-policy>
`

var policyRequest = "<policy-file-request/>"
var m_request = []byte(policyRequest)

func main() {
	tcpAddr, err := net.ResolveTCPAddr("tcp4", ":8000")
	if err != nil {
		log.Fatal("err")
	}
	listener, _ := net.ListenTCP("tcp", tcpAddr)

	for {
		conn, err := listener.AcceptTCP()
		if err != nil {
			log.Println("accept failed", err)
			continue
		}
		log.Println("connected: ", conn.RemoteAddr())
		go handleClient(conn)
	}
}

func handleClient(conn net.Conn) {
	defer conn.Close()
	recvPolicyReq(conn)
}

func recvPolicyReq(conn net.Conn) {
	policyBuf := make([]byte, len(m_request))
	_, err := io.ReadFull(conn, policyBuf)
	if err != nil {
		panic(err)
	}

	log.Println(string(policyBuf[:]))
	conn.Write([]byte(policy))

}
