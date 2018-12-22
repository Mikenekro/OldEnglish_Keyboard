using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OldEnglishKeyboard
{
    public enum Modifiers
    {
        None = 0x0000,
        //Alt = 0x0001,
        Control = 0x0002,
        Shift = 0x0004
        //, Win = 0x0008
    }

    public class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private int modifier;
        private int key;
        private IntPtr hWnd;
        private int id;
        

        public NativeMethods(int modifiers, Keys key, Form f)
        {
            this.modifier = modifiers;
            this.key = (int)key;
            this.hWnd = f.Handle;
            id = this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }


        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }
        public bool Unregister()
        {
            return UnregisterHotKey(hWnd, id);
        }
    }
}
