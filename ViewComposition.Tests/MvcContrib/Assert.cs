using NUnit.Framework;

namespace ViewComposition.Tests.MvcContrib {
    public static class Assert {
        public static void ShouldNotBeNull(this object Actual, string message) {
            if (Actual == null) {
                throw new AssertionException(message);
            }
        }
        public static void ShouldEqual(this object actual, object expected, string message) {
            if (actual == null && expected == null) {
                return;
            }
            if (!actual.Equals(expected)) {
                throw new AssertionException(message);
            }
        }
    }
}