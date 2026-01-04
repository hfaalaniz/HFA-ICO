using System;
using System.Windows.Forms;


namespace HFA_ICO
{
    public class InputBoxResult
    {
        public DialogResult Result { get; set; }
        public string Input { get; set; }
    }

    public abstract class MsgBox
    {
        // MessageBox methods
        public static DialogResult Box(string text)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text))
                result = msgForm.ShowDialog();
            return result;
        }

        public static DialogResult Box(string text, string caption)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption))
                result = msgForm.ShowDialog();
            return result;
        }

        public static DialogResult Box(string text, string caption, MessageBoxButtons buttons)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption, buttons))
                result = msgForm.ShowDialog();
            return result;
        }

        public static DialogResult Box(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption, buttons, icon))
                result = msgForm.ShowDialog();
            return result;
        }

        public static DialogResult Box(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption, buttons, icon, defaultButton))
                result = msgForm.ShowDialog();
            return result;
        }

        public static DialogResult Box(IWin32Window owner, string text)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text))
                result = msgForm.ShowDialog(owner);
            return result;
        }

        public static DialogResult Box(IWin32Window owner, string text, string caption)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption))
                result = msgForm.ShowDialog(owner);
            return result;
        }

        public static DialogResult Box(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption, buttons))
                result = msgForm.ShowDialog(owner);
            return result;
        }

        public static DialogResult Box(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption, buttons, icon))
                result = msgForm.ShowDialog(owner);
            return result;
        }

        public static DialogResult Box(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult result;
            using (var msgForm = new frmMsgBox(text, caption, buttons, icon, defaultButton))
                result = msgForm.ShowDialog(owner);
            return result;
        }

        // InputBox methods
        public static InputBoxResult Input(string prompt)
        {
            using (var inputForm = new frmInputBox(prompt))
            {
                var result = inputForm.ShowDialog();
                return new InputBoxResult { Result = result, Input = inputForm.InputText };
            }
        }

        public static InputBoxResult Input(string prompt, string caption)
        {
            using (var inputForm = new frmInputBox(prompt, caption))
            {
                var result = inputForm.ShowDialog();
                return new InputBoxResult { Result = result, Input = inputForm.InputText };
            }
        }

        public static InputBoxResult Input(string prompt, string caption, string defaultResponse)
        {
            using (var inputForm = new frmInputBox(prompt, caption, defaultResponse))
            {
                var result = inputForm.ShowDialog();
                return new InputBoxResult { Result = result, Input = inputForm.InputText };
            }
        }

        public static InputBoxResult Input(IWin32Window owner, string prompt)
        {
            using (var inputForm = new frmInputBox(prompt))
            {
                var result = inputForm.ShowDialog(owner);
                return new InputBoxResult { Result = result, Input = inputForm.InputText };
            }
        }

        public static InputBoxResult Input(IWin32Window owner, string prompt, string caption)
        {
            using (var inputForm = new frmInputBox(prompt, caption))
            {
                var result = inputForm.ShowDialog(owner);
                return new InputBoxResult { Result = result, Input = inputForm.InputText };
            }
        }

        public static InputBoxResult Input(IWin32Window owner, string prompt, string caption, string defaultResponse)
        {
            using (var inputForm = new frmInputBox(prompt, caption, defaultResponse))
            {
                var result = inputForm.ShowDialog(owner);
                return new InputBoxResult { Result = result, Input = inputForm.InputText };
            }
        }
    }
}