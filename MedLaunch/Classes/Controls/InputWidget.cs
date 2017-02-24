﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedLaunch.Classes.Controls
{
    public sealed class InputWidget : TextBox
    {
        /*
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private string _wasPressed = string.Empty;


        public InputWidget()
        {
            //ContextMenu = new ContextMenu();
            _timer.Tick += Timer_Tick;
            ClearBindings();
            AutoTab = true;
            //Cursor = Cursors.Arrow;
        }

        public bool AutoTab { get; set; }
        public string WidgetName { get; set; }

        public string Bindings
        {
            get
            {
                return Text;
            }

            set
            {
                ClearBindings();
                var newBindings = value.Trim().Split(',');
                //_bindings.AddRange(newBindings);
                UpdateLabel();
            }
        }

        [DllImport("user32")]
        private static extern bool HideCaret(IntPtr hWnd);

        protected override void PreviewMouseUp(MouseEventArgs e)
        {
            HideCaret(Handle);
            base.OnMouseClick(e);
        }

        public void ClearAll()
        {
            ClearBindings();
            Clear();
        }

        private void ClearBindings()
        {
            _bindings.Clear();
        }

        protected override void PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _timer.Start();

            _wasPressed = Input.Instance.GetNextBindEvent();
            //BackColor = Color.FromArgb(unchecked((int)0xFFC0FFFF)); // Color.LightCyan is too light on Windows 8, this is a bit darker
        }

        protected override void OnLeave(EventArgs e)
        {
            _timer.Stop();
            UpdateLabel();
            BackColor = SystemColors.Window;
            base.OnLeave(e);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ReadKeys();
        }

        public void EraseMappings()
        {
            ClearBindings();
            Text = string.Empty;
        }

        /// <summary>
        /// sets a binding manually. This may not be implemented quite right.
        /// </summary>
        public void SetBinding(string bindingStr)
        {
            _bindings.Add(bindingStr);
            UpdateLabel();
            Increment();
        }

        /// <summary>
		/// Poll input events and apply processing related to accepting that as a binding
		/// </summary>
		private void ReadKeys()
        {
            Input.Instance.Update();
            var bindingStr = Input.Instance.GetNextBindEvent();
            if (!string.IsNullOrEmpty(_wasPressed) && bindingStr == _wasPressed)
            {
                return;
            }

            if (bindingStr != null)
            {

                //has special meaning for the binding UI system (clear it).
                //you can set it through the special bindings dropdown menu
                if (bindingStr == "Escape")
                {
                    EraseMappings();
                    Increment();
                    return;
                }

                //seriously, we refuse to allow you to bind this to anything else.
                if (bindingStr == "Alt+F4")
                {
                    return;
                }

                //ignore special bindings
                foreach (var spec in SpecialBindings)
                    if (spec.BindingName == bindingStr)
                        return;

                if (!IsDuplicate(bindingStr))
                {
                    if (AutoTab)
                    {
                        ClearBindings();
                    }

                    _bindings.Add(bindingStr);
                }

                _wasPressed = bindingStr;
                UpdateLabel();
                Increment();
            }
        }

        private bool IsDuplicate(string binding)
        {
            return _bindings.FirstOrDefault(x => x == binding) != null;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Modifiers == Keys.Alt)
            {
                base.OnKeyUp(e);
            }

            _wasPressed = string.Empty;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Modifiers == Keys.Alt)
            {
                base.OnKeyDown(e);
                return;
            }

            e.Handled = true;
        }

        // Advances to the next widget depending on the autotab setting
        public void Increment()
        {
            if (AutoTab)
            {
                CompositeWidget.TabNext();
            }
        }

        public void Decrement()
        {
            if (AutoTab)
            {
                Parent.SelectNextControl(this, false, true, true, true);
            }
        }

        public void UpdateLabel()
        {
            Text = string.Join(",", _bindings.Where(str => !string.IsNullOrWhiteSpace(str)));
            CompositeWidget.RefreshTooltip();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0201: // WM_LBUTTONDOWN
                    Focus();
                    return;
                case 0x0203: // WM_LBUTTONDBLCLK
                case 0x0204: // WM_RBUTTONDOWN
                case 0x0205: // WM_RBUTTONUP
                case 0x0206: // WM_RBUTTONDBLCLK
                    return;
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                Decrement();
            }
            else
            {
                Increment();
            }

            base.OnMouseWheel(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            HideCaret(Handle);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return !(keyData.ToString() == "F4" || keyData.ToString().Contains("Alt"));
        }
        */
    }
}
