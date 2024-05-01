namespace Unibrics.Core.Tests
{
    using System;
    using System.Diagnostics;
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

        [SetUp]
        public void SetUp()
        {
            var provider = Substitute.For<IInstanceProvider>();
            
            provider.GetInstance<LongCommand>().Returns(new LongCommand());
            tree = new ExecutionTreeBuilder(provider).CreateTree();
        }

        [Test]
        public async Task _01Test()
        {
            
        }
        
        [Test]
        public async Task _01CommandsShouldRunInParallel()
        {
            var sw = new Stopwatch();
            sw.Start();
            await tree.AddCommand<LongCommand>()
                .AddCommand<LongCommand>()
                .AddCommand<LongCommand>()
                .Execute();

           // throw new Exception();
           Debug.Log($"{sw.ElapsedMilliseconds}");
           Debug.Log(Thread.CurrentThread.ManagedThreadId + " " + LongCommand.A);
        }
    }

    class LongCommand : ExecutableCommand
    {
        public static int A = 0;

        protected override void ExecuteInternal()
        {
            A++;
            Debug.Log("Command Execute " + Thread.CurrentThread.ManagedThreadId);

            Retain();
            Thread.Sleep(500);
            ReleaseAndComplete();
        }
    }
}