using ContextMenu.Helpers;

namespace ContextMenu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                DisplayMenu();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeConstants.WM_COMMAND:
                    switch (m.WParam.ToInt64() & 0xFFFF) // LOWORD(wParam)
                    {
                        case NativeConstants.WM_USER + 1:
                            MessageBox.Show("Item 1");
                            break;
                        case NativeConstants.WM_USER + 2:
                            MessageBox.Show("Item 2");
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                DisplayMenu();
        }

        private void DisplayMenu()
        {
            IntPtr m = NativeInvoke.CreatePopupMenu();
            NativeInvoke.InsertMenu(m, 0, NativeConstants.MF_BYPOSITION | NativeConstants.MF_STRING, NativeConstants.WM_USER + 1, "First Item");
            NativeInvoke.InsertMenu(m, 1, NativeConstants.MF_BYPOSITION | NativeConstants.MF_STRING, NativeConstants.WM_USER + 2, "Second Item");

            NativeInvoke.TrackPopupMenuEx(m, 0, Cursor.Position.X, Cursor.Position.Y, this.Handle, IntPtr.Zero);
        }
    }
}