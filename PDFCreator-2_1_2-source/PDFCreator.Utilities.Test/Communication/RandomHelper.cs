using System;
using System.Threading;

namespace pdfforge.PDFCreator.Utilities.Test.Communication
{
    internal static class RandomHelper
    {
        private static int _seedCounter = new Random().Next();

        [ThreadStatic]
        private static Random _rng;

        public static Random Instance
        {
            get
            {
                if (_rng == null)
                {
                    int seed = Interlocked.Increment(ref _seedCounter);
                    _rng = new Random(seed);
                }
                return _rng;
            }
        }
    }
}
