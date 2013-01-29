﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Samba.Infrastructure
{
    public class MessagingServerObject : MarshalByRefObject, ISubject
    {
        private readonly IList<IObserver> _clients = new List<IObserver>();

        public void SetValue(string clientData)
        {
            Notify(clientData, 0);
        }

        public void Attach(IObserver client)
        {
            Console.WriteLine("observer bağlandı.");
            _clients.Add(client);
        }

        public void Detach(IObserver client)
        {
            _clients.Remove(client);
        }

        public void Ping()
        { }

        public bool Notify(string clientData, short objState)
        {
            for (var i = _clients.Count - 1; i >= 0; i--)
            {
                try
                {
                    _clients[i].Update(this, clientData, objState);
                }
                catch (Exception)
                {
                    _clients.RemoveAt(i);
                }
            }
            return true;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public int GetConnectionCount()
        {
            return _clients.Count;
        }
    }
}