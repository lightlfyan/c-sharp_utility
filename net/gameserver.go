package main

import (
	"encoding/binary"
	"io"
	"log"
	"net"
)

func main() {
	tcpAddr, err := net.ResolveTCPAddr("tcp4", ":8001")
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

	header := make([]byte, 4)

	for {
		data, err := recv(header, conn)
		if err != nil {
			break
		}
		log.Println(string(data[:]))

		msg := append(header, data...)
		conn.Write(msg)
	}
	log.Println("conn exit")
}

func recv(header []byte, conn net.Conn) ([]byte, error) {
	n, err := io.ReadFull(conn, header)
	if err != nil {
		log.Println("error receiving header, bytes:", n, "reason:", err)
		return nil, err
	}

	size := binary.BigEndian.Uint32(header)
	data := make([]byte, size)
	n, err = io.ReadFull(conn, data)
	if err != nil {
		log.Println("error receiving msg, bytes:", n, "reason:", err)
		return nil, err
	}
	return data, nil
}
