using System.IO;

namespace Bolt.Extensions {
  internal static class StringReaderExtensions {
    public static void ReadWhitespace(this StringReader sr) {
      int r;
      while((r = sr.Peek()) != -1) {
        switch(r) {
          case 32:
          case 10:
          case 13:
          case 9:
            sr.Read();
            break;

          default:
            return;
        }
      }
    }
  }
}
