namespace Bolt {
  internal class StringReader {
    private int _length;
    private string _s;
    private int _i;

    public StringReader(string s) {
      this._length = s.Length;
      this._i = -1;
      this._s = s;
    }

    public virtual int Peek() {
      ReadWhitespace();
      return More() ? this._s[this._i + 1] : -1;
    }

    public virtual int Read() {
      ReadWhitespace();
      return More() ? this._s[++this._i] : -1;
    }

    public virtual int Read(char[] buffer, int index, int count) {
      int read = 0;
      for(int i = 0; i < count; i++) {
        int c = Read();
        if(c != -1) {
          buffer[index++] = (char)c;
          read++;
        } else {
          return read;
        }
      }
      return read;
    }

    private bool More() {
      return this._i + 1 < this._length;
    }

    private void ReadWhitespace() {
      while(More() && char.IsWhiteSpace(this._s[this._i + 1])) {
        this._i++;
      }
    }
  }
}
