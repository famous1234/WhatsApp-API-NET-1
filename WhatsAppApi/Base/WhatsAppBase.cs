﻿using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppApi.Helper;
using WhatsAppApi.Settings;

namespace WhatsAppApi
{
    public class WhatsAppBase : WhatsEventBase
    {
        public long m_LastSentInfo = 0;

        protected ProtocolTreeNode uploadResponse;

        protected AccountInfo accountinfo;

        public static bool Debug;

        public static bool DebugOutBound;

        protected string password;

        protected bool hidden;

        protected CONNECTION_STATUS loginStatus;

        public CONNECTION_STATUS ConnectionStatus
        {
            get
            {
                return this.loginStatus;
            }
        }

        protected KeyStream outputKey;

        protected object messageLock = new object();

        protected List<ProtocolTreeNode> messageQueue;

        protected string name;

        protected string phoneNumber;

        protected BinTreeNodeReader reader;

        protected int timeout = 300000;

        protected WhatsNetwork whatsNetwork;

        public static readonly Encoding SysEncoding = Encoding.UTF8;

        protected byte[] _ChallengeBytes;

        protected BinTreeNodeWriter BinWriter;

        protected void ConstructBase(string phoneNum, string imei, string nick, bool debug, bool hidden)
        {
            this.messageQueue = new List<ProtocolTreeNode>();
            this.phoneNumber = phoneNum;
            this.password = imei;
            this.name = nick;
            this.hidden = hidden;
            WhatsApp.Debug = debug;
            this.reader = new BinTreeNodeReader();
            this.loginStatus = CONNECTION_STATUS.DISCONNECTED;
            this.BinWriter = new BinTreeNodeWriter();
            this.whatsNetwork = new WhatsNetwork(WhatsConstants.WhatsAppHost, WhatsConstants.WhatsPort, this.timeout);
        }

        public void Connect()
        {
            try
            {
                this.whatsNetwork.Connect();
                this.loginStatus = CONNECTION_STATUS.CONNECTED;
                //Success
                this.FireOnConnectSuccess();
            }
            catch (Exception e)
            {
                this.FireOnConnectFailed(e);
            }
        }

        public void Disconnect(Exception ex = null)
        {
            this.whatsNetwork.Disconenct();
            this.loginStatus = CONNECTION_STATUS.DISCONNECTED;
            this.FireOnDisconnect(ex);
        }

        protected byte[] EncryptPassword()
        {
            return Convert.FromBase64String(this.password);
        }

        public AccountInfo GetAccountInfo()
        {
            return this.accountinfo;
        }

        public ProtocolTreeNode[] GetAllMessages()
        {
            ProtocolTreeNode[] tmpReturn = null;
            lock (messageLock)
            {
                tmpReturn = this.messageQueue.ToArray();
                this.messageQueue.Clear();
            }
            return tmpReturn;
        }

        protected void AddMessage(ProtocolTreeNode node)
        {
            lock (messageLock)
            {
                this.messageQueue.Add(node);
            }
        }

        public bool HasMessages()
        {
            if (this.messageQueue == null)
                return false;
            return this.messageQueue.Count > 0;
        }

        protected void SendData(byte[] data)
        {
            try
            {
                this.whatsNetwork.SendData(data);
            }
            catch (ConnectionException)
            {
                this.Disconnect();
            }
        }

        public void SendNode(ProtocolTreeNode node)
        {
            m_LastSentInfo = DateTime.UtcNow.ToFileTime();
            this.SendData(this.BinWriter.Write(node));
        }
    }
}