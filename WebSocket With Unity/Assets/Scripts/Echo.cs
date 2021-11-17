using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;

public class Echo : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Send(e.Data);
    }
}