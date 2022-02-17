using NUnit.Framework;
using IntroSE.Kanban;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using Moq;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace UnitTest
{
    public class Tests
    {

        Service s;
        //users
        private readonly string user0 = "notRegistered@walla.com";
        private readonly string user1 = "user1@gmail.com";
        private readonly string user2 = "user2@walla.co.il";
        private readonly string user3 = "user3@post.bgu.ac.il";
        private readonly string user4 = "notRegistered2@walla.com";
        //passwards for users
        private readonly string pass1 = "Aa123456";
        private readonly string pass2 = "Aa1234567";
        private readonly string pass3 = "Aa1234568";
        //boards
        private readonly string board1 = "board1";
        private readonly string board2 = "board2";
        private readonly string board3 = "board3";
        private readonly string board4 = "board4";
        //dates
        private readonly DateTime dueDate1 = DateTime.Today.AddDays(1);
        private readonly DateTime dueDate2 = DateTime.Today.AddDays(2);
        private readonly DateTime dueDate3 = DateTime.Today.AddDays(3);
        //tasks
        private STask task1;
        private STask task2;
        private STask task3;

        [SetUp]
        public void SetUp()
        {
            CleanUP();
            s = new Service();
            s.Register(user1, pass1);
            s.Register(user2, pass2);
            s.Register(user3, pass3);
            s.Login(user1, pass1);
            s.Login(user2, pass2);
            s.Login(user3, pass3);
            s.AddBoard(user1, board1);
            s.AddBoard(user2, board2);
            s.AddBoard(user3, board3);
            s.JoinBoard(user2, user1, board1);
            s.JoinBoard(user3, user1, board1);
            task1 = s.AddTask(user1, user1, board1, "task1", "desc1", dueDate1).Value;
            task2 = s.AddTask(user2, user2, board2, "task2", "desc2", dueDate2).Value;
            task3 = s.AddTask(user3, user3, board3, "task3", "desc3", dueDate3).Value;

            //user0 - not registerd
            //user1 - registerd, logged in, member of board1, assignee of task1, 0 task in progress
            //user2 - registerd, logged in, member of board1 and board2, assignee of task2, 0 task in progress
            //user3 - registerd, logged in, member of board1 and board3, assignee of task3, 0 task in progress
            //board1 - creator: user1. members: user1, user2, user3. column limits: none. tasks: backlog/task1
            //board2 - creator: user2. members: user2. column limits: none. tasks: backlog/task2
            //board3 - creator: user3. members: user3. column limits: none. tasks: backlog/task3
            //task1 - board: user1,board1. column: 0. title: task1. desc: desc1. due date: dueDate1. assignee: user1
            //task2 - board: user2,board2. column: 0. title: task2. desc: desc2. due date: dueDate2. assignee: user2
            //task3 - board: user3,board3. column: 0. title: task3. desc: desc3. due date: dueDate3. assignee: user3
        }

        [TearDown]
        public void CleanUP()
        {
            s = new Service();
            s.DeleteData();
        }
       
        public void RemoveBoard()
        {
            SetUp();
            //deleting from not exiting user
            Response e1=s.RemoveBoard(user1, user1,"board4");
            Assert.AreEqual("Board does not exist", e1.ErrorMessage);
            Response e2 = s.RemoveBoard(user1, user1," board5");
            Assert.AreEqual("Board does not exist", e2.ErrorMessage);
            //deleting existing board
            s.AddBoard(user1,  board4);
            Response e3 = s.RemoveBoard(user1, user1, board1);
            Assert.AreEqual(null, e3.ErrorMessage);
            Response e4 = s.RemoveBoard(user2, user2, board2);
            Assert.AreEqual(null, e4.ErrorMessage);
            Response e5 = s.RemoveBoard(user1, user1, board4);
            Assert.AreEqual(null, e5.ErrorMessage);
            CleanUP();
        }
        [Test]
        public void AddColumn()
        {
            SetUp();
            //test adding a column
            Response e1 = s.AddColumn(user1, user1, board1, 3, "blabla");
            Assert.AreEqual(null, e1.ErrorMessage);
            s.MoveColumn(user1, user1, board1, 3, -1);
            s.MoveColumn(user1, user1, board1, 2, -1);
            s.MoveColumn(user1, user1, board1, 1, -1);
            //adding column with the same name-suppose to work
            Response e2 = s.AddColumn(user1, user1, board1, 2, "blabla");
            Assert.AreEqual(null, e2.ErrorMessage);
            //adding column at the end of the list
            Response e3 = s.AddColumn(user1, user1, board1, 5, "nadavcol");
            Assert.AreEqual(null, e3.ErrorMessage);
            CleanUP();
        }
        public void RemoveColumn()
        {
            SetUp();
            //removing in progress column
            Response e1 = s.RemoveColumn(user1, user1, board1, 1);
            Assert.AreEqual(null, e1.ErrorMessage);
            //removing a column when have only 2 colimns left
            Response e2 = s.AddColumn(user1, user1, board1, 1, "blabla");
            Assert.AreEqual("cant delete a column when have only 2 left", e2.ErrorMessage);
            //trying to remove when column ordinal is wrong
            Response e3 = s.RemoveColumn(user2, user2, board2, 5);
            Assert.AreEqual("column ordinal not found", e3.ErrorMessage);
            CleanUP();
        }

        public void RenameColumn()
        {
            SetUp();
            //test renaming a column
            Response e1 = s.RenameColumn(user1, user1, board1, 1, "jiglipaf");
            Assert.AreEqual(null, e1.ErrorMessage);
            //test renaming a column
            Response e2 = s.RenameColumn(user1, user1, board1, 2, "jiglipaf2");
            Assert.AreEqual(null, e2.ErrorMessage);
            //test renaming a column
            Response e3 = s.RenameColumn(user1, user1, board1, 0, "546123");
            Assert.AreEqual(null, e3.ErrorMessage);
            CleanUP();
        }

        public void MoveColumn()
        {
            SetUp();
            //test adding a column
            Response e1 = s.MoveColumn(user1, user1, board1, 1, 1);
            Assert.AreEqual(null, e1.ErrorMessage);
            //adding column with the same name-suppose to work
            Response e2 = s.MoveColumn(user1, user1, board1, 2, -2);
            Assert.AreEqual(null, e2.ErrorMessage);
            //adding column at the end of the list
            Response e3 = s.MoveColumn(user1, user1, board1, 2, -3);
            Assert.AreEqual(e3.ErrorMessage, e3.ErrorMessage);
            CleanUP();
        }
    }
}