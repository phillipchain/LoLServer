﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeagueSandbox.GameServer.Logic.Chatbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeagueSandbox.GameServer.Logic.Chatbox.ChatboxManager;
using LeagueSandbox.GameServer.Logic.Chatbox.Commands;

namespace GameServerTests
{
    [TestClass()]
    public class ChatboxManagerTests
    {
        [TestMethod()]
        public void AddCommandTest()
        {
            var game = new TestHelpers.DummyGame();
            game.LoadChatboxManager();

            var command = new HelpCommand("ChatboxManagerTestsTestCommand", "", game.ChatboxManager);
            var result = game.ChatboxManager.AddCommand(command);
            Assert.AreEqual(true, result);
            result = game.ChatboxManager.AddCommand(command);
            Assert.AreEqual(false, result);
        }
    }
}