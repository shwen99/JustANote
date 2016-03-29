using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JustANote
{
    struct TextSelection
    {
        public TextSelection(TextBox textBox)
        {
            _textBox = textBox;
        }

        private readonly TextBox _textBox;

        /// <summary>
        /// 将当前选择扩展到整行：
        ///  * 如果没有选区，则选择当前行
        ///  * 如果选择没有跨行，则选择当前行
        ///  * 选择包含选择区域的整行
        /// </summary>
        /// <returns></returns>
        public void ExtendSelection()
        {
            ExtendToFullLine();
        }

        public int TopLine
        {
            get
            {
                return _textBox.GetLineFromCharIndex(_textBox.SelectionStart);
            }
        }

        public SelectionPoint TopPoint
        {
            get
            {
                var pos = _textBox.SelectionStart;
                var line = _textBox.GetLineFromCharIndex(pos);
                var pos0 = _textBox.GetFirstCharIndexFromLine(line);
                var col = pos - pos0;
                return new SelectionPoint(pos, line, col);
            }
        }

        public int BottomLine
        {
            get
            {
                return _textBox.GetLineFromCharIndex(_textBox.SelectionStart + _textBox.SelectionLength);
            }
        }

        public SelectionPoint BottomPoint
        {
            get
            {
                var pos = _textBox.SelectionStart + _textBox.SelectionLength;
                var line = _textBox.GetLineFromCharIndex(pos);
                var pos0 = _textBox.GetFirstCharIndexFromLine(line);
                var col = pos - pos0;
                return new SelectionPoint(pos, line, col);
            }
        }

        public bool IsEmpty
        {
            get { return _textBox.SelectionLength == 0; }
        }

        public void ExtendToFullLine()
        {
            var line1 = TopLine;
            var line2 = BottomLine;

            if (IsEmpty || !BottomPoint.AtStartOfLine)
            {
                line2 += 1;
            }

            var pos0 = _textBox.GetFirstCharIndexFromLine(line1);
            var pos1 = _textBox.GetFirstCharIndexFromLine(line2);

            if (pos1 == -1)
            {
                pos1 = _textBox.TextLength;
            }

            _textBox.Select(pos0, pos1-pos0);
        }

        public bool IsFullLine()
        {
            return !IsEmpty && TopPoint.AtStartOfLine && (BottomPoint.AtStartOfLine || BottomPoint.CharIndex == _textBox.TextLength);
        }

        public struct SelectionPoint
        {
            public SelectionPoint(int charIndex, int line, int column)
            {
                CharIndex = charIndex;
                Line = line;
                Column = column;
            }

            public readonly int CharIndex;
            public readonly int Line;
            public readonly int Column;

            public bool AtStartOfLine
            {
                get { return Column == 0; }
            }
        }
    }
}
