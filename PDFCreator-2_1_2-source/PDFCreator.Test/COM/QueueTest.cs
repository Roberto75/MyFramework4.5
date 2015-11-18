using System;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using pdfforge.PDFCreator;
using pdfforge.PDFCreator.COM;

namespace PDFCreator.Test.COM
{
   /* REWROTE THE TESTS */
    [TestFixture]
    class QueueTest
    {
        private Queue _queue;
  
        private void CreateTestPages(int n)
        {
            for (int i = 0; i < n; i++)
            {
                JobInfoQueue.Instance.AddTestPage();
            }
        }

        [SetUp]
        public void SetUp()
        {
            _queue = new Queue();
            _queue.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _queue.ReleaseCom();
        }

        [Test]
        [ExpectedException(typeof(COMException))]
        public void GetJobByIndex_IfIndexOutOfRange_ThrowsCOMException()
        {
            if (_queue == null) 
                return;

            CreateTestPages(3);
            _queue.GetJobByIndex(3);
        }

        [Test]
        [ExpectedException(typeof(COMException))]
        public void MergeAllJobs_IfQueueEmpty_ThrowsCOMException()
        {
            _queue.MergeAllJobs();
        }


        [Test]
        public void MergeAllJobs_IfQueueCountGreater1_QueueCountEquals1()
        {
            CreateTestPages(4);
            _queue.MergeAllJobs();

            int jobCount = _queue.Count;
            
            Assert.AreEqual(1, jobCount);
        }

        [Test]
        public void MergeAllJobs_IfQueueCountExactly1_QueueCountEquals1()
        {
            CreateTestPages(1);
            _queue.MergeAllJobs();

            int jobCount = _queue.Count;

            Assert.AreEqual(1,jobCount);
        }

        [Test]
        public void WaitForJobs_IfLessJobsEnteredThanExpected_ReturnsFalse()
        {
            var aThread = new Thread(() => CreateTestPages(5));
            aThread.Start();
            var hasTooFewEntered = !_queue.WaitForJobs(6, 1);

            Console.Write(_queue.Count);

            Assert.IsTrue(hasTooFewEntered);
        }

        [Test]
        public void WaitForJobs_IfTimeoutOver_ReturnFalse()
        {
            var isNotTimedOut = _queue.WaitForJobs(2, 1);

            Assert.IsFalse(isNotTimedOut);
        }
    }
}
