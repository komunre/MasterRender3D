using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MasterRender3D.RenderMath;

namespace MasterRender3D
{
    public class ANSIHelper
    {
        public Vector2i CurrentPos = new Vector2i();

        public int CurrentWidth;
        public int CurrentHeight;

        protected StringBuilder Buffer = new StringBuilder();

        public ANSIHelper()
        {
            try
            {
                Console.WindowWidth = 120;
                Console.WindowHeight = 50;

                CurrentWidth = Console.WindowWidth;
                CurrentHeight = Console.WindowHeight;
            }
            catch
            {
                CurrentWidth = 120;
                CurrentHeight = 50;
            }
        }
        
        public void Flush()
        {
            GoToImmediate(new Vector2i(0, 0));
            Console.Write(Buffer);
            Buffer.Clear();
        }

        public void Write(string text)
        {
            //Console.Write(text);
            Buffer.Append(text);
            CurrentPos.x += text.Length;
            if (CurrentPos.x > CurrentWidth) {
                CurrentPos.x -= CurrentWidth;
                CurrentPos.y++;
            }
        }

        public void WriteNoAdvance(string text)
        {
            Buffer.Append(text);
        }

        public void FullClear()
        {
            CSIStartImmediate();
            Console.Write("0J");
        }

        /*public void Void()
        {
            for (var i = 0; i < CurrentWidth*CurrentHeight; i++)
            {
                Console.Write(" ");
            }
        }*/

        public void HideFrame()
        {
            GoTo(new Vector2i(0, 0));
            CSIStart();
            WriteNoAdvance("8m");
        }

        public void UnhideFrame()
        {
            GoTo(new Vector2i(0, 0));
            CSIStart();
            WriteNoAdvance("0m");
        }

        public void GoTo(Vector2i pos)
        {
            CSIStart();
            WriteNoAdvance(pos.y.ToString() + ';' + pos.x.ToString() + 'H');
            CurrentPos = pos;
        }

        public void GoToImmediate(Vector2i pos)
        {
            CSIStartImmediate();
            Console.Write(pos.y.ToString() + ';' + pos.x.ToString() + 'H');
            CurrentPos = pos;
        }

        public void SetStyle(int style)
        {
            CSIStart();
            WriteNoAdvance(style.ToString() + 'm');
        }

        public void CarriageReturn()
        {
            WriteNoAdvance(((char)0x0D).ToString());
        }

        public void Beep()
        {
            WriteNoAdvance(((char)0x07).ToString());
        }

        protected void CSIStartImmediate()
        {
            EscapeStartImmediate();
            Console.Write("[");
        }

        protected void CSIStart()
        {
            EscapeStart();
            WriteNoAdvance("[");
        }

        protected void EscapeStartImmediate()
        {
            Console.Write((char)0x1B);
        }

        protected void EscapeStart()
        {
            WriteNoAdvance(((char)0x1B).ToString());
        }
    }

    public enum ANSISTyle
    {
        None = 0,
        Bold,
        Faint,
        Italic,
        Underline,
        SlowBlink,
        FastBlink,
        Invert,
        Hide,
        CrossedOut,
        PrimaryFont,
        AlternativeFont,
        Fraktur = 20,
        DoublyUnderlinedOrNotBold,
        NormalIntensity,
        NeitherItalicNorBlackletter,
        NotUnderlined,
        NotBlinking,
        ProportionalSpacing,
        NotReversed,
        Reveal,
        NotCrossedOut,
        SetForegroundColor,
        SetForegroundColor8bit = 38,
        DefaultForegroundColor,
        SetBackgroundColor,
        SetBackgroundColor8bit = 48,
        DefaultBackgroundColor,
        DisableProportionalSpacing,
        Framed,
        Encircled,
        Overlined,
        NeitherFramedNorEncircled,
        NotOverlined,
        SetUnderlineColor,
        DefaultUnderlineColor,
        IdeogramUnderlineOrRightSideLine,
        IdeogramDoubleUnderlineOrDoubleRightSideLine,
        IdeogramOverlineOrLeftSideLine,
        IdeogramDoubleOverlineOrDoubleLeftSideLine,
        IdeogramStressMarking,
        NoIdeogramAttributes,
        Superscript,
        Subscript,
        NeitherSubscriptNorSuperscript,
        SetBrightForegoundColor,
        SetBrightBackgroundColor = 100,
    }
}
