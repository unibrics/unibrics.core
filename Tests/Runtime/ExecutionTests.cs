namespace Unibrics.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DI;
    using Execution;
    using NSubstitute;
    using NUnit.Framework;
    using Debug = UnityEngine.Debug;

    [TestFixture]
    public class ExecutionTests
    {
        private IExecutionTree tree;

        private const int MainThreadId = 1;

        private int ThreadId => Thread.CurrentThread.ManagedThreadId;

        [SetUp]
        public void SetUp()
        {
            var provider = Substitute.For<IInstanceProvider>();
            
            provider.GetInstance<LongCommand>().Returns(new LongCommand(null));
            provider.GetInstance<LongCommandA>().Returns(new LongCommandA(null));
            provider.GetInstance<LongCommandB>().Returns(new LongCommandB(null));
            provider.GetInstance<LongCommandC>().Returns(new LongCommandC(null));
            provider.GetInstance<LongCommandD>().Returns(new LongCommandD(null));
            provider.GetInstance<MainThreadCommandA>().Returns(new MainThreadCommandA(null));
            tree = new ExecutionTreeBuilder(provider).CreateTree();
        }

        [Test]
        public async Task _01Test()
        {
            
        }
        
        [Test]
        public async Task _01CommandsShouldRunInParallel()
        {
            ThreadPool.GetMaxThreads(out var i, out var x);
            var sw = new Stopwatch();
            sw.Start();
            
            tree.AddCommand<LongCommand>();
            tree.AddCommand<LongCommand>();
            tree.AddCommand<LongCommand>();
            await tree.Execute();

           // throw new Exception();
           Debug.Log($"{sw.ElapsedMilliseconds} - {i}/{x}");
           Debug.Log(Thread.CurrentThread.ManagedThreadId + " " + LongCommand.A);
        }
        
        [Test]
        public async Task _02Commands_ShouldWaitForRequisites()
        {
            var sw = new Stopwatch();
            sw.Start();
            
            tree.AddCommand<LongCommandA>();
            tree.AddCommand<LongCommandB>();
            tree.AddCommand<LongCommandC>();
            
            tree.AddCommand<LongCommand>()
                .After<LongCommandA>()
                .After<LongCommandB>()
                .After<LongCommandC>()
                ;
            
            tree.AddCommand<LongCommandD>()
                .After<LongCommandA>()
                .After<LongCommandB>()
                .After<LongCommandC>()
                .After<LongCommand>()
                ;
            await tree.Execute();
            //await Task.Delay(1000);// tree.Execute();

            // throw new Exception();
            Debug.Log($"{sw.ElapsedMilliseconds}");
        }

        [Test]
        public async Task _03MainThreadRequirement_IsKept()
        {
            var id = 0;
            tree.AddCommand<LongCommandA>();
            tree.AddCommand(new CallbackCommand(() => id = Thread.CurrentThread.ManagedThreadId))
                .InTheMainThread();
            tree.AddCommand<LongCommandC>();

            await tree.Execute();
            Assert.That(id, Is.EqualTo(MainThreadId));
        }

        [Test]
        public async Task _04SyncPoints_ShouldWork()
        {
            var list = new List<int>();
            tree.AddCommand(new CallbackCommand(() => list.Add(1)));
            tree.AddSyncPoint();
            
            tree.AddCommand(new CallbackCommand(() => list.Add(2)));
            tree.AddSyncPoint();
            
            tree.AddCommand(new CallbackCommand(() => list.Add(3)));
            tree.AddSyncPoint();
            
            tree.AddCommand(new CallbackCommand(() => list.Add(4)));
            tree.AddSyncPoint();

            await tree.Execute();
            Debug.Log($"{string.Join(",", list.Select(i => i))}");
            CollectionAssert.AreEqual(new List<int>(){1,2,3,4}, list);
        }
        
        [Test]
        public async Task _05MainThread_AfterBackground_RequirementIsKept()
        {
            var idMain = 0;
            var idBack = 0;
            tree.AddCommand(new LongCommandA(() => idBack = ThreadId));
            tree.AddCommand(new CallbackCommand(() => idMain = ThreadId))
                .After<LongCommandA>()
                .InTheMainThread();
            tree.AddCommand<LongCommandC>();

            await tree.Execute();
            Assert.That(idMain, Is.EqualTo(MainThreadId));
            Assert.That(idBack, Is.Not.EqualTo(MainThreadId));
        }

        [Test]
        public async Task _06MainThreadSequenceShouldWork()
        {
            var list = new List<int>();
            var id = 0;
            tree.AddCommand(new CallbackCommand(() => list.Add(ThreadId))).InTheMainThread();
            tree.AddCommand(new CallbackCommand(() => list.Add(ThreadId))).InTheMainThread();
            tree.AddCommand(new CallbackCommand(() => list.Add(ThreadId))).InTheMainThread();
            tree.AddCommand(new CallbackCommand(() => list.Add(ThreadId))).InTheMainThread();
            tree.AddCommand(new CallbackCommand(() => id = ThreadId));

            await tree.Execute();
            
            CollectionAssert.AreEqual(new List<int>(){1,1,1,1}, list);
            Assert.That(id, Is.Not.EqualTo(MainThreadId));
        }

        [Test]
        public async Task _07Sequences_ShouldBeProcessedBySingleThread()
        {
            var list = new List<int>();
            tree.AddCommand(new LongCommandA(() => list.Add(ThreadId)));
            tree.AddCommand(new LongCommandB(() => list.Add(ThreadId))).After<LongCommandA>();
            tree.AddCommand(new LongCommandC(() => list.Add(ThreadId))).After<LongCommandB>();
            tree.AddCommand(new LongCommandD(() => list.Add(ThreadId))).After<LongCommandC>();

            await tree.Execute();
            Assert.That(list.Distinct().Count(), Is.EqualTo(1));
        }
    }

    class LongCommand : ExecutableCommand
    {
        public static int A = 0;

        private readonly Action callback;

        public LongCommand(Action callback)
        {
            this.callback = callback;
        }

        protected override void ExecuteInternal()
        {
            A++;
            Debug.Log($"Command Execute {GetType().Name} " + Thread.CurrentThread.ManagedThreadId);
            callback?.Invoke();

            Retain();
            Thread.Sleep(500);
            ReleaseAndComplete();
        }
    }

    class LongCommandA : LongCommand
    {

        public LongCommandA(Action callback) : base(callback)
        {
        }
    }
    
    class LongCommandB : LongCommand
    {
        public LongCommandB(Action callback) : base(callback)
        {
        }
    }
    
    class LongCommandC : LongCommand
    {
        public LongCommandC(Action callback) : base(callback)
        {
        }
    }
    
    class LongCommandD : LongCommand
    {
        public LongCommandD(Action callback) : base(callback)
        {
        }
    }

    class CallbackCommand : ExecutableCommand
    {
        private Action action;

        public CallbackCommand(Action action)
        {
            this.action = action;
        }

        protected override void ExecuteInternal()
        {
            var thread = Thread.CurrentThread.ManagedThreadId;
            Debug.Log($"Executing {GetType().Name} in Thread#{thread}");
            action();
        }
    }

    class MainThreadCommandA : LongCommand
    {
        protected override void ExecuteInternal()
        {
            base.ExecuteInternal();
            Assert.That(Thread.CurrentThread.ManagedThreadId, Is.EqualTo(1));
        }

        public MainThreadCommandA(Action callback) : base(callback)
        {
        }
    }
}